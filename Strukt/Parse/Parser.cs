using System.Data;
using Strukt.Lex;
using Strukt.Parse.ParseNodeTypes;

namespace Strukt.Parse
{
    public class Parser
    {
        public ModuleNode ParseModule(Token[] tokens)
        {
            ParseCursor _cursor = new ParseCursor(tokens);

            var result = new ModuleNode();
            while (_cursor.Advance())
            {
             
                switch (_cursor.Current.Kind)
                {
                    case TokenKind.Reserved:
                        ParseGlobalScope(_cursor, result);
                        break;
                    default:
                        throw new DataException("Unexpected token: " + _cursor.Current);
                }   
            }

            return result;
        }

        private void ParseGlobalScope(ParseCursor cursor, ModuleNode result)
        {
            var currToken = cursor.Current;
            switch (currToken.Text)
            {
                case "class":
                    ClassNode classModule = ParseClassDeclaration(cursor);
                    result.Children.Add(classModule);
                    break;
            }
        }

        private ClassNode ParseClassDeclaration(ParseCursor cursor)
        {
            var result = new ClassNode();
            cursor.Advance();
            result.Name = cursor.Current.Text;
            cursor.Advance();
            return result;
        }
    }
}