using System.IO;
using Strukt.Lex;

namespace Strukt.Parse
{
    public class ParseCursor
    {
        private readonly Token[] _tokens;
        public int Pos { get; set; }
        public Token Current
        {
            get => _tokens[Pos];
        }
        public ParseCursor(Token[] tokens)
        {
            _tokens = tokens;
        }

        public bool Advance(TokenKind expected = TokenKind.None)
        {
            if (expected != TokenKind.None)
            {
                if (expected != Current.Kind)
                {
                    throw new InvalidDataException($"Expected {expected} but got {Current}");
                }
            }
            Pos++;
            return Pos < _tokens.Length;
        }
    }
}