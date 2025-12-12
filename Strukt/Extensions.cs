using System;
using System.Text;

namespace Strukt;

public static class Extensions
{
    public static TOut[] SelectToArray<TIn, TOut>(this TIn[] self, Func<TIn, TOut> selector)
        => self.Length == 0
            ? []
            : Array.ConvertAll(self, it => selector(it));

    public static string JoinTexts(this string[] texts)
    {
        StringBuilder sb = new ();
        foreach (string text in texts)
        {
            sb.Append(text);
        }

        return sb.ToString();
    }
}