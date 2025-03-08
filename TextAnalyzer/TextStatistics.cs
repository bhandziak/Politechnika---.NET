using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TextAnalyzerProject
{
    public class TextStatistics
    {
        public int CharacterCount;
        public int CharacterCountWithOutWhiteChars;
        public int LettersCount;
        public int NumberCount;
        public int PunctuationMarksCount;

        public int WordCount;
        public int UniqueWordsCount;
        public float AverageWordLenght;
        public string LongestWord;
        public string ShortestWord;
        public int SentenceCount;

        public float AverageCountOfWordInSentences;
        public string LongestSentence;
        public string MostCommonWord;

        public TextStatistics(string v_text)
        {

            CharacterCount = TextAnalyzer.CountCharacters(v_text);
            CharacterCountWithOutWhiteChars = TextAnalyzer.CountCharatersWithOutWhiteChars(v_text);
            LettersCount = TextAnalyzer.CountLetters(v_text);
            NumberCount = TextAnalyzer.CountNumbers(v_text);
            PunctuationMarksCount = TextAnalyzer.CountPunctuationMarks(v_text);

            WordCount = TextAnalyzer.CountWords(v_text);
            UniqueWordsCount = TextAnalyzer.CountUniqueWords(v_text);
            AverageWordLenght = TextAnalyzer.AvgLenWord(v_text);
            ShortestWord = TextAnalyzer.LongestAndShortestWord(v_text)[0];
            LongestWord = TextAnalyzer.LongestAndShortestWord(v_text)[1];
            SentenceCount = TextAnalyzer.CountSentences(v_text);

            AverageCountOfWordInSentences = TextAnalyzer.AvgCountOfWordInSentences(v_text);
            LongestSentence = TextAnalyzer.LongestSentenceByNumberOfWords(v_text);
            MostCommonWord = TextAnalyzer.FindMostCommonWord(v_text);

        }

        public void printStatistics()
        {
            Console.WriteLine("Statystyki tekstu:");
            Console.WriteLine("-------------------\n");

            Console.WriteLine($"Liczba znaków: {CharacterCount}");
            Console.WriteLine($"Liczba znaków bez spacji: {CharacterCountWithOutWhiteChars}");
            Console.WriteLine($"Liczba liter: {LettersCount}");
            Console.WriteLine($"Liczba cyfr: {NumberCount}");
            Console.WriteLine($"Liczba znaków interpunkcyjnych: {PunctuationMarksCount}");

            Console.WriteLine($"Liczba słów: {WordCount}");
            Console.WriteLine($"Liczba unikalnych słów: {UniqueWordsCount}");
            Console.WriteLine($"Średnia długość słowa: {AverageWordLenght}");
            Console.WriteLine($"Najdłuższe słowo: {LongestWord}");
            Console.WriteLine($"Najkrótsze słowo: {ShortestWord}");

            Console.WriteLine($"Liczba zdań: {SentenceCount}");

            Console.WriteLine($"Średnia liczba słów w zdaniu: {AverageCountOfWordInSentences}");
            Console.WriteLine($"Najdłuższe zdanie: {LongestSentence}");
            Console.WriteLine($"Najczęściej występujące słowo: {MostCommonWord}");
        }
    }
}
