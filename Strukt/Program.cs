using System;
using System.IO;
using Strukt.Lex;
using Strukt.Parse;

namespace Strukt;

class Program
{
    static void Main(string[] args)
    {
        string[] files = Directory.GetFiles(
            @"C:\codehub\Strukt\Strukt\", "*.cs", SearchOption.AllDirectories);
        Scanner scanner = new Scanner();
        foreach (string fileName in files)
        {
            string content = File.ReadAllText(fileName);
            char[] contentChars = content.ToCharArray();
            scanner.Code = contentChars;
            CsMiniParser csMiniParser = new CsMiniParser();
            CompilationUnit compilationUnit = csMiniParser.Parse(scanner);


            Console.WriteLine("Success: " + fileName);
        }
    }
}