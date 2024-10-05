using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;

public class LanguageStats {

    public static string[] getWords(string text) {
        return text.ToLower().Split(" \n\r\t1234567890-=!@#$%^&*()_+[]\\{}|;':\",./<>?`~–”„".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    }

    public static Dictionary<string, int> getWordCountDict(string[] words) {
        Dictionary<string, int> wordCount = new();
        for(int i=0; i<words.Length; i++) {
            string word = words[i];
            if(wordCount.ContainsKey(word)) {
                wordCount[word]++;
            } else {
                wordCount[word] = 1;
            }
        }
        return wordCount;
    }

    public static Dictionary<string, int> nPercentOfWords(Dictionary<string, int> wordCount, int percent) {
        int n = wordCount.Sum(x => x.Value) * percent / 100;
        int currentWordSum = 0;
        int currentWordCount = 0;
        foreach(var wc in wordCount.OrderByDescending(x => x.Value)) {
            if(currentWordSum >= n) {
                break;
            }
            else {
                currentWordSum += wc.Value;
                currentWordCount++;
            }
        }
        return wordCount.OrderByDescending(x => x.Value).Take(currentWordCount).ToDictionary(x => x.Key, x => x.Value);
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

    public static string[] wordCountToString(Dictionary<string, int> wordCount) {
        int total = wordCount.Sum(x => x.Value);
        int maxFLen = wordCount.Select(wc => wc.Value).Max().ToString().Length;
        return wordCount.OrderBy(x => x.Value).Reverse()
            .Select((wc, i) => 
                wc.Value.ToString().PadLeft(maxFLen) + "  " 
                + (wc.Value * (i+1)).ToString().PadRight(5) + "  "
                + ((float)wc.Value  / total).ToString("F7") + " %   "
                + wc.Key
            )
            .ToArray();
    }

    public static string generateExample(int wordLength, int letterCount, int wordCount) {
        string example = "";
        for(int i=0; i<wordCount; i++) {
            for(int j=0; j<wordLength; j++) {
                example += (char)('a' + new Random().NextInt64() % letterCount);
            }
            example += " ";
        }
        return example;

    }
}