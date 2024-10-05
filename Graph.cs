using System.Text;

public class Graph {
    public Dictionary<string, int> distinctWordOrder;
    public int[][] matrix;
    public Edge[] edges;

    public Graph(Dictionary<string, int> distinctWordOrder, int[][] matrix) {
        this.distinctWordOrder = distinctWordOrder;
        this.matrix = matrix;
        edges = getEdges();
    }

    private Edge[] getEdges() {
        string[] names = distinctWordOrder.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
        return matrix.SelectMany((x, i) => x.Select((y, j) => (i, j, y))).Where(a => a.Item3 > 0).OrderByDescending(a => a.Item3)
            .Select(x => new Edge(names[x.i], names[x.j], x.y)).ToArray();
    }

    public string[] edgesToStringArr() {
        int maxNumberLen = matrix.SelectMany(x => x).Max().ToString().Length;
        int maxWordLen = distinctWordOrder.Select(x => x.Key).Max(x => x.Length);

        // string[] names = distinctWordOrder.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
        // string[] lines = matrix.SelectMany((x, i) => x.Select((y, j) => (i, j, y))).Where(a => a.y > 0).OrderByDescending(a => a.y)
        //     .Select(x => x.y.ToString().PadLeft(maxNumberLen) + ":  " + names[x.i].PadLeft(maxWordLen) + "  ->  " + names[x.j]).ToArray();
        
        string[] lines = edges.Select(x => x.count.ToString().PadLeft(maxNumberLen) + ":  " + x.startWord.PadLeft(maxWordLen) + "  ->  " + x.endWord).ToArray();
        return lines;
    }

    public string[] edgeCountToStringArr() {
        int maxNumberLen = matrix.Select(x => x.Where(y => y > 0).Count()).Max().ToString().Length;
        int maxWordLen = distinctWordOrder.Select(x => x.Key).Max(x => x.Length);

        // string[] names = distinctWordOrder.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
        // string[] lines = matrix.ToList().OrderByDescending(x => x.Where(y => y > 0).Count()).Select((x, i) => x.Where(y => y > 0).Count().ToString().PadLeft(maxNumberLen) + ":  " + names[i]).ToArray();
        
        string[] lines = edges.GroupBy(x => x.startWord).Select(x => (x.Key, x.Count())).OrderByDescending(x => x.Item2).Select(x => x.Item2.ToString().PadLeft(maxNumberLen) + ":  " + x.Key).ToArray();
        return lines;
    }

    public string[] edgeWeightSumToStringArr() {
        int maxNumberLen = matrix.Select(x => x.Sum()).Max().ToString().Length;
        int maxWordLen = distinctWordOrder.Select(x => x.Key).Max(x => x.Length);

        // string[] names = distinctWordOrder.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
        // string[] lines = matrix.ToList().OrderByDescending(x => x.Sum()).Select((x, i) => x.Sum().ToString().PadLeft(maxNumberLen) + ":  " + names[i]).ToArray();
        
        string[] lines = edges.GroupBy(x => x.startWord).Select(x => (x.Key, x.Sum(y => y.count))).OrderByDescending(x => x.Item2).Select(x => x.Item2.ToString().PadLeft(maxNumberLen) + ":  " + x.Key).ToArray();
        return lines;
    }

    public Graph FilterGraph(string[] choosenWords) {
        Dictionary<string, int> newDistinctWordOrder = choosenWords.Distinct().OrderBy(x => x).Select((word, index) => (word, index)).ToDictionary(x => x.word, x => x.index);
        int[][] newMatrix = new int[choosenWords.Length][];
        for(int i=0; i<newMatrix.Length; i++) {
            newMatrix[i] = new int[choosenWords.Length];
        }
        for(int i=0; i<choosenWords.Length; i++) {
            for(int j=0; j<choosenWords.Length; j++) {
                newMatrix[newDistinctWordOrder[choosenWords[i]]][newDistinctWordOrder[choosenWords[j]]] = matrix[distinctWordOrder[choosenWords[i]]][distinctWordOrder[choosenWords[j]]];
            }
        }
        Graph graph = new(newDistinctWordOrder, newMatrix);
        return graph;
    }
}