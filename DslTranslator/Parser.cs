using DslTranslator.AstNodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DslTranslator
{
    /// <summary>
    ///   Implements the following Production Rules
    ///       EXPRESSION: TERM [('+'|'-') TERM]*
    ///             TERM: FACTOR [('*'|'/') FACTOR]*
    ///           FACTOR: '-'? EXPONENT
    ///     IMPLICIT_MUL: EXPONENT IMPLICIT_MUL
    ///         EXPONENT: FACTORIAL_FACTOR ['^' EXPONENT]*
    /// FACTORIAL_FACTOR: PRIMARY '!'?
    ///          PRIMARY: IDENTIFIER | NUMBER | SUB_EXPRESSION
    ///       IDENTIFIER: VARIABLE | FUNCTION
    ///         FUNCTION: FUNCTION_NAME '(' FUNC_ARGS ')'
    ///        FUNC_ARGS: EXPRESSION [, EXPRESSION]*
    ///   SUB_EXPRESSION: '(' EXPRESSION ')'
    /// </summary>
    public class Parser
    {
        private static TokenType[] TERM_OPERATORS = new TokenType[] { TokenType.Addition, TokenType.Minus };
        private static TokenType[] FACTOR_OPERATORS = new TokenType[] { TokenType.Multiplication, TokenType.Division };

        private readonly Lexer _lexer;
        private readonly SymbolTable _symbolTable;

        public Parser(Lexer lexer) : this(lexer, new SymbolTable())
        {
        }

        public Parser(Lexer lexer, SymbolTable symbolTable)
        {
            _lexer = lexer;
            _symbolTable = symbolTable;
        }

        public AstNode Parse()
        {
            if (TryParseExpression(out var node))
            {
                Expect(TokenType.EOE);
                return node;
            }
            else
                throw new Exception($"Failed to parse expression tree");
        }

        private bool TryParseExpression(out AstNode node)
        {
            if(TryParseTerm(out node))
            {
                while (IsNext(TERM_OPERATORS))
                {
                    var op = Accept();
                    if(TryParseTerm(out var rhs))
                        node = CreateBinaryOperator(op, node, rhs);
                    else
                        throw new Exception($"Failed to parse expression rule at position {_lexer.Position}");
                }
            }

            return node != null;
        }

        private bool TryParseTerm(out AstNode node)
        {
            if(TryParseFactor(out node))
            {
                while (IsNext(FACTOR_OPERATORS))
                {
                    var op = Accept();
                    if (TryParseFactor(out var rhs))
                        node = CreateBinaryOperator(op, node, rhs);
                    else
                        throw new Exception($"Failed to parse factor rule at position {_lexer.Position}");
                }
            }

            return node != null;
        }

        private bool TryParseFactor(out AstNode node)
        {
            if (IsNext(TokenType.Minus))
            {
                var op = Accept();

                if (TryParseImplicitMultiplication(out node))
                    node = new NegationUnaryOperatorAstNode(op, node);
                else
                    throw new Exception($"Exception parsing the factor rule at position {_lexer.Position}");
            }
            else
                TryParseImplicitMultiplication(out node);

            return node != null;
        }

        private bool TryParseImplicitMultiplication(out AstNode node)
        {
            if(TryParseExponent(out node))
                if(TryParseImplicitMultiplication(out var rhs))
                    node = CreateBinaryOperator(new Token(TokenType.Multiplication, -1, null), node, rhs);

            return node != null;
        }

        private bool TryParseExponent(out AstNode node)
        {
            if(TryParseFactorialFactor(out node))
            {
                if (IsNext(TokenType.Exponent))
                {
                    var op = Accept();

                    if(TryParseExponent(out var rhs))
                        node = new ExponentBinaryOperatorAstNode(op, node, rhs);
                }
            }

            return node != null;
        }

        private bool TryParseFactorialFactor(out AstNode node)
        {
            if (TryParsePrimary(out node))            
                if (IsNext(TokenType.Factorial))
                    node = new FactorialUnaryOperatorAstNode(Accept(), node);
            
            return node != null;
        }

        private bool TryParsePrimary(out AstNode node)
        {
            if (TryParseIdentifier(out node))
                return true;

            if (TryParseNumber(out node))
                return true;

            if (TryParseSubExpression(out node))
                return true;

            return false;
        }

        private bool TryParseIdentifier(out AstNode node)
        {
            if (TryParseVariable(out node))
                return true;

            if (TryParseFunction(out node))
                return true;

            return false;
        }

        private bool TryParseVariable(out AstNode node)
        {
            node = null;

            if (IsNext(TokenType.Identifier))
            {
                var token = _lexer.Peek();
                var stEntry = _symbolTable.Get(token.Value);

                if (stEntry is null)
                    throw new Exception($"Undefined identifier {token.Value} at position {token.Position}");

                if (stEntry.Type == EntryType.Variable)
                    node = new VariableIdentifierAstNode(Accept());
            }

            return node != null;
        }

        private bool TryParseFunction(out AstNode node)
        {
            node = null;

            if (IsNext(TokenType.Identifier))
            {
                var token = _lexer.Peek();
                var stEntry = _symbolTable.Get(token.Value);

                if (stEntry is null)
                    throw new Exception($"Undefined identifier {token.Value} at position {token.Position}");

                if (stEntry.Type == EntryType.Function)
                {
                    node = new FunctionIdentifierAstNode(Accept());
                    Expect(TokenType.OpenParen);
                    Accept();

                    if (TryParseFuncArgs(out var args))
                        (node as FunctionIdentifierAstNode).ArgumentNodes.AddRange(args);

                    Expect(TokenType.CloseParen);
                    Accept();
                }
            }

            return node != null;
        }

        private bool TryParseFuncArgs(out List<AstNode> argNodes)
        {
            argNodes = new();

            if(TryParseExpression(out var expNode))
            {
                argNodes.Add(expNode);

                while(IsNext(TokenType.ArgSeparator))
                {
                    Accept();

                    if (TryParseExpression(out expNode))
                        argNodes.Add(expNode);
                    else
                        throw new Exception($"Error parsing function arguments at position {_lexer.Position}");
                }
            }

            return argNodes.Any();
        }

        private bool TryParseNumber(out AstNode node)
        {
            node = null;

            if (IsNext(TokenType.Number))
                node = new NumberAstNode(Accept());

            return node != null;
        }

        private bool TryParseSubExpression(out AstNode node)
        {
            node = null;

            if(IsNext(TokenType.OpenParen))
            {
                Accept();
                if(TryParseExpression(out node))
                {
                    Expect(TokenType.CloseParen);
                    Accept();
                }                
            }

            return node != null;
        }

        private Token Accept() => _lexer.ReadNext();

        private void Expect(TokenType tokenType)
        {
            if (!IsNext(tokenType))
                throw new Exception($"Unexpected token {_lexer.Peek()?.Value} at position {_lexer.Position}");
        }

        private bool IsNext(params TokenType[] possibleTokens) =>
            IsNext(x => possibleTokens.Any(k => k == x));

        private bool IsNext(Predicate<TokenType> match) =>
            match(_lexer.Peek().Type);

        private AstNode CreateBinaryOperator(Token operation, AstNode left, AstNode right)
        {
            switch (operation.Type)
            {
                case TokenType.Addition: return new AdditionBinaryOperatorAstNode(operation, left, right);
                case TokenType.Minus: return new SubtractionBinaryOperatorAstNode(operation, left, right);
                case TokenType.Multiplication: return new MultiplicationBinaryOperatorAstNode(operation, left, right);
                case TokenType.Division: return new DivisionBinaryOperatorAstNode(operation, left, right);
                default: throw new ArgumentOutOfRangeException(nameof(operation));
            }
        }
    }
}
