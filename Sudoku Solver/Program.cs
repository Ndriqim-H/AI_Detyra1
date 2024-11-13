using System.Drawing;
using System.Xml.Linq;

class Sudoku_Solver
{
    static void Main()
    {
        int[,] initialState = new int[,] {
            { 5, 0, 8, 4, 0, 2, 0, 0, 1 },
            { 3, 0, 0, 9, 5, 1, 0, 7, 8 },
            { 1, 0, 0, 6, 0, 0, 0, 0, 5 },
            { 0, 3, 4, 0, 8, 9, 0, 2, 0 },
            { 0, 0, 0, 1, 2, 3, 0, 9, 7 },
            { 0, 1, 9, 7, 4, 6, 0, 0, 3 },
            { 0, 0, 1, 0, 0, 0, 0, 6, 0 },
            { 0, 2, 6, 8, 0, 0, 0, 0, 4 },
            { 0, 7, 0, 0, 0, 0, 0, 1, 0 }
        };

        int[,] initialState2 = new int[,] {
            { 0, 0, 0, 2, 6, 0, 7, 0, 1 },
            { 6, 8, 0, 0, 7, 0, 0, 9, 0 },
            { 1, 9, 0, 0, 0, 4, 5, 0, 0 },
            { 8, 2, 0, 1, 0, 0, 0, 4, 0 },
            { 0, 0, 4, 6, 0, 2, 9, 0, 0 },
            { 0, 5, 0, 0, 0, 3, 0, 2, 8 },
            { 0, 0, 9, 3, 0, 0, 0, 7, 4 },
            { 0, 4, 0, 0, 5, 0, 0, 3, 6 },
            { 7, 0, 3, 0, 1, 8, 0, 0, 0 }
        };

        int[,] initialState3 = new int[,] {
                {0, 2, 0, 0, 0, 0, 0, 0, 0 },
                {0, 0, 0, 6, 0, 0, 0, 0, 3 },
                {0, 7, 4, 0, 8, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0, 3, 0, 0, 2 },
                {0, 8, 0, 0, 4, 0, 0, 1, 0 },
                {6, 0, 0, 5, 0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 1, 0, 7, 8, 0 },
                {5, 0, 0, 0, 0, 9, 0, 0, 0 },
                {0, 0, 0, 0, 0, 0, 0, 4, 0 }};

        Sudoku sudoku = new Sudoku(initialState2);
        Sudoku.ShowSudoku(sudoku.Nodes);
        Console.WriteLine("==========================");

        Stack<Sudoku> stack = new();
        List<Sudoku> closedList = new();
        stack.Push(sudoku);
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (Sudoku.CheckIfSolved(current.Nodes))
            {
                Console.WriteLine($"Solved! At Depth: {current.Depth}");
                Sudoku.ShowSudoku(current.Nodes);
                break;
            }
            closedList.Add(current);
            var children = current.GetChildren();
            foreach (var child in children)
            {
                if (!closedList.Contains(child))
                {
                    stack.Push(child);
                }
                else
                {
                    var y = 0;
                }
            }
        }

        //Console.WriteLine("No solution found.");


    }
}


class Node(int row, int column, int value = 0, bool initial = false, int[]? domain = null)
{
    private static readonly int[] InitialDomain = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    public int RowId { get; set; } = row;
    public int ColId { get; set; } = column;
    public int[] Domain { get; set; } = initial ? new int[] { value } : domain ?? InitialDomain;
    public int Variable { get; set; } = value;
    public bool Initial { get; set; } = initial;
}
class Sudoku
{
    public List<Node> Nodes { get; set; }
    public int Depth { get; set; }
    public Sudoku(int[,] variables, int depth = 0)
    {
        Nodes = new List<Node>();
        for (int i = 0; i < variables.GetLength(0); i++)
        {
            for (int j = 0; j < variables.GetLength(1); j++)
            {
                Node node = new(i, j, variables[i, j], variables[i, j] != 0);
                Nodes.Add(node);
            }
        }
        Depth = depth;
    }

    public Sudoku(List<Node> variables, int depth = 0)
    {
        Nodes = new List<Node>();
        foreach (var variable in variables)
        {
            Nodes.Add(new Node(variable.RowId, variable.ColId, variable.Variable, variable.Initial, variable.Domain));
        }
        Depth = depth;
    }

    public List<Sudoku> GetChildren()
    {
        var children = new List<Sudoku>();
        var node = Nodes.FirstOrDefault(t => t.Variable == 0);
        for (int i = 0; i < node.Domain.Length; i++)
        {
            var newNodes = Nodes.Select(t => t.Equals(node) ? new Node(t.RowId, t.ColId, node.Domain[i], false, node.Domain) : t).ToList();
            if (ValidateSudoku(newNodes))
                children.Add(new Sudoku(newNodes, Depth + 1));
        }

        return children;
    }

    //Check if all cells are filled.
    public static bool CheckIfSolved(List<Node> variable) => !variable.Any(t => t.Variable == 0);

    // Validate the sudoku
    public static bool ValidateSudoku(List<Node> nodes)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            if (node.Variable == 0)
                continue;
            if (nodes.Where(t => t.RowId == node.RowId && t.Variable == node.Variable).Count() > 1)
                return false;
            if (nodes.Where(t => t.ColId == node.ColId && t.Variable == node.Variable).Count() > 1)
                return false;
            if (nodes.Where(t => t.RowId / 3 == node.RowId / 3 && t.ColId / 3 == node.ColId / 3 && t.Variable == node.Variable).Count() > 1)
                return false;
        }
        return true;
    }

    public static void ShowSudoku(List<Node> variables)
    {
        //Create a 9x9 0 matrix.
        int[,] x = new[,]
        {
                {0,0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0,0 },
                {0,0,0,0,0,0,0,0,0 }
            };

        for (int i = 0; i < variables.Count; i++)
        {
            x[variables[i].RowId, variables[i].ColId] = variables[i].Variable;

        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (x[i, j] != 0)
                    Console.Write($"{x[i, j]} ");
                else
                    Console.Write("  ");
            }
            Console.WriteLine();
        }
    }

}
