using System;
using System.Linq;
using Strukt.Lex;

namespace Strukt.Parse;

public class CsMiniParser
{
    internal CompilationUnit Parse(Scanner scanner)
    {
        CompilationUnit result = new();
        VisibilityAndStatusPermissions visibilityAndStatusPermissions = new();
        do
        {
            Token currentToken = scanner.CurrentToken;
            if (currentToken.Kind == TokenKind.Eof)
            {
                break;
            }

            if (visibilityAndStatusPermissions.AddPermission(currentToken.Text))
            {
                scanner.Advance();
                continue;
            }

            if (currentToken.Kind == TokenKind.Reserved)
            {
                switch (currentToken.Text)
                {
                    case "using":
                        ParseUsingBlock(scanner, result);
                        break;
                    case "namespace":
                        ParseNamespace(scanner, result);
                        break;
                    case "class":
                        ClassParser.ParseClass(scanner, result, visibilityAndStatusPermissions);
                        break;
                    default:
                        throw new InvalidOperationException("Cannot parse token: " + currentToken.Text);
                }
            }
            else
            {
                throw new InvalidOperationException($"Unexpected token: {currentToken.Text}");
            }
        } while (true);

        return result;
    }

    private void ParseNamespace(Scanner scanner, CompilationUnit result)
    {
        scanner.Advance();
        Token[] namespaceTokens = scanner.ReadUntil(";");
        string[] words = namespaceTokens.SelectToArray(tok => tok.Text);
        string allNamespaces = words.JoinTexts();
        result.Namespace = allNamespaces;
    }

    private static void ParseUsingBlock(Scanner scanner, CompilationUnit compilationUnit)
    {
        scanner.Advance();
        Token[] namespaceTokens = scanner.ReadUntil(";");
        string[] words = namespaceTokens.SelectToArray(tok => tok.Text);
        string allNamespaces = words.JoinTexts();
        compilationUnit.Usage.Add(allNamespaces);
    }
}