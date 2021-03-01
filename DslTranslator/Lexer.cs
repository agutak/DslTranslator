using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DslTranslator
{
    //OPERATOR: + | - | * | /
    //NUMBER: [0-9]+
    public class Lexer
    {
        const char PLUS = '+';
        const char MINUS = '-';
        const char MULTIPLICATION = '*';
        const char DIVISION = '/';
        const char OPEN_PAREN = '(';
        const char CLOSE_PAREN = ')';
        const char DECIMAL_SEPARATOR = '.';
        const char FACTORIAL = '!';
        const char EXPONENT = '^';
        const char ARG_SEPARATOR = ',';

        private static char[] E_NOTATION = new char[] { 'e', 'E' };
        private static char[] SIGN_OPERATORS = new char[] { PLUS, MINUS };

        private readonly static Dictionary<char, Func<int, char, Token>> SimpleTokenMap =
            new Dictionary<char, Func<int, char, Token>>
            {
                { PLUS, (p,v) => new Token(TokenType.Addition, p, v.ToString()) },
                { MINUS, (p,v) => new Token(TokenType.Minus, p, v.ToString()) },
                { MULTIPLICATION, (p,v) => new Token(TokenType.Multiplication, p, v.ToString()) },
                { DIVISION, (p,v) => new Token(TokenType.Division, p, v.ToString()) },
                { OPEN_PAREN, (p,v) => new Token(TokenType.OpenParen, p, v.ToString()) },
                { CLOSE_PAREN, (p,v) => new Token(TokenType.CloseParen, p, v.ToString()) },
                { FACTORIAL, (p,v) => new Token(TokenType.Factorial, p, v.ToString()) },
                { EXPONENT, (p,v) => new Token(TokenType.Exponent, p, v.ToString()) },
                { ARG_SEPARATOR, (p,v) => new Token(TokenType.ArgSeparator, p, v.ToString()) }
            };

        private readonly SourceScanner _scanner;

        public Lexer(SourceScanner sourceScanner)
        {
            _scanner = sourceScanner;
        }

        public int Position => _scanner.Position;

        //OPERATOR: + | - | * | /
        private bool TryTokenizeSimpleToken(out Token token)
        {
            token = null;

            if(IsNext(SimpleTokenMap.ContainsKey))
            {
                var position = Position;
                var op = Accept();
                token = SimpleTokenMap[op](position, op);
            }

            return token != null;
        }

        //NUMBER: [0-9]+
        private bool TryTokenizeNumber(out Token token)
        {
            token = null;
            
            var sb = new StringBuilder();
            var position = Position;

            sb.Append(ReadDigits());

            if (IsNext(DECIMAL_SEPARATOR))
                sb.Append(Accept());

            sb.Append(ReadDigits());

            if (sb.Length > 0 && char.IsDigit(sb[sb.Length - 1]) && IsNext(E_NOTATION))
            {
                sb.Append(Accept());

                if(IsNext(SIGN_OPERATORS))
                    sb.Append(Accept());

                Expect(char.IsDigit);

                sb.Append(ReadDigits());
            }
            
            if (sb.Length > 0)
                token = new Token(TokenType.Number, position, sb.ToString());

            if (token is not null && !double.TryParse(token.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
                throw new Exception($"Invalid numeric value {token.Value} found at position {token.Position}");

            return token != null;
        }

        private string ReadDigits()
        {
            var sb = new StringBuilder();

            while (IsNext(char.IsDigit))
                sb.Append(Accept());

            return sb.ToString();
        }

        private bool TryTokenizeIdentifier(out Token token)
        {
            token = null;
            var sb = new StringBuilder();
            var position = Position;

            if(IsNext('_'))
            {
                sb.Append(Accept());
                Expect(char.IsLetter);
            }

            if(IsNext(char.IsLetter))
            {
                sb.Append(Accept());

                while (IsNext(x => char.IsLetterOrDigit(x) || x == '_'))
                    sb.Append(Accept());

            }

            if (sb.Length > 0)
                token = new Token(TokenType.Identifier, position, sb.ToString());

            return token != null;
        }

        private char Accept() => _scanner.Read().Value;

        private void Expect(Predicate<char> match)
        {
            if (!IsNext(match))
                throw new Exception($"Unexpected value at position {Position}");
        }

        private bool IsNext(params char[] possibleValues) =>
            IsNext(x => possibleValues.Any(k => k == x));

        private bool IsNext(Predicate<char> match)
        {
            var lookahead = _scanner.Peek();
            return lookahead.HasValue && match(lookahead.Value);
        }

        private void ConsumeWhiteSpace()
        {
            while (IsNext(char.IsWhiteSpace))
                Accept();
        }

        public Token ReadNext()
        {
            if (_scanner.EndOfSource)
                return new Token(TokenType.EOE, _scanner.Position, null);

            ConsumeWhiteSpace();

            if (TryTokenizeSimpleToken(out var token))
                return token;

            if (TryTokenizeNumber(out token))
                return token;

            if (TryTokenizeIdentifier(out token))
                return token;

            throw new Exception($"Unexpected character {_scanner.Peek()} found at position {_scanner.Position}");
        }

        public Token Peek()
        {
            _scanner.Push();

            var token = ReadNext();
            _scanner.Pop();
            return token;
        }
    }
}
