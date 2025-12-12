using System;

namespace Strukt.Lex;

static class LexerExtensions
{
    public static int LengthMatchSpan(ReadOnlySpan<char> text, Func<char, bool> firstCharMatch,
        Func<char, bool> nextChars)
    {
        if (!firstCharMatch(text[0]))
            return 0;
        for (int i = 1; i < text.Length; i++)
        {
            if (!nextChars(text[i]))
            {
                return i;
            }
        }

        return text.Length;
    }

    internal static int LengthMatchAll(ArraySegment<char> text, Func<char, bool> allChars)
    {
        Span<char> span = text.AsSpan();
        return LengthMatchSpan(span, allChars, allChars);
    }

    public static int IndexOf(this ArraySegment<char> text, char item)
    {
        return text.AsSpan().IndexOf(item);
    }
}