using NUnit.Framework;
using System.Collections.Generic;
using TextAnalyzerProject;
[TestFixture]
public class TextAnalyzerTests
{
    [Test]
    public void CountCharacters_ShouldReturnCorrectNumber()
    {
        var text = "Hello, world!";
        int result = TextAnalyzer.CountCharacters(text);
        Assert.AreEqual(13, result);
    }

    [Test]
    public void CharacterCountWithOutWhiteChars_ShouldReturnCorrectNumber()
    {
        var text = "Hello, world!";
        int result = TextAnalyzer.CountCharatersWithOutWhiteChars(text);
        Assert.AreEqual(12, result);
    }

    public void LettersCount_ShouldReturnCorrectNumber()
    {
        var text = "Hello world! How are you? I am fine.";
        int result = TextAnalyzer.CountLetters(text);
        Assert.AreEqual(26, result);

    }


    [Test]
    public void CountWords_ShouldReturnCorrectNumber()
    {
        var text = "Hello world!";
        int result = TextAnalyzer.CountWords(text);
        Assert.AreEqual(2, result);
    }

    [Test]
    public void CountSentences_ShouldReturnCorrectNumber()
    {
        var text = "Hello world! How are you? I am fine.";
        int result = TextAnalyzer.CountSentences(text);
        Assert.AreEqual(3, result);
    }

    [Test]
    public void MostCommonWord_ShouldReturnCorrectWord()
    {
        var text = "apple banana apple orange apple banana";
        string result = TextAnalyzer.FindMostCommonWord(text);
        Assert.AreEqual("apple", result);
    }

    [Test]
    public void CountNumbers_ShouldReturnCorrectNumber()
    {
        var text = "There are 3 apples and 5 bananas.";
        int result = TextAnalyzer.CountNumbers(text);
        Assert.AreEqual(2, result);
    }

    [Test]
    public void CountPunctuationMarks_ShouldReturnCorrectNumber()
    {
        var text = "Hello, world! How are you?";
        int result = TextAnalyzer.CountPunctuationMarks(text);
        Assert.AreEqual(3, result);
    }


    [Test]
    public void CountUniqueWords_ShouldReturnCorrectNumber()
    {
        var text = "Hello world! Hello again.";
        int result = TextAnalyzer.CountUniqueWords(text);
        Assert.AreEqual(3, result);
    }



    [Test]
    public void LongestAndShortestWord_ShouldReturnCorrectString()
    {
        var text = "A quick brown fox jumps over the lazy dog longwordsssssssssssss";
        var words = TextAnalyzer.LongestAndShortestWord(text);
        Assert.AreEqual("a", words[0]);
        Assert.AreEqual("longwordsssssssssssss", words[1]);
    }

    [Test]
    public void AvgCountOfWordInSentences_ShouldReturnCorrectNumber()
    {
        var text = "Hello world! How are you? I am fine.";
        float result = TextAnalyzer.AvgCountOfWordInSentences(text);
        Assert.AreEqual(2.67f, result, 0.1);
    }

    [Test]
    public void LongestSentenceByNumberOfWords_ShouldReturnCorrectString()
    {
        var text = "This is a test. Another longer sentence with more words. Short one.";
        string result = TextAnalyzer.LongestSentenceByNumberOfWords(text);
        Assert.AreEqual("Another longer sentence with more words", result);
    }

    [Test]
    public void AnalyzeText_WithEmptyString_ShouldReturnZeroes()
    {
        var text = "";
        var result = TextAnalyzer.AnalyzeText(text);

        Assert.AreEqual(0, result.CharacterCount);
        Assert.AreEqual(0, result.CharacterCountWithOutWhiteChars);
        Assert.AreEqual(0, result.LettersCount);
        Assert.AreEqual(0, result.NumberCount);
        Assert.AreEqual(0, result.PunctuationMarksCount);

        Assert.AreEqual(0, result.WordCount);
        Assert.AreEqual(0, result.UniqueWordsCount);
        Assert.AreEqual(0, result.AverageWordLenght);
        Assert.AreEqual("", result.ShortestWord);
        Assert.AreEqual("", result.LongestWord);
        Assert.AreEqual(0, result.SentenceCount);


        Assert.AreEqual(0, result.AverageCountOfWordInSentences);
        Assert.AreEqual("", result.LongestSentence);
        Assert.AreEqual("", result.MostCommonWord);
    }

    [Test]
    public void AnalyzeText_WithWhiteSpaces_ShouldReturnZeroes()
    {
        var text = "                   ";
        var result = TextAnalyzer.AnalyzeText(text);

        Assert.AreEqual(19, result.CharacterCount);
        Assert.AreEqual(0, result.CharacterCountWithOutWhiteChars);
        Assert.AreEqual(0, result.LettersCount);
        Assert.AreEqual(0, result.NumberCount);
        Assert.AreEqual(0, result.PunctuationMarksCount);

        Assert.AreEqual(0, result.WordCount);
        Assert.AreEqual(0, result.UniqueWordsCount);
        Assert.AreEqual(0, result.AverageWordLenght);
        Assert.AreEqual("", result.ShortestWord);
        Assert.AreEqual("", result.LongestWord);
        Assert.AreEqual(0, result.SentenceCount);


        Assert.AreEqual(0, result.AverageCountOfWordInSentences);
        Assert.AreEqual("", result.LongestSentence);
        Assert.AreEqual("", result.MostCommonWord);
    }

    [Test]
    public void AnalyzeLongText_ShouldReturnZeroes()
    {
        var text = "Duis condimentum nunc mollis lorem fermentum interdum. In vitae dolor nulla. Sed semper ante eu nunc consequat fermentum. Suspendisse ornare sit amet eros eget vehicula. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis semper vulputate felis, in eleifend mauris iaculis at. Sed eu turpis condimentum, maximus neque ut, pulvinar felis. Phasellus ullamcorper lobortis felis sit amet aliquet. Integer ullamcorper turpis non pellentesque varius. In hac habitasse platea dictumst. Duis Duis Duis Duis condimentum nunc";
        var result = TextAnalyzer.AnalyzeText(text);

        Assert.AreEqual(525, result.CharacterCount);
        Assert.AreEqual(451, result.CharacterCountWithOutWhiteChars);
        Assert.AreEqual(437, result.LettersCount);
        Assert.AreEqual(0, result.NumberCount);
        Assert.AreEqual(14, result.PunctuationMarksCount);

        Assert.AreEqual(75, result.WordCount);
        Assert.AreEqual(50, result.UniqueWordsCount);

        Assert.AreEqual("at", result.ShortestWord);
        Assert.AreEqual("pellentesque", result.LongestWord);
        Assert.AreEqual(10, result.SentenceCount);


        Assert.AreEqual("Duis semper vulputate felis, in eleifend mauris iaculis at", result.LongestSentence);
        Assert.AreEqual("duis", result.MostCommonWord);
    }
}