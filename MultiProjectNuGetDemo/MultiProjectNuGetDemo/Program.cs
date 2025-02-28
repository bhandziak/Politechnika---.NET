using MyLibrary;
using MyServices;
using Newtonsoft.Json;

using Microsoft.Extensions.DependencyInjection;



namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator Calculator = new();

            int sum = Calculator.Add(5, 3);
            var result = new { Operation = "Add", A = 5, B = 3, Result = sum };
            string jsonResult = JsonConvert.SerializeObject(result, Formatting.Indented);
            Console.WriteLine(jsonResult);


            // Konfiguracja kontenera DI
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ILoggerService, ConsoleLogger>()
                .BuildServiceProvider();

            // Uzyskanie instancji loggera
            var logger = serviceProvider.GetService<ILoggerService>();
            logger.Log("Aplikacja uruchomiona.");

            // Przykładowe użycie kalkulatora
            sum = Calculator.Add(10, 15);
            logger.Log($"Wynik dodawania: {sum}");
        }
    }
}
