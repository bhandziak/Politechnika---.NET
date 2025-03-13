using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalkulatorApp
{
    public class Calculator
    {
        public double Add(double a, double b)
        {
            return a + b;
        }

        public double Subtract(double a, double b)
        {
            return a - b;
        }

        public double Multiply(double a, double b)
        {
            return a * b;
        }

        public double Divide(double a, double b)
        {
            double res;

            if (b == 0) {
                Console.Error.WriteLine("Błąd: Nie można dzielić przez zero.");
                throw new DivideByZeroException();
            }
            res = a / b;
            return res;

        }

        public double SumSequence(IEnumerable<double> numbers)
        {
            if (numbers.Count() == 0) {
                throw new ArgumentException("Błąd: Brak liczb!");
            }

            return numbers.Sum();
        }

        public double AvgOfSequence(IEnumerable<double> numbers)
        {
            int c = numbers.Count();

            if(c == 0)
            {
                throw new ArgumentException("Błąd: Brak liczb!");
            }
            return numbers.Sum() / c;

        }

        public double MaxValue(IEnumerable<double> numbers)
        {
            int c = numbers.Count();

            if (c == 0)
            {
                throw new ArgumentException("Błąd: Brak liczb!");
            }
            return numbers.Max();

        }

        public double MinValue(IEnumerable<double> numbers)
        {
            int c = numbers.Count();

            if (c == 0)
            {
                throw new ArgumentException("Błąd: Brak liczb!");
            }
            return numbers.Min();

        }
    }
}
