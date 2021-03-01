namespace DslTranslator
{
    public class Token
    {
        public Token(TokenType type, int position, string value)
        {
            Type = type;
            Position = position;
            Value = value;
        }

        public TokenType Type { get; }
        public int Position { get; }
        public string Value { get; }

    }

    public enum TokenType
    {
        EOE,
        Number,
        Addition,
        Minus,
        Multiplication,
        Division,
        OpenParen,
        CloseParen,
        Factorial,
        Exponent,
        Identifier,
        ArgSeparator
    }
}
