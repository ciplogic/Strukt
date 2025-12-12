using System;
using System.Collections.Generic;
using System.Linq;

namespace Strukt.Lex;

public static class LexMatchers
{
    private static char[] EolnChars = { '\n', '\r' };

    public static int Eoln(ArraySegment<char> text)
    {
        return LexerExtensions.LengthMatchAll(text, ch => EolnChars.Contains(ch));
    }

    private static char[] SpaceChars = { ' ', '\t' };

    public static int Spaces(ArraySegment<char> text)
    {
        return LexerExtensions.LengthMatchAll(text, ch => SpaceChars.Contains(ch));
    }

    public static int IdentifierSpan(ReadOnlySpan<char> text)
    {
        return LexerExtensions.LengthMatchSpan(text,
            c => char.IsLetter(c) || c == '_',
            c => char.IsLetterOrDigit(c) || c == '_');
    }

    public static int Identifier(ArraySegment<char> text)
    {
        Span<char> span = text.AsSpan();
        return IdentifierSpan(span);
    }

    public static int Number(ArraySegment<char> text)
    {
        return LexerExtensions.LengthMatchAll(text, char.IsDigit);
    }

    public static int FieldIdentifier(ArraySegment<char> text)
    {
        Span<char> span = text.AsSpan();
        if (span[0] != '#')
            return 0;
        if (!char.IsLetter(span[1]))
            return 0;
        return 1 + IdentifierSpan(span.Slice(1));
    }

    public static int LineComment(ArraySegment<char> text)
    {
        Span<char> span = text.AsSpan();
        if (span[0] != '/' || span[1] != '/')
        {
            return 0;
        }

        for (int i = 2; i < span.Length; i++)
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
        "private", "public", "internal",
        "static",
        "using", "namespace", "class", "struct", "enum", "interface"
    }.StringsToArray();

    static int ReservedMatch(ArraySegment<char> text)
    {
        int idLen = Identifier(text);
        if (idLen == 0)
            return 0;
        int matchReserved = ScannerUtils.MatchAny(text, ReservedWords);
        return matchReserved == idLen ? matchReserved : 0;
    }

    static int MacroDefine(ArraySegment<char> text)
    {
        if (text[0] != '$')
            return 0;
        Span<char> slice = text.AsSpan().Slice(1);
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
    }.OrderBy(op => op.Length).ToArray().StringsToArray();

    static int OperatorMatch(ArraySegment<char> text)
    {
        return ScannerUtils.MatchAny(text, Operators);
    }

    private static readonly char[] QuoteChars = { '\'', '"', '`' };

    static int QuoteMatch(ArraySegment<char> text)
    {
        Span<char> span = text.AsSpan();
        char firstChar = span[0];
        if (!QuoteChars.Contains(firstChar))
            return 0;
        for (int i = 1; i < span.Length; i++)
        {
            if (span[i] == firstChar)
                return i + 1;
        }

        return -1;
    }

    public static (TokenKind kind, Func<ArraySegment<char>, int>)[] Matchers()
    {
        List<(TokenKind kind, Func<ArraySegment<char>, int>)> allMatchers =
            new List<(TokenKind kind, Func<ArraySegment<char>, int>)>
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