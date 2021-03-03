using System;
using System.IO;
using Strukt.Lex;
using Strukt.Parse;

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
            var parser = new Parser();
            var module = parser.ParseModule(simplified);
            
            Console.WriteLine("Token count: " + simplified.Length);
        }
    }
}
