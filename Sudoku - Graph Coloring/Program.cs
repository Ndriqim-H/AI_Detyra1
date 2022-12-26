using System.Security.Cryptography.X509Certificates;

namespace Sudoku_Graph_Coloring_Final_Solution
{
    class Program
    {
        public static void Main(string[] args)
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
                { 0, 0, 0, 2, 6, 0, 7, 0, 1 }, //1,2,6,7,8,9
                { 6, 8, 0, 0, 7, 0, 0, 9, 0 },
                { 1, 9, 0, 0, 0, 4, 5, 0, 0 },
                { 8, 2, 0, 1, 0, 0, 0, 4, 0 },
                { 0, 0, 4, 6, 0, 2, 9, 0, 0 },
                { 0, 5, 0, 0, 0, 3, 0, 2, 8 },
                { 0, 0, 9, 3, 0, 0, 0, 7, 4 },
                { 0, 4, 0, 0, 5, 0, 0, 3, 6 },
                { 7, 0, 3, 0, 1, 8, 0, 0, 0 }};

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


            int[,] sudoku = initialState;



            List<Node> nodes = new();
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                for (int j = 0; j < sudoku.GetLength(1); j++)
                {
                    //List<Node> neighbors = new();

                    nodes.Add(new Node(sudoku[i, j], sudoku[i, j] != 0, i, j));
                }
            }
            Sudoku sud = new(nodes);
            var y = sud.GetChildren(nodes);

            Sudoku.UpdateNodes(nodes);
            var x = nodes[0];
            x.GetChildren();

            //BFS = Queue; DFS = Stack
            Queue<Sudoku> OpenList = new();
            List<Sudoku> ClosedList = new();
            OpenList.Enqueue(new Sudoku(nodes));

            while (OpenList.Count > 0)
            {

                Sudoku currentNode = OpenList.Dequeue();
                //showSudoku(currentNode.State);
                ClosedList.Add(currentNode);
                if (currentNode.IsGoal())
                {
                    Console.WriteLine("Goal Found");
                    Sudoku.ShowSudoku(currentNode.State);
                    break;
                }
                else
                {
                    var children = currentNode.GetChildren(currentNode.State);
                    foreach (var child in children)
                    {
                        if (!ClosedList.Contains(child))
                        {
                            OpenList.Enqueue(child);
                        }
                    }
                }

            }

            Console.WriteLine("Solved");
            //showSudoku(nodes);
        }
        

        


        public static bool CheckIfSolved(List<Node> nodes) => nodes.All(t => t.Color != 0);

    }

    class Sudoku
    {
        public List<Node> State { get; set; } = new();
        public Sudoku(List<Node> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                State.Add(new Node(nodes[i].Color, nodes[i].Initial, nodes[i].RowId, nodes[i].ColId));
            }

            UpdateNodes(State);
        }


        public static void UpdateNodes(List<Node> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                List<Node> sameRow = nodes.Where(t => t.RowId == nodes[i].RowId && t.ColId != nodes[i].ColId).ToList();
                List<Node> sameCol = nodes.Where(t => t.ColId == nodes[i].ColId && t.RowId != nodes[i].RowId).ToList();
                List<Node> sameSubMatrix = nodes.Where(t => (t.RowId / 3 == nodes[i].RowId / 3 && t.ColId / 3 == nodes[i].ColId / 3)).ToList();
                sameSubMatrix = sameSubMatrix.Where(t => !(t.RowId == nodes[i].RowId && t.ColId == nodes[i].ColId)).ToList();

                List<Node> neighbors = new(sameRow);
                neighbors.AddRange(sameCol);
                neighbors.AddRange(sameSubMatrix);
                nodes[i].Neighbors = neighbors;
            }
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
                x[variables[i].RowId, variables[i].ColId] = variables[i].Color;

            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write($"{x[i, j]} ");
                }
                Console.WriteLine();
            }
        }

        public bool IsGoal() => State.All(t => t.Color != 0);

        public List<Sudoku> GetChildren(List<Node> nodes)
        {
            List<Sudoku> children = new();
            Node EmptyVariable = State.Where(t => t.Color == 0).FirstOrDefault();
            if (EmptyVariable == null)
                return null;
            List<Node> variableChildren = EmptyVariable.GetChildren();
            for (int i = 0; i < variableChildren.Count; i++)
            {
                Sudoku newChild = new(State);
                Node childNode = newChild.State.Where(t => t.RowId == EmptyVariable.RowId && t.ColId == EmptyVariable.ColId).FirstOrDefault();
                childNode.Color = variableChildren[i].Color;
                children.Add(newChild);
            }

            return children;
        }

    }


    //Write a class that represents a graph coloring node
    class Node
    {
        public List<int> Colors = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public int RowId { get; set; }
        public int ColId { get; set; }
        public bool Initial { get; set; }
        public Stack<Node> Children { get; set; } = new();
        public List<Node> UsedChildren { get; set; } = new();
        public List<Node> Neighbors { get; set; }
        public int Color { get; set; }


        public Node(int variable, bool initial, int row, int column)
        {
            if (initial)
                Colors = new() { variable };
            else
                Colors = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Color = variable;
            RowId = row;
            ColId = column;
            Initial = initial;
        }

        public List<Node> GetChildren()
        {
            List<Node> children = new();
            if (Initial)
            {
                children.Add(new Node(Colors[0], true, RowId, ColId));

                Children.Push(children[0]);
                return children;
                //return children;
            }

            for (int i = 0; i < Colors.Count; i++)
            {
                if (Neighbors.All(t => t.Color != Colors[i]))
                {
                    Node child = new Node(Colors[i], false, RowId, ColId);
                    //child.Color = Colors[i];
                    //child.Id = Id++;
                    //child.Neighbors = Neighbors;
                    children.Add(child);
                    //Children.Push(child);
                    //if (!UsedChildren.Contains(child) && !Children.Contains(child))
                    //    Children.Push(child);
                    //else
                    //    continue;
                }
            }

            return children;

        }

        override
            public bool Equals(object obj)
        {
            Node node = (Node)obj;


            return Color == node.Color && RowId == node.RowId && ColId == node.ColId;
        }
    }

}