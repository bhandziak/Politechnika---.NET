using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TextAnalyzerProject
{
    public class TextAnalyzer
    {
        private static char[] _delimiters = new char[] { ' ', ',', '.', '!', '?', ';', ':' };



        public static int CountCharacters(string text)
        {
            return text.Count();
        }
        public static int CountCharatersWithOutWhiteChars(string text)
        {
            return text.Count(c => !char.IsWhiteSpace(c));
        }

        public static int CountLetters(string text)
        {
            return text.Count(char.IsLetter);
        }

        public static int CountNumbers(string text)
        {
            return text.Count(char.IsDigit);
        }

        public static int CountPunctuationMarks(string text)
        {
            return text.Count(char.IsPunctuation);
        }

        public static int CountWords(string text)
        {
            string[] words = text.Split(_delimiters, StringSplitOptions.RemoveEmptyEntries);
            return words.Count();
        }

        public static int CountUniqueWords(string text)
        {
            Dictionary<string, int> stats = CreateStats(text);

            return stats.Count;
        }

        public static float AvgLenWord(string text)
        {
            float res = 0;
            string[] words = text.Split(_delimiters, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                res += word.Length;
            }
            if(words.Length == 0) { return 0; }

            return res / words.Length;
        } 

        public static string[] LongestAndShortestWord(string text)
        {
            Dictionary<string, int> stats = CreateStats(text);
            string longest = "";
            string shortest = "";

            foreach (string word in stats.Keys)
            {
                if (longest == "" && shortest == "") {
                    longest = word;
                    shortest = word;
                }
                if(word.Length > longest.Length)
                {
                    longest = word;
                }
                if (word.Length < shortest.Length)
                {
                    shortest = word;
                }

            }
            string[] res = { shortest, longest };

            return res;
        }

        public static int CountSentences(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }
            char[] endOfSentences = new char[] { '.', '!', '?' };

            text = text.Trim();

            List<string> sentences = new List<string> (text.Split(endOfSentences, StringSplitOptions.RemoveEmptyEntries));

            if (!endOfSentences.Any(c => text.EndsWith(c))) // pozostały tekst nie jest zakończony '.', '!', '?'
            {
                sentences.RemoveAt(sentences.Count - 1);
            }

            return sentences.Count();
        }

        public static float AvgCountOfWordInSentences(string text)
        {
            if (CountSentences(text) == 0) { return 0; }

            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }
            char[] endOfSentences = new char[] { '.', '!', '?' };

            text = text.Trim();

            List<string> sentences = new List<string>(text.Split(endOfSentences, StringSplitOptions.RemoveEmptyEntries));

            if (!endOfSentences.Any(c => text.EndsWith(c))) // pozostały tekst nie jest zakończony '.', '!', '?'
            {
                sentences.RemoveAt(sentences.Count - 1);
            }

            string joinedSentences = string.Join(" ", sentences);
            return (float)CountWords(joinedSentences) / CountSentences(text);
        }

        public static string LongestSentenceByNumberOfWords(string text)
        {
            char[] endOfSentences = new char[] { '.', '!', '?' };
            string[] sentences = text.Split(endOfSentences, StringSplitOptions.RemoveEmptyEntries);
            int numOfWords = 0;
            string longestSentence = "";
            string[] words;

            foreach (string sentence in sentences) {
                words = sentence.Split(_delimiters, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length > numOfWords) { 
                    numOfWords = words.Length;
                    longestSentence = sentence;
                }
            }

            return longestSentence.Trim();
        }

        public static string FindMostCommonWord(string text)
        {
            Dictionary<string, int> stats = CreateStats(text);

            if (stats.Count == 0) { return "";  }

            return stats.Keys.Last(); ;
        }

        private static Dictionary<string, int> CreateStats(string text)
        {
            string[] words = text.ToLower().Split(_delimiters, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, int> stats = new Dictionary<string, int>();

            foreach (string word in words)
            {
                if (!stats.ContainsKey(word))
                {
                    stats[word] = 1;
                }
                else
                {
                    stats[word]++;
                }
            }
            var sortedStats = stats.OrderBy(el => el.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            return sortedStats;
        }
        public static TextStatistics AnalyzeText(string text)
        {
            TextStatistics textStatistics = new TextStatistics(text);
            return textStatistics;
        }
    }
}
