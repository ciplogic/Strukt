using System;
using System.IO;
using Strukt.Lex;

namespace Strukt
{
    class Program
    {
        static void Main(string[] args)
        {
            var scanner = new Scanner();
            var content = File.ReadAllText("main.stk");
            var tokens = scanner.Lex(content);
            Console.WriteLine("Token count: " + tokens.Length);
        }
    }
}
