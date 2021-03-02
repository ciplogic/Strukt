using System;
using System.Collections.Generic;
using System.IO;

namespace Strukt.Lex
{
    public class Scanner
    {
        public Token[] Lex(string text)
        {
            var result = new List<Token>();
            (TokenKind kind, Func<string, int, int>)[] matchers = LexMatchers.Matchers();
            for (var start = 0; start < text.Length;)
            {
                (TokenKind kind, int matchLen)? match = ScannerUtils.Match(text, start, matchers);

                ReportError(match, text, start);


                start += AddToken(text, match, start, result);
            }

            return result.ToArray();
        }

        private static int AddToken(string text, (TokenKind kind, int matchLen)? match, int start, List<Token> result)
        {
            var matchValue = match.Value;
            var tokenText = text.Substring(start, matchValue.matchLen);
            var token = new Token
            {
                Kind = matchValue.kind,
                Text = tokenText
            };
            result.Add(token);
            return matchValue.matchLen;
        }

        private static void ReportError((TokenKind kind, int matchLen)? match, string text, int start)
        {
            if (match != null)
            {
                return;
            }

            var startError = text.Substring(start);
            if (startError.Length > 100)
            {
                startError = startError.Substring(0, 100);
            }

            throw new InvalidDataException($"Invalid start text: '{startError}'");
        }
    }
}
