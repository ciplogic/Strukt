namespace Strukt.Lex;

public enum TokenKind
{
    Space,
    Quote,
    Reserved,
    Identifier,
    Number,
    Eoln,
    Operator,
    FieldIdentifier,
    Comment,
    None,
    Macro,
    Eof
}