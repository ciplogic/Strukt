using System.Linq;
using Strukt.Lex;

namespace Strukt.Parse;

public class CsMiniParser
{
    internal CompilationUnit Parse(Scanner scanner)
    {
        CompilationUnit result = new();
        do
        {
            Token currentToken = scanner.CurrentToken;
            if (currentToken.Kind == TokenKind.Eof)
            {
                break;
            }

            if (currentToken.Text == "using")
            {
                ParseUsingBlock(scanner, result);
            }
        } while (true);

        return result;
    }

    private static void ParseUsingBlock(Scanner scanner, CompilationUnit compilationUnit)
    {
        scanner.Advance();
        Token[] namespaceTokens = scanner.ReadUntil(";");
        string[] words = namespaceTokens.SelectToArray(tok => tok.Text);
        string allNamespaces = words.JoinTexts();
        string namespaceTrimmed = allNamespaces[..^1];
        compilationUnit.Usage.Add(namespaceTrimmed);
    }
}