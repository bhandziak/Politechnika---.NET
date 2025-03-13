using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalkulatorApp
{
    public class CalculatorService
    {

        public void Run()
        {
            Console.WriteLine("Kalkulator naukowy w C#");
            string choice;
            double a;
            double b;
            (double a, double b)? twoNumbers;
            double? oneNumber;
            IEnumerable<double> sequence;
            Boolean isRunning = true;
            while (isRunning)
            {
                Console.Write("\nWybierz operację: +, -, *, /, ^, sqrt, log, sum, avg, min, max:\n> ");
                choice = Console.ReadLine();
                try
                {
                    switch (choice)
                    {
                        case "+":
                            twoNumbers = GetTwoNumbers();
                            (a, b) = twoNumbers.Value;
                            Console.WriteLine("Suma: " + ScientificCalculator.calculator.Add(a, b));

                            break;

                        case "-":
                            twoNumbers = GetTwoNumbers();
                            (a, b) = twoNumbers.Value;
                            Console.WriteLine("Różnica: " + ScientificCalculator.calculator.Subtract(a, b));

                            break;

                        case "*":
                            twoNumbers = GetTwoNumbers();
                            (a, b) = twoNumbers.Value;
                            Console.WriteLine("Iloczyn: " + ScientificCalculator.calculator.Multiply(a, b));

                            break;

                        case "/":
                            twoNumbers = GetTwoNumbers();
                            (a, b) = twoNumbers.Value;
                            Console.WriteLine("Iloraz: " + ScientificCalculator.calculator.Divide(a, b));

                            break;
                        case "^":
                            twoNumbers = GetTwoNumbers("( podstawa potęgi )", "( wykładnik potęgi )");
                            (a, b) = twoNumbers.Value;
                            Console.WriteLine("Wynik potęgowania: " + ScientificCalculator.Power(a, b));

                            break;

                        case "sqrt":
                            oneNumber = GetOneNumber();
                            Console.WriteLine("Wynik pierwiastkowania: " + ScientificCalculator.SquareRoot(oneNumber.Value));

                            break;
                        case "log":
                            oneNumber = GetOneNumber();
                            Console.WriteLine("Wynik: " + ScientificCalculator.Log(oneNumber.Value));
                            break;

                        case "sum":
                            sequence = GetIEnumerable();
                            Console.WriteLine("Suma: " + ScientificCalculator.calculator.SumSequence(sequence));
                            break;

                        case "avg":
                            sequence = GetIEnumerable();
                            Console.WriteLine("Średnia: " + ScientificCalculator.calculator.AvgOfSequence(sequence));

                            break;

                        case "min":
                            sequence = GetIEnumerable();
                            Console.WriteLine("Najmniejsza wartość: " + ScientificCalculator.calculator.MinValue(sequence));

                            break;

                        case "max":
                            sequence = GetIEnumerable();
                            Console.WriteLine("Największa wartość: " + ScientificCalculator.calculator.MaxValue(sequence));


                            break;

                        default:
                            Console.Error.WriteLine("Podano nieznane polecenie! Zamykam program ...");
                            isRunning = false;
                            break;
                    }
                }
                catch (Exception ex) {
                    Console.Error.WriteLine(ex.Message);
                }
                
            }
            
        }

        (double a, double b) GetTwoNumbers(string addText1 = "", string addText2 = "")
        {
            double a, b;
            string input;
            Console.Write("Podaj pierwszą liczbę " +addText1 + ":\n> ");
            input = Console.ReadLine();

            if (!double.TryParse(input, out a))
            {
                throw new ArgumentException("Błąd: Podano błędne dane!");
            }

            Console.Write("Podaj drugą liczbę " + addText2 + ":\n> ");
            input = Console.ReadLine();

            if (!double.TryParse(input, out b))
            {
                throw new ArgumentException("Błąd: Podano błędne dane!");
            }

            return (a, b);
        }

        double GetOneNumber() {
            double a;
            Console.Write("Podaj liczbę:\n> ");
            string input = Console.ReadLine();

            if (!double.TryParse(input, out a))
            {
                throw new ArgumentException("Błąd: Podano błędne dane!");
            }
            return a;
        }

        IEnumerable<double> GetIEnumerable()
        {
            Console.Write("Podaj liczby, oddzielone spacją:\n> ");
            string input = Console.ReadLine();
            double number;
            List<string> numbersStr = new List<string>(input.Split(" "));
            List<double> numbers = new List<double>();

            foreach (string numberStr in numbersStr) {
                if(double.TryParse(numberStr, out number))
                {
                    numbers.Add(number);
                }
            }

            return numbers;
        }
    }
}
