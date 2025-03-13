using KalkulatorApp;

namespace CalculatorTests
{
    public class CalculatorTests
    {

        [Test]
        public void Add_ReturnsCorrectResult()
        {
            double result = ScientificCalculator.calculator.Add(5, 7);
            Assert.AreEqual(12, result);
        }

        [Test]
        public void Multiply_ReturnsCorrectResult()
        {
            double result = ScientificCalculator.calculator.Multiply(5, 7);
            Assert.AreEqual(35, result);
        }

        [Test]
        public void Subtract_ReturnsCorrectResult()
        {
            double result = ScientificCalculator.calculator.Subtract(5, 7);
            Assert.AreEqual(-2, result);
        }

        [Test]
        public void Divide_ReturnsCorrectResult()
        {
            double result = ScientificCalculator.calculator.Divide(10,2);
            Assert.AreEqual(5, result);
        }

        [Test]
        public void DivideByZero_ThrowsException()
        {
            Assert.Throws<DivideByZeroException>(() => 
                ScientificCalculator.calculator.Divide(10, 0));

        }

        [Test]
        public void SumSequence_ReturnsCorrectResult()
        {
            IEnumerable<double> seq = new List<double> { 1, 2, 3, 4, 5 };
            double result = ScientificCalculator.calculator.SumSequence(seq);
            Assert.AreEqual(15, result);
        }

        [Test]
        public void Null_IEnumerable_ThrowsException()
        {
            IEnumerable<double> seq = new List<double> { };
            Assert.Throws<ArgumentException>(() =>
                ScientificCalculator.calculator.SumSequence(seq));
        }

        [Test]
        public void AvgOfSequence_ReturnsCorrectResult()
        {
            IEnumerable<double> seq = new List<double> { 1, 2, 3, 4, 5 };
            double result = ScientificCalculator.calculator.AvgOfSequence(seq);
            Assert.AreEqual(3, result);
        }

        [Test]
        public void MaxValue_ReturnsCorrectResult()
        {
            IEnumerable<double> seq = new List<double> { 6, 2, 3, -3, 2 };
            double result = ScientificCalculator.calculator.MaxValue(seq);
            Assert.AreEqual(6, result);
        }

        [Test]
        public void MinValue_ReturnsCorrectResult()
        {
            IEnumerable<double> seq = new List<double> { 6, 2, 3, -3, 2 };
            double result = ScientificCalculator.calculator.MinValue(seq);
            Assert.AreEqual(-3, result);
        }
    }
}