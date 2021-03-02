using System;
using System.Collections.Generic;
using System.Linq;

namespace Strukt.Lex
{
    public static class ScannerUtils
    {
        public static (TokenKind kind, int matchLen)? Match(string text, int start,
            (TokenKind kind, Func<string, int, int>)[] matchers)
        {
            foreach ((TokenKind kind, Func<string,int,int> match) matcher in matchers)
            {
                var matchLen = matcher.match(text, start);
                if (matchLen == 0)
                {
                    continue;
                }

                return (matcher.kind, matchLen);
            }

            return null;
        }

        public static char[][] StringsToArray(this string[] values)
        {
            var result = new List<char[]>(values.Length);
            result.AddRange(values.Select(it=>it.ToCharArray()));
            return result.ToArray();
        }
        
        public static int MatchAnyIndex(string text, int start, char[][] items)
        {
            ReadOnlySpan<char> span = text.AsSpan(start);
            for (int index = 0; index < items.Length; index++)
            {
                char[] item = items[index];
                var firstChar = item[0];
                if (span[0] != firstChar)
                    continue;
                if (span.Length < item.Length)
                    continue;
                var found = false;
                for (var i = 1; i < item.Length; i++)
                {
                    var currChar = item[i];
                    if (currChar != span[i])
                    {
                        found = true; 
                        break;
                    }
                }

                if (found)
                {
                    continue;
                }
                return index;
            }
            return -1;
        }
        
        public static int MatchAny(string text, int start, char[][] items)
        {
            var indexMatch = MatchAnyIndex(text, start, items);
            return indexMatch == -1 
                ? 0 
                : items[indexMatch].Length;
        }
    }
}