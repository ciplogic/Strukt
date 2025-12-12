namespace Strukt.Lex;

public struct Token
{
    public TokenKind Kind;
    public override string ToString()
    {
        return $"{Text} -> {Kind}";
    }

    public string Text;
}