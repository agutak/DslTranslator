using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslTranslator.UnitTests
{
    [TestClass]
    public class FactorialTests
    {
        [TestMethod]
        public void EvaluateFactorial1()
        {
            var expression = "5!";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(120, result);
        }

        [TestMethod]
        public void EvaluateFactorial2()
        {
            var expression = "-5!";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(-120, result);
        }

        [TestMethod]
        public void EvaluateFactorial3()
        {
            var expression = "(2 + 3)!";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(120, result);
        }

        [TestMethod]
        public void EvaluateFactorial4()
        {
            var expression = "-(2 + 3)!";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(-120, result);
        }
    }
}
