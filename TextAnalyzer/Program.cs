using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;

namespace TextAnalyzerProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string text;
            
            if (args.Length == 1)
            {
                Console.WriteLine("Podano argument!");

                string path = args[0];
                text = readFile(path);

                if (text == null || text == "") { return; }

                TextStatistics textStatistics = new TextStatistics(text);
                textStatistics.printStatistics();

            }
            else if (args.Length == 0)
            {
                Console.WriteLine("Wpisz:\n1. Aby wpisać ścieżkę do pliku\n2. Podaj tekst");

                string input = Console.ReadLine();
                int choice;

                if (int.TryParse(input, out choice))
                {
                    if (choice == 1)
                    {
                        Console.WriteLine("Wpisz ścieżkę do pliku:");
                        string path = Console.ReadLine();

                        text = readFile(path);

                        if (text == null) { return; }

                        TextStatistics textStatistics = new TextStatistics(text);
                        textStatistics.printStatistics();
                    }
                    else if (choice == 2)
                    {
                        Console.WriteLine("Wpisz tekst:");
                        text = Console.ReadLine();

                        if (text == null || text == "") {
                            Console.Error.WriteLine("Podano pusty tekst!");
                            return; 
                        }

                        TextStatistics textStatistics = new TextStatistics(text);
                        textStatistics.printStatistics();
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy wybór.");
                    }
                }
                else
                {
                    Console.WriteLine("Podano niepoprawną liczbę.");
                }

            }
            else
            {
                Console.Error.WriteLine("Podano zbyt dużo argumentów do programu");
            }
        }

        static string readFile(string path) {
            string text = null;
            try
            {
                text = File.ReadAllText(path);
                if (text == null || text == "")
                {
                    throw new ArgumentException($"Error: Plik \"{path}\" jest pusty!");
                }
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine(ex.Message);
                
            }
            catch (FileNotFoundException)
            {
                Console.Error.WriteLine($"Error: Plik \"{path}\" nie został znaleziony.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return text;
        }
    }
}
