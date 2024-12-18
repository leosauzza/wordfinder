using Xunit;
using Utilities;

public class WordFinderTest
{
    [Fact]
    public void Constructor_ValidMatrix_ShouldInitializeCorrectly()
    {
        var matrix = new List<string> {     
            "mhola",
            "algog",
            "miuzt",
            "acvan" 
        };
        var wordFinder = new WordFinder(matrix);
        Assert.NotNull(wordFinder);
    }

    [Fact]
    public void Constructor_InvalidMatrix_ShouldThrowArgumentException()
    {
        var matrix = new List<string>{
            "mhola",
            "algog",
            "miuzt",
            "acvanasd"
        }; 
        Assert.Throws<ArgumentException>(() => new WordFinder(matrix));
    }

    [Fact]
    public void Find_EmptyWordstream_ShouldReturnEmptySet()
    {
        var matrix = new List<string>{
            "mhola",
            "algog",
            "miuzt",
            "acvan"
        };
        var wordFinder = new WordFinder(matrix);
        var wordstream = Enumerable.Empty<string>();
        var result = wordFinder.Find(wordstream);
        Assert.Empty(result);
    }

    [Fact]
    public void Find_WordNotInMatrix_ShouldReturnEmptySet()
    {
        var matrix = new List<string> {     
            "mhola",
            "algog",
            "miuzt",
            "acvan" 
        };
        var wordFinder = new WordFinder(matrix);
        var wordstream = new List<string> { "perro", "gato", "pico"};
        var result = wordFinder.Find(wordstream);
        Assert.Empty(result);
    }

    [Fact]
    public void Find_WordsInMatrix_ShouldReturnTopMatches()
    {
        var matrix = new List<string> {     
            "mhola",
            "algog",
            "miuzt",
            "holan" 
        };
        var wordFinder = new WordFinder(matrix);
        var wordstream = new List<string> {"algo", "loza", "mamh", "hola", "ola", "loza", "perro","hola"};
        var result = wordFinder.Find(wordstream);

        Assert.Contains("hola", result);
        Assert.Contains("ola", result);
        Assert.Contains("mamh", result);
        Assert.Contains("algo", result);
        Assert.Contains("loza", result);
        Assert.Equal(5, result.Count());
    }
}