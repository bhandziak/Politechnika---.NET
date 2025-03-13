using KalkulatorApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorTests
{
    public class ScientificCalculatorTests
    {
        [Test]
        public void Power_ReturnsCorrectResult()
        {
            double result = ScientificCalculator.Power(10, 2);
            Assert.AreEqual(100, result);
        }

        [Test]
        public void SquareRoot_ReturnsCorrectResult()
        {
            double result = ScientificCalculator.SquareRoot(16);
            Assert.AreEqual(4, result);
        }

        [Test]
        public void SquareRoot_NegativeNumber_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                ScientificCalculator.SquareRoot(-5));
        }

        [Test]
        public void Log_ReturnsCorrectResult()
        {
            double result = ScientificCalculator.Log(Math.E);
            Assert.AreEqual(1, result);
        }
    }
}
