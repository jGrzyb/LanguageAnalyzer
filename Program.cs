public class Program {
    public static void Main() {
        string resultsDirectory = "Results";
        string text = File.ReadAllText("book.txt");
        if (!Directory.Exists(resultsDirectory)) {
            Directory.CreateDirectory(resultsDirectory);
        }
        string[] words = LanguageStats.getWords(text);

        var zipfData = LanguageStats.getZipfData(words);
        var zipfDataString = LanguageStats.getZipfDataString(zipfData);
        File.WriteAllLines($"{resultsDirectory}/WordCount.txt", zipfDataString);
        Console.WriteLine("Word count saved.");
        
        for(int i=1; i<=5; i++) {
            File.WriteAllLines($"{resultsDirectory}/{i*10}percentWords.txt", LanguageStats.getZipfDataString(LanguageStats.nPercentOfWords(zipfData, i*10)));
        }

        Graph graph = LanguageStats.makeGraph(words);

        File.WriteAllLines($"{resultsDirectory}/EdgeWeight.txt", graph.edgesToStringArr());
        Console.WriteLine("Edge weight saved");

        File.WriteAllLines($"{resultsDirectory}/EdgeCount.txt", graph.edgeCountToStringArr());
        Console.WriteLine("Edge count saved");

        File.WriteAllLines($"{resultsDirectory}/EdgeWeightSum.txt", graph.edgeWeightSumToStringArr());
        Console.WriteLine("Edge weight sum saved");

        Console.WriteLine("Success");
    }
}