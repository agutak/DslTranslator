using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslTranslator.UnitTests
{
    [TestClass]
    public class ImplicitMultiplicationTests
    {
        [TestMethod]
        public void Test0()
        {
            var expression = "2 x";
            var variables = new { x = 5 };

            var ee = new ExpressionEngine();

            var result = ee.Evaluate(expression, variables);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void Test1()
        {
            var expression = "2x";
            var variables = new { x = 5 };

            var ee = new ExpressionEngine();

            var result = ee.Evaluate(expression, variables);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void Test2()
        {
            var expression = "2x + y";
            var variables = new { x = 5, y = 10 };

            var ee = new ExpressionEngine();

            var result = ee.Evaluate(expression, variables);

            Assert.AreEqual(20, result);
        }
    }
}
