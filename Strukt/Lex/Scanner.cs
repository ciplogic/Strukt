using System;
using System.Collections.Generic;
using System.IO;

namespace Strukt.Lex;

public class Scanner
{
    public ArraySegment<char> Code { get; set; } = [];

    public Token Peek()
    {
        if (Code.Count == 0)
            return new Token()
            {
                Kind = TokenKind.Eof,
                Text = ""
            };
        (TokenKind kind, Func<ArraySegment<char>, int>)[] matchers = LexMatchers.Matchers();
        (TokenKind kind, int matchLen)? match = null;
        foreach ((TokenKind kind, Func<ArraySegment<char>, int>) matcher in matchers)
        {
            match = ScannerUtils.Match(Code, matchers);
            if (match != null) break;
        }

        if (!match.HasValue)
        {
            ReportError(null, Code);
        }

        return BuildToken(Code, match!.Value);
    }

    public Token CurrentToken
    {
        get
        {
            Token token = Peek();
            while (IsSpacesToken(token))
            {
                Advance();
                token = Peek();
            }

            return token;
        }
    }


    public Token Advance()
    {
        Token token = Peek();
        Code = Code.Slice(token.Text.Length);
        return token;
    }

    public bool IsSpacesToken(Token token) => SpaceTokenKinds.Contains(token.Kind);

    private static Token BuildToken(ArraySegment<char> code, (TokenKind kind, int matchLen) match)
    {
        Span<char> spanToken = code.AsSpan().Slice(0, match.matchLen);
        return new Token
        {
            Kind = match.kind,
            Text = spanToken.ToString()
        };
    }

    private static void ReportError((TokenKind kind, int matchLen)? match, ArraySegment<char> text)
    {
        if (match != null)
        {
            return;
        }

        Span<char> startError = text.AsSpan();
        if (startError.Length > 100)
        {
            startError = startError.Slice(0, 100);
        }

        string errorText = startError.ToString();
        throw new InvalidDataException($"Invalid start text: '{errorText}'");
    }

    private static readonly TokenKind[] SpaceTokenKinds =
    [
        TokenKind.Comment, TokenKind.Space, TokenKind.Eoln
    ];

    public Token[] ReadUntil(string tokenText)
    {
        List<Token> resultList = new List<Token>();
        do
        {
            Token token = Advance();
            if (IsSpacesToken(token))
            {
                continue;
            }

            resultList.Add(token);
            if (token.Text == tokenText)
            {
                break;
            }
        } while (true);

        return resultList.ToArray();
    }
}