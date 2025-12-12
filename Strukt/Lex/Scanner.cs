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
        MatchWithLength match = null;
        foreach ((TokenKind kind, Func<ArraySegment<char>, int>) matcher in matchers)
        {
            match = ScannerUtils.Match(Code, matchers);
            if (match != null) break;
        }

        if (match is null)
        {
            ReportError(null, Code);
        }

        return BuildToken(Code, match);
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

    private static Token BuildToken(ArraySegment<char> code, MatchWithLength match)
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
            if (token.Text == tokenText)
            {
                break;
            }

            resultList.Add(token);
        } while (true);

        return resultList.ToArray();
    }

    public override string ToString()
    {
        int indexEoln = Code.IndexOf('\r');
        if (indexEoln < 0)
        {
            return new string(Code.ToArray());
        }

        var currentLine = Code.Slice(0, indexEoln).ToArray();
        return new string(currentLine);
    }
}