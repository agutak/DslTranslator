using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DslTranslator.UnitTests
{
    [TestClass]
    public class ExpressionEngineTests
    {
        [TestMethod]
        public void EvaluateTest()
        {
            var expression = "1 + 2 * 3";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(7, result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EvaluateInvalidTest()
        {
            var expression = "(1 + 2))";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void EvaluateDecimal()
        {
            var expression = "1 + 1e5";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(100001, result);
        }

        [TestMethod]
        public void EvaluateSubExpressions()
        {
            var expression = "3 * (2 + 1)";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(9, result);
        }

        [TestMethod]
        public void EvaluateNegation()
        {
            var expression = "-3 * (2 + 1)";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(-9, result);
        }

        [TestMethod]
        public void EvaluateFactorial()
        {
            var expression = "5!";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(120, result);
        }
    }
}
