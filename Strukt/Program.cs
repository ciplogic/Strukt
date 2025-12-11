using System;
using System.IO;
using Strukt.Lex;

namespace Strukt
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(
                @"C:\codehub\Strukt\Strukt\", "*.cs", SearchOption.AllDirectories);
            var scanner = new Scanner();
            foreach (var fileName in files)
            {
                try
                {
                    var content = File.ReadAllText(fileName);
                    var contentChars = content.ToCharArray() ;
                    scanner.Code = contentChars;
                    Token firstToken = scanner.Advance();
                    Console.WriteLine("Success: " + fileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Fail on:" + fileName+ "\n"+e.Message);
                 
                }}
        }
    }
}
