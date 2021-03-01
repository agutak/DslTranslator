using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslTranslator.UnitTests
{
    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        public void ReadNextTest()
        {
            var expression = "1 + 2";

            (TokenType, int, string)[] expectedResults = new (TokenType, int, string)[]
            {
                (TokenType.Number, 0, "1"),
                (TokenType.Addition, 2, "+"),
                (TokenType.Number, 4, "2")
            };

            var lexer = new Lexer(new SourceScanner(expression));

            foreach (var (t,p,v) in expectedResults)
            {
                var token = lexer.ReadNext();
                Assert.AreEqual(t, token.Type);
                Assert.AreEqual(p, token.Position);
                Assert.AreEqual(v, token.Value);
            }

            Assert.AreEqual(TokenType.EOE, lexer.ReadNext().Type);
        }

        [TestMethod]
        [DataRow("100")]
        [DataRow("1.5")]
        [DataRow(".5")]
        [DataRow("1e5")]
        public void ReadNextTest_Decimal(string expected)
        {
            var lexer = new Lexer(new SourceScanner(expected));
            Assert.AreEqual(expected, lexer.ReadNext().Value);
        }
    }
}
