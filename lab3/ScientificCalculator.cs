using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalkulatorApp
{
    public class ScientificCalculator
    {
        static public Calculator calculator = new();
        static public double Power(double a, double b)
        {
            double res;

            if (a == 0 && b == 0) {
                throw new ArgumentException("Symbol nieoznaczony!");
            }
            res = Math.Pow(a, b);
            return Math.Pow(a, b);

        }

        static public double SquareRoot(double a)
        {
            double res;

            if (a < 0) {
                throw new ArgumentException("Nie można pierwiastkować ujemnych liczb!");
            }
            res = Math.Sqrt(a);
            return res;
 
        }

        static public double Log(double a)
        {
            double res;

            if (a <= 0)
            {
                throw new ArgumentException("Nie istnieje taki logarytm!");
            }
            res = Math.Log(a);
            return res;
            

        }
    }
}
