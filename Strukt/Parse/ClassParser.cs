using Strukt.Lex;

namespace Strukt.Parse;

class ClassParser
{
    
    public static void ParseClass(Scanner scanner, CompilationUnit result,
        VisibilityAndStatusPermissions visibilityAndStatusPermissions)
    {
        scanner.Advance();
        Token[] namespaceTokens = scanner.ReadUntil("{");
        string[] definitions = namespaceTokens.SelectToArray(tok => tok.Text);
        var definitionName = definitions.JoinTexts();

        result.Definitions[definitionName] = new ClassDefinition(definitionName, visibilityAndStatusPermissions);
        visibilityAndStatusPermissions.Clear();
        ParseClassBody(scanner, result);
    }

    private static void ParseClassBody(Scanner scanner, CompilationUnit result)
    {
        
    }

}