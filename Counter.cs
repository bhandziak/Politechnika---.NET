using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GutenbergProject
{
    public class Counter
    {
        public static Dictionary<string, int> CreateStats(string text)
        {
            var matches = Regex.Matches(text.ToLowerInvariant(), @"\b[a-z]+\b");

            Dictionary<string, int> stats = new Dictionary<string, int>();

            foreach (Match match in matches)
            {
                string word = match.Value;
                if (!stats.ContainsKey(word))
                {
                    stats[word] = 1;
                }
                else
                {
                    stats[word]++;
                }
            }
            var sortedStats = stats.OrderByDescending(el => el.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            return sortedStats;
        }

        public static void PrintStats(Dictionary<string, int> stats, int limit)
        {
            Console.WriteLine("Najczęstsze słowa:");
            int i = 1;
            foreach (var pair in stats.Take(limit))
            {
                Console.WriteLine($"{i++}. {pair.Key}: {pair.Value}");
            }
        }

    }
}
