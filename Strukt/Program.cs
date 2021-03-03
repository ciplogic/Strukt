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
            Token[] tokens = scanner.Lex(content);
            Token[] simplified = scanner.Simplify(tokens);
            
            Console.WriteLine("Token count: " + simplified.Length);
        }
    }
}
