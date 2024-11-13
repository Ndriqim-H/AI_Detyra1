using System;
using System.Drawing;

class LatinSquare
{
    public List<State> State { get; set; }
    public int Size { get; set; }

    public LatinSquare()
    {

    }


}

public class State
{
    public int[,] Node { get; set; }
    public int Depth { get; set; }

    public State(int size, int depth)
    {
        if (depth == 0)
        {
            Node = new int[size, size];
        }
        else
        {

        }
    }

    public int[] GeneratePermutation(int size)
    {

    }


    // Generate the next states
    public List<State> GetChildren()
    {
        List<State> states = [];
        for (int i = 0; i < Node.Length; i++)
        {
            // Get first row
            if (Node[0, i] == 0)
            {
                for (int j = 1; j <= Node.Length; j++)
                {
                    State newState = new(Node.Length, Depth + 1)
                    {
                        Node = (int[,])Node.Clone()
                    };
                    newState.Node[0, i] = j;
                    states.Add(newState);
                }
            }
        }
        return states;
    }

    // Check if the state's node is valid
    // The node is valid if the number of the node is not repeated in the row and column
    public bool ValidateState()
    {
        for (int i = 0; i < Node.Length; i++)
        {
            for (int j = 0; j < Node.Length; j++)
            {
                if (Node[i, j] != 0)
                {
                    for (int k = 0; k < Node.Length; k++)
                    {
                        if (k != j && Node[i, j] == Node[i, k])
                        {
                            return false;
                        }
                        if (k != i && Node[i, j] == Node[k, j])
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    // Return false if the node is empty
                    return false;
                }
            }
        }
        return true;
    }



    // Check if the state is a final state

    public bool FinalState()
    {
        for (int i = 0; i < Node.Length; i++)
        {
            for (int j = 0; j < Node.Length; j++)
            {
                if (Node[i, j] == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
class Program
{
    static void Main()
    {
        //Console.Write("Jepni madhësinë e katrorit latin (n): ");
        //int size = int.Parse(Console.ReadLine());
        //GenerateLatinSquare(size);
    }
}
