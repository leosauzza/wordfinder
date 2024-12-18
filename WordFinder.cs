using System.Collections.Concurrent;

namespace Utilities;
public class WordFinder
{
    private readonly char[,] _matrix;
    private readonly int _rows;
    private readonly int _cols;

    public WordFinder(IEnumerable<string> matrix)
    {
        if (matrix is null || !matrix.Any())
        {
            throw new ArgumentException("Matrix cannot be null or empty");
        }

        var matrixArray = matrix.ToArray();
        _rows = matrixArray.Length;
        _cols = matrixArray[0].Length;

        if (matrixArray.Any(row => row.Length != _cols))
        {
            throw new ArgumentException("All rows in the matrix must have the same length");
        }

        _matrix = new char[_rows, _cols];

        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                _matrix[i, j] = matrixArray[i][j];
            }
        }
    }

    public IEnumerable<string> Find(IEnumerable<string> wordstream)
    {
        if (wordstream == null || !wordstream.Any())
        {
            return Enumerable.Empty<string>();
        }

        var filteredWordstream = wordstream.Where(word => word.Length <= _rows && word.Length <= _cols);
        var wordSet = new HashSet<string>(filteredWordstream);
        var wordCounts = new ConcurrentDictionary<string, int>();

        Parallel.ForEach(wordSet, word =>
        {
            int count = SearchWord(word);
            if (count > 0)
            {
                wordCounts.AddOrUpdate(word, count, (key, oldValue) => oldValue + count);
            }
        });

        return wordCounts
            .OrderByDescending(kvp => kvp.Value)
            .ThenBy(kvp => kvp.Key)
            .Take(10)
            .Select(kvp => kvp.Key);
    }

    private int SearchWord(string word)
    {
        int wordLength = word.Length;
        int totalCount = 0;

        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col <= _cols - wordLength; col++)
            {
                if (IsHorizontalMatch(row, col, word))
                {
                    totalCount++;
                }
            }
        }

        for (int col = 0; col < _cols; col++)
        {
            for (int row = 0; row <= _rows - wordLength; row++)
            {
                if (IsVerticalMatch(row, col, word))
                {
                    totalCount++;
                }
            }
        }

        return totalCount;
    }

    private bool IsHorizontalMatch(int row, int col, string word)
    {
        for (int i = 0; i < word.Length; i++)
        {
            if (_matrix[row, col + i] != word[i])
            {
                return false;
            }
        }
        return true;
    }

    private bool IsVerticalMatch(int row, int col, string word)
    {
        for (int i = 0; i < word.Length; i++)
        {
            if (_matrix[row + i, col] != word[i])
            {
                return false;
            }
        }
        return true;
    }
}