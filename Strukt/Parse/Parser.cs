using System.Data;
using System.IO;
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
            do
            {

                switch (_cursor.Current.Kind)
                {
                    case TokenKind.Reserved:
                        ParseGlobalScope(_cursor, result);
                        continue;
                    default:
                        throw new DataException("Unexpected token: " + _cursor.Current);
                }

                _cursor.Advance();
            } while (true);
            return result;
        }

        private void ParseGlobalScope(ParseCursor cursor, ModuleNode result)
        {
            var currToken = cursor.Current;
            switch (currToken.Text)
            {
                case "package":
                    SetPackageName(cursor, result);
                    break;
                case "func":
                    SetFunctionDeclaration(cursor, result);
                    break;
                default:
                    throw new InvalidDataException("Unexpected reserved word: " + currToken);
            }
        }

        private void SetFunctionDeclaration(ParseCursor cursor, ModuleNode result)
        {
            var functionDeclaration = new FunctionDeclaration(cursor.ReadRow());

            throw new System.NotImplementedException();
        }

        private void SetPackageName(ParseCursor cursor, ModuleNode result)
        {
            cursor.Advance();
            result.Name = cursor.AdvanceToken().Text;
            cursor.Advance(TokenKind.Eoln);
        }
    }
}