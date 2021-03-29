using System;
using System.Collections.Generic;
using System.Linq;

namespace Strukt.Lex
{
    public static class LexMatchers
    {
        static int LengthMatchSpan(ReadOnlySpan<char> text, Func<char, bool> firstCharMatch, Func<char, bool> nextChars)
        {
            if (!firstCharMatch(text[0]))
                return 0;
            for (var i = 1; i < text.Length; i++)
            {
                if (!nextChars(text[i]))
                {
                    return i;
                }
            }

            return text.Length;
        }

        static int LengthMatchAll(string text, int start, Func<char, bool> allChars)
        {
            var span = text.AsSpan(start);
            return LengthMatchSpan(span, allChars, allChars);
        }

        private static char[] EolnChars = {'\n', '\r'};

        public static int Eoln(string text, int start)
        {
            return LengthMatchAll(text, start, ch => EolnChars.Contains(ch));
        }

        private static char[] SpaceChars = {' ', '\t'};

        public static int Spaces(string text, int start)
        {
            return LengthMatchAll(text, start, ch => SpaceChars.Contains(ch));
        }

        public static int IdentifierSpan(ReadOnlySpan<char> text)
        {
            return LengthMatchSpan(text, 
                c => char.IsLetter(c) || c=='_', 
                c=>char.IsLetterOrDigit(c) || c == '_');
        }

        public static int Identifier(string text, int start)
        {
            var span = text.AsSpan(start);
            return IdentifierSpan(span);
        }

        public static int Number(string text, int start)
        {
            return LengthMatchAll(text, start, char.IsDigit);
        }

        public static int FieldIdentifier(string text, int start)
        {
            var span = text.AsSpan(start);
            if (span[0] != '#')
                return 0;
            if (!char.IsLetter(span[1]))
                return 0;
            return 1 + IdentifierSpan(span.Slice(1));
        }

        public static int LineComment(string text, int start)
        {
            var span = text.AsSpan(start);
            if (span[0] != '/' || span[1] != '/')
            {
                return 0;
            }

            for (var i = 2; i < span.Length; i++)
            {
                if (span[i] == '\n')
                {
                    return i;
                }
            }

            return span.Length;
        }

        private static readonly char[][] ReservedWords = new[]
        {
            "break",
            "case",
            "chan",
            "const",
            "continue",
            "default",
            "defer",
            "else",
            "falltrough",
            "for",
            "fn",
            "go",
            "goto",
            "if",
            "import",
            "package",
            "range",
            "return",
            "struct",
            "switch",
            "type",
            "var"
        }.StringsToArray();

        static int ReservedMatch(string text, int start)
        {
            int idLen = Identifier(text, start);
            if (idLen == 0)
                return 0;
            int matchReserved = ScannerUtils.MatchAny(text, start, ReservedWords);
            return matchReserved == idLen ? matchReserved : 0;
        }

        static int MacroDefine(string text, int start)
        {
            if (text[start] != '$')
                return 0;
            var slice = text.AsSpan().Slice(start + 1);
            return IdentifierSpan(slice) + 1;
        }

        private static readonly char[][] Operators = new[]
        {
            "+", "&", " +=", "&=", " &&", "==", "!=", "(", ")",
            "-", "|", " -=", "|=", " ||", "<", " <=", "[", "]",
            "*", "^", " *=", "^=", " <-", ">", " >=", "{", "}",
            "/", "<<", "/=", "<<=", "++", "=", ":=", ",", ";",
            "%", ">>", "%=", ">>=", "--", "!", "...", ":",
            "..", ".", "~",
            "&^", "&^=", 
            "?",
            "@",
        }.OrderBy(op=>op.Length).ToArray().StringsToArray();

        static int OperatorMatch(string text, int start)
        {
            return ScannerUtils.MatchAny(text, start, Operators);
        }

        private static readonly char[] QuoteChars = {'\'', '"', '`'};

        static int QuoteMatch(string text, int start)
        {
            var span = text.AsSpan(start);
            var firstChar = span[0];
            if (!QuoteChars.Contains(firstChar))
                return 0;
            for (var i = 1; i < span.Length; i++)
            {
                if (span[i] == firstChar)
                    return i + 1;
            }

            return -1;
        }

        public static (TokenKind kind, Func<string, int, int>)[] Matchers()
        {
            var allMatchers = new List<(TokenKind kind, Func<string, int, int>)>
            {
                (TokenKind.Eoln, Eoln),
                (TokenKind.Space, Spaces),
                (TokenKind.Comment, LineComment),
                (TokenKind.FieldIdentifier, FieldIdentifier),
                (TokenKind.Reserved, ReservedMatch),
                (TokenKind.Macro, MacroDefine),
                (TokenKind.Identifier, Identifier),
                (TokenKind.Operator, OperatorMatch),
                (TokenKind.Quote, QuoteMatch),
                (TokenKind.Number, Number),
            };

            return allMatchers.ToArray();
        }
    }
}