using System;

namespace MyServices
{
    public class ConsoleLogger : ILoggerService
    {
        public void Log(string message)
        {
            Console.WriteLine($"[LOG]: {message}");
        }
    }
}
