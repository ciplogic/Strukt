using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

        public IEnumerable<string> Remaining
        {
            get
            {
                for (var i = Pos; i < _tokens.Length; i++)
                {
                    yield return _tokens[i].Text;
                }
            }
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

        public Token AdvanceToken()
        {
            Advance();
            return Current;
        }

        public ParseCursor TokensUntil(TokenKind terminatingToken)
        {
            var result = new List<Token>();
            do
            {
                if (Pos >= _tokens.Length || Current.Kind == terminatingToken)
                {
                    Advance();
                    var resultTokens = result.ToArray();
                    return new ParseCursor(resultTokens);
                }
                result.Add(AdvanceToken());
            } while (true);
        }

        public bool Advance(string expected)
        {
            Debug.Assert(expected == Current.Text);
            return Advance();
        }

        public ParseCursor ReadRow() 
            => TokensUntil(TokenKind.Eoln);
    }
}