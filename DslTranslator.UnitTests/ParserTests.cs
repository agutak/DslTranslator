using DslTranslator.AstNodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DslTranslator.UnitTests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ParseTest1()
        {
            var expression = "1 + 2";

            var ast = new Parser(new Lexer(new SourceScanner(expression))).Parse() as BinaryOperatorAstNode;

            Assert.IsNotNull(ast);

            Assert.AreEqual(TokenType.Addition, ast.Token.Type);
            Assert.AreEqual(TokenType.Number, ast.Left.Token.Type);
            Assert.AreEqual(TokenType.Number, ast.Right.Token.Type);

            Assert.AreEqual(1, (ast.Left as NumberAstNode).Value);
            Assert.AreEqual(2, (ast.Right as NumberAstNode).Value);
        }

        [TestMethod]
        public void ParseTest2()
        {
            var expression = "1 + 2 * 3";

            var ast = new Parser(new Lexer(new SourceScanner(expression))).Parse() as BinaryOperatorAstNode;

            Assert.IsNotNull(ast);

            Assert.AreEqual(TokenType.Addition, ast.Token.Type);
            Assert.AreEqual(TokenType.Number, ast.Left.Token.Type);

            Assert.AreEqual(TokenType.Multiplication, ast.Right.Token.Type);
            Assert.AreEqual(TokenType.Number, (ast.Right as BinaryOperatorAstNode).Left.Token.Type);
            Assert.AreEqual(TokenType.Number, (ast.Right as BinaryOperatorAstNode).Right.Token.Type);

            Assert.AreEqual(1, (ast.Left as NumberAstNode).Value);
            Assert.AreEqual(2, ((ast.Right as BinaryOperatorAstNode).Left as NumberAstNode).Value);
            Assert.AreEqual(3, ((ast.Right as BinaryOperatorAstNode).Right as NumberAstNode).Value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ParseTest3()
        {
            var expression = "1 +";

            _ = new Parser(new Lexer(new SourceScanner(expression))).Parse();
        }

        [TestMethod]
        public void ParseTest4()
        {
            var expression = "2 * x * y";
            var variables = new { x = 5, y = 10 };

            var ee = new ExpressionEngine();

            var result = ee.Evaluate(expression, variables);

            Assert.AreEqual(100, result);
        }
    }
}
