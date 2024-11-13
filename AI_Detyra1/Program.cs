using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_2022___Console
{

    //The main program for the Sudoku problem
    internal class Program
    {

        public static void updateNodes(List<Sudoku> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                List<Sudoku> sameRow = nodes.Where(t => t.Row == nodes[i].Row && t.Col != nodes[i].Col).ToList();
                List<Sudoku> sameCol = nodes.Where(t => t.Col == nodes[i].Col && t.Row != nodes[i].Row).ToList();
                List<Sudoku> sameSubMatrix = nodes.Where(t => (t.Row / 3 == nodes[i].Row / 3 && t.Col / 3 == nodes[i].Col / 3)).ToList();
                sameSubMatrix = sameSubMatrix.Where(t => !(t.Row == nodes[i].Row && t.Col == nodes[i].Col)).ToList();

                List<Sudoku> neighbors = new(sameRow);
                neighbors.AddRange(sameCol);
                neighbors.AddRange(sameSubMatrix);
                nodes[i].Neighbors = neighbors;
            }
        }

        public static void Main()
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
                { 0, 7, 0, 0, 0, 0, 0, 1, 0 }};


            int[,] initialState2 = new int[,] {
                { 0, 0, 0, 2, 6, 0, 7, 0, 1 },
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
            int[,] sudoku = initialState3;
            List<Sudoku> variables = new();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    variables.Add(new Sudoku(sudoku[i, j], sudoku[i, j] != 0, i, j));
                }
            }
            updateNodes(variables);
            showSudoku(variables);
            Console.WriteLine("==========================");
            Console.WriteLine("Solving...");
            Stack<Sudoku> variableStack = new();
            variableStack.Push(variables[0]);
            bool backTracking = false;
            bool solved = false;

            while (variableStack.Count > 0)
            {
                //We take the first element from the stack without removing it.
                Sudoku currentNode = variableStack.Peek();

                //Show the current state of the Sudoku.
                //Console.WriteLine("_________________________");
                //showSudoku(variables.ToList());
                //Console.WriteLine("_________________________");

                //If we are at the last cell, we check differently since the are no more future cells.
                if (variableStack.Count == 81)
                {
                    //if (checkIfSolved(variables))
                    if (!(currentNode.Domain.Count == 0))
                    {
                        currentNode.Variable = currentNode.Domain[0];
                        if (!currentNode.Initial)
                            currentNode.Domain.RemoveAt(0);
                        if (!checkStateValid(currentNode, variables) && !currentNode.Initial)
                        {
                            //OpenList.Pop();
                            continue;
                        }
                        solved = true;
                        break;

                    }
                    else
                    {
                        backTracking = true;
                        currentNode.Domain = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                        currentNode.Variable = 0;
                        variableStack.Pop();
                        continue;
                    }
                }

                //If the domain of the cell is empty we know that something went wrong and we need to backtrack.
                if (currentNode.Domain.Count == 0)
                {
                    backTracking = true;
                    currentNode.Domain = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    currentNode.Variable = 0;
                    variableStack.Pop();
                    continue;
                }
                //If we are backtracking and we hit a cell that is gived by the problem we just remove it.
                if (backTracking && currentNode.Initial)
                {
                    variableStack.Pop();
                    continue;
                }

                //Now we give the cell a value from the domain and remove the value from the doamin.
                backTracking = false;
                currentNode.Variable = currentNode.Domain[0];

                //If the cell is given by the Sudoku problem then we don't change its domain.
                if (!currentNode.Initial)
                    currentNode.Domain.RemoveAt(0);

                //We push the next node to the stack and check if the value is valid.
                variableStack.Push(variables[variableStack.Count]);
                if (!checkStateValid(currentNode, variables) && !currentNode.Initial)
                {
                    //If the value is not valid we pop the it from the stack.
                    variableStack.Pop();
                    continue;
                }

            }
            Console.WriteLine("==========================");

            if (solved)
                showSudoku(variables);
            else
                Console.WriteLine("No solution found :/");
            Console.WriteLine();
            //showSudoku(variableStack.ToList());
        }


        //Check if all cells are filled.
        public static bool checkIfSolved(List<Sudoku> variable) => variable.Any(t => t.Variable == 0);

        //Check if the given value to the variable is valid.
        public static bool checkStateValid(Sudoku current, List<Sudoku> variables)
        {
            return current.Neighbors.All(t => t.Variable != current.Variable);

            List<Sudoku> sameRow = variables.Where(t => t.Row == current.Row && t.Col != current.Col).ToList();
            List<Sudoku> sameCol = variables.Where(t => t.Col == current.Col && t.Row != current.Row).ToList();
            List<Sudoku> sameSubMatrix = variables.Where(t => (t.Row / 3 == current.Row / 3 && t.Col / 3 == current.Col / 3)).ToList();
            sameSubMatrix = sameSubMatrix.Where(t => !(t.Row == current.Row && t.Col == current.Col)).ToList();

            for (int i = 0; i < 8; i++)
            {
                if (current.Variable == sameRow[i].Variable || current.Variable == sameCol[i].Variable || current.Variable == sameSubMatrix[i].Variable)
                    return false;
            }


            return true;
        }

        //show the Sudoku by the variables.
        public static void showSudoku(List<Sudoku> variables)
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
                x[variables[i].Row, variables[i].Col] = variables[i].Variable;

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



    public class Sudoku
    {
        public int Variable { get; set; }
        public List<int> Domain { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public bool Initial { get; set; }
        public List<Sudoku> Neighbors { get; set; }


        public Sudoku(int variable, bool initial, int row, int column)
        {
            if (initial)
                Domain = new() { variable };
            else
                Domain = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Variable = variable;
            Row = row;
            Col = column;
            Initial = initial;
        }

    }
}
