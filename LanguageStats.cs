public record ZipfData(string word, int rank, int frequency, int zipfCoef, double percent);
public class LanguageStats {
    public static string[] getWords(string text) {
        return text.ToLower().Split(" \n\r\t1234567890-=!@#$%^&*()_+[]\\{}|;':\",./<>?`~–”„".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    }

    public static ZipfData[] getZipfData(string[] words) {
        var wordCount = new Dictionary<string, int>();
        int total = 0;
        foreach (var word in words) {
            if (wordCount.ContainsKey(word)) wordCount[word]++;
            else wordCount[word] = 1;
            total++;
        }

        return wordCount
            .OrderByDescending(wc => wc.Value)
            .Select((wc, i) => new ZipfData(wc.Key, i + 1, wc.Value, (i + 1) * wc.Value, (double) wc.Value / total * 100))
            .ToArray();
    }

    public static string[] getZipfDataString(ZipfData[] zipfData) {
        var maxFrequency = zipfData.Select(item => item.frequency).Max();
        var maxFrequencyLength = maxFrequency.ToString().Length;
        maxFrequencyLength = maxFrequencyLength >= 4 ? maxFrequencyLength : 4;

        var maxRank = zipfData.Count();
        var maxRankLength = maxRank.ToString().Length;
        maxRankLength = maxRankLength >= 4 ? maxRankLength : 4;
        
        var maxRFLength = (maxFrequency * maxRank).ToString().Length;
        maxRFLength = maxRFLength >= 3 ? maxRFLength : 3;

        return zipfData
            .Select(item => $"{item.rank.ToString().PadRight(maxRankLength)} | {item.frequency.ToString().PadRight(maxFrequencyLength)} | {item.zipfCoef.ToString().PadRight(maxRFLength)} | {item.percent:F7}% | {item.word}")
            .Prepend(string.Concat(Enumerable.Repeat('-', maxRankLength + maxFrequencyLength + maxRFLength + 26)))
            .Prepend($"{"rank".PadRight(maxRankLength)} | {"freq".PadRight(maxFrequencyLength)} | {"r*f".PadRight(maxRFLength)} | % of words | word")
            .ToArray();
    }

    public static ZipfData[] nPercentOfWords(ZipfData[] wordCount, int percent) {
        int n = wordCount.Sum(x => x.frequency) * percent / 100;
        int currentWordSum = 0;
        int currentWordCount = 0;
        foreach(var wc in wordCount) {
            if(currentWordSum >= n) {
                break;
            }
            else {
                currentWordSum += wc.frequency;
                currentWordCount++;
            }
        }
        return wordCount.Take(currentWordCount).ToArray();
    }

    public static Graph makeGraph(string[] words) {
        Dictionary<string, int> distinctWordOrder = words.Distinct().OrderBy(x => x).Select((word, index) => (word, index)).ToDictionary(x => x.word, x => x.index);
        int[][] matrix = new int[distinctWordOrder.Count][];
        for(int i=0; i<matrix.Length; i++) {
            matrix[i] = new int[distinctWordOrder.Count];
        }
        for(int i=0; i<words.Length-1; i++) {
            matrix[distinctWordOrder[words[i]]][distinctWordOrder[words[i+1]]]++;
        }
        Graph graph = new(distinctWordOrder, matrix);
        
        return graph;
    }
}