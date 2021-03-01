using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslTranslator.UnitTests
{
    [TestClass]
    public class ExponentTests
    {
        [TestMethod]
        public void EvaluateExponent1()
        {
            var expression = "2^3";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(8, result);
        }

        [TestMethod]
        public void EvaluateExponent2()
        {
            var expression = "2^(3^2)";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(512, result);
        }

        [TestMethod]
        public void EvaluateExponent3()
        {
            var expression = "(2^3)^2";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(64, result);
        }

        [TestMethod]
        public void EvaluateExponent4()
        {
            var expression = "2^3^2";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(512, result);
        }

        [TestMethod]
        public void EvaluateExponent5()
        {
            var expression = "2^3!^2";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(68719476736, result);
        }

        [TestMethod]
        public void EvaluateExponent6()
        {
            var expression = "(2^3!)^2";

            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression);

            Assert.AreEqual(4096, result);
        }
    }
}
