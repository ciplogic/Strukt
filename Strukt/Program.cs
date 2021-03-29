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
            var files = Directory.GetFiles(
                @"C:\Oss\v-master\", "*.v", SearchOption.AllDirectories);
            var scanner = new Scanner();
            foreach (var fileName in files)
            {
                try
                {
                    var content = File.ReadAllText(fileName);
                    Token[] tokens = scanner.Lex(content);
                    Token[] simplified = scanner.Simplify(tokens);
                    
                    Console.WriteLine("Success: " + fileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Fail on:" + fileName+ "\n"+e.Message);
                 
                }}
        }
    }
}
