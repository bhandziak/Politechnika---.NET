using System.Collections.Concurrent;
using System.Diagnostics;

namespace GutenbergProject
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            string[] urls = { 
                "https://www.gutenberg.org/files/84/84-0.txt",
                "https://www.gutenberg.org/files/11/11-0.txt",
                "https://www.gutenberg.org/files/1661/1661-0.txt",
                "https://www.gutenberg.org/files/2701/2701-0.txt"
            };

            ConcurrentDictionary<string, int> globalStats = new();

            var fetchTime = Stopwatch.StartNew();

            // fetch async
            Task<string>[] fetchTasks = urls.Select(
                url => WebClient.GetPageContentAsync(url)).ToArray();

            string[] contents = await Task.WhenAll(fetchTasks);

            fetchTime.Stop();
            

            var analizeTime = Stopwatch.StartNew();

            // analyze async
            Parallel.ForEach(contents, content =>
            {
                var localStats = Counter.CreateStats(content);

                foreach (var statWord in localStats)
                {
                    globalStats.AddOrUpdate(statWord.Key, statWord.Value, (_, oldVal) => oldVal + statWord.Value);
                }
            });

            analizeTime.Stop();
            

            var sorted = globalStats
                .OrderByDescending(statWord => statWord.Value)
                .ToDictionary(statWord => statWord.Key, statWord => statWord.Value);

            Counter.PrintStats(sorted, 10);
            Console.WriteLine();
            Console.WriteLine($"Czas pobierania: {fetchTime.Elapsed.TotalSeconds:F2} sekundy");
            Console.WriteLine($"Czas przetwarzania: {analizeTime.Elapsed.TotalSeconds:F2} sekundy");
        }


    }
}
