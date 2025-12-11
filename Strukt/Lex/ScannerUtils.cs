using System;
using System.Collections.Generic;
using System.Linq;

namespace Strukt.Lex
{
    public static class ScannerUtils
    {
        public static (TokenKind kind, int matchLen)? Match(ArraySegment<char> text,
            (TokenKind kind, Func<ArraySegment<char>, int>)[] matchers)
        {
            foreach ((TokenKind kind, Func<ArraySegment<char>,int> match) matcher in matchers)
            {
                var matchLen = matcher.match(text);
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
        
        public static int MatchAnyIndex(ArraySegment<char> text, char[][] items)
        {
            ReadOnlySpan<char> span = text.AsSpan();
            for (int index = 0; index < items.Length; index++)
            {
                char[] item = items[index];
                var firstChar = item[0];
                if (span[0] != firstChar)
                    continue;
                if (span.Length < item.Length)
                    continue;
                if (span.StartsWith(item))
                {
                    return item.Length;
                }
            }
            return -1;
        }
        
        public static int MatchAny(ArraySegment<char> text, char[][] items)
        {
            var indexMatch = MatchAnyIndex(text, items);
            return indexMatch == -1 
                ? 0 
                : items[indexMatch].Length;
        }
    }
}