namespace Strukt.Lex
{
    public static class LexCharacters
    {
        public static bool IsLetter(char ch)
        {
            if (ch == '_')
                return true;
            return char.IsLetter(ch);
        }

        public static bool IsDecimalDigit(char ch)
        {
            return ch >= '0' && ch <= '9';
        }
        
        public static bool IsBinaryDigit(char ch)
        {
            return ch >= '0' && ch <= '1';
        }
        
        public static bool IsOctalDigit(char ch)
        {
            return ch >= '0' && ch <= '7';
        }

        public static bool IsHexDigit(char ch)
        {
            return 
                (ch >= '0' && ch <= '9')
                ||(ch >= 'a' && ch <= 'f')
                ||(ch >= 'A' && ch <= 'F');
        }
    }
}