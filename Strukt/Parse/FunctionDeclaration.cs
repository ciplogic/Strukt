using System.Collections.Generic;
using Strukt.Lex;
using Strukt.TypeDescription;

namespace Strukt.Parse
{
    internal class FunctionDeclaration
    {
        public FunctionDeclaration(ParseCursor cursor)
        {
            if (cursor.Current.Kind == TokenKind.Identifier)
            {
                Name = cursor.Current.Text;
                cursor.Advance();
            }

            cursor.Advance("(");
            if (cursor.Current.Text == ")")
            {
                cursor.Advance();
            }
            if (cursor.Current.Text == "{")
                return;
            
            throw new System.NotImplementedException();
        }

        public string Name { get; set; }

        public ArgumentInfo ThisArg;

        public List<ArgumentInfo> Arguments
        {
            get;
        } = new();
    }

}