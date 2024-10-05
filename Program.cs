

using System.Security.Cryptography.X509Certificates;

public class Program {
    public static void Main() {
        string resultsDirectory = "Results";
        string text = File.ReadAllText("book.txt");

        // string text = LanguageStats.generateExample(2, 3, 100);
        // File.WriteAllText("SourceExample.txt", text);

        // string text = File.ReadAllText("SourceExample.txt");
        if (!Directory.Exists(resultsDirectory)) {
            Directory.CreateDirectory(resultsDirectory);
        }
        string[] words = LanguageStats.getWords(text);

        Dictionary<string, int> wordCount = LanguageStats.getWordCountDict(words);
        string[] wordCountString = LanguageStats.wordCountToString(wordCount);
        File.WriteAllLines($"{resultsDirectory}/WordCount.txt", wordCountString);
        Console.WriteLine("Word count saved");

        Graph graph = LanguageStats.makeGraph(words);

        File.WriteAllLines($"{resultsDirectory}/EdgeWeight.txt", graph.edgesToStringArr());
        Console.WriteLine("Edge weight saved");

        File.WriteAllLines($"{resultsDirectory}/EdgeCount.txt", graph.edgeCountToStringArr());
        Console.WriteLine("Edge count saved");

        File.WriteAllLines($"{resultsDirectory}/EdgeWeightSum.txt", graph.edgeWeightSumToStringArr());
        Console.WriteLine("Edge weight sum saved");

        for(int i=1; i<=5; i++) {
            File.WriteAllLines($"{resultsDirectory}/{i*10}percentWords.txt", LanguageStats.wordCountToString(LanguageStats.nPercentOfWords(wordCount, i*10)));
        }

        Console.WriteLine("Success");

        // Graph filteredGraph = graph.FilterGraph(wordCount.OrderBy(x => x.Value).Reverse().Take(20).Select(x => x.Key).ToArray());
        // string graphString = filteredGraph.graphToString();
        // File.WriteAllText("GraphMatrix.txt", graphString);
    }
}