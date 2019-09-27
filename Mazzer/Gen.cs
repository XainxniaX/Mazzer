using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazzer
{

    public enum StaticProbabilities
    {
        Up, Right, Down, Left
    }

    public enum Direction
    {
        RIGHT, LEFT, UP, DOWN
    }

    class Gen
    {
        public static readonly Dictionary<StaticProbabilities, float> DEFAULT_STATPROBS = new Dictionary<StaticProbabilities, float>()
        {
            { StaticProbabilities.Up, 0.25f },
            { StaticProbabilities.Right, 0.25f },
            { StaticProbabilities.Down, 0.25f },
            { StaticProbabilities.Left, 0.25f }
        };


        public Random Rng;
        public Dictionary<StaticProbabilities, float> StaticProbs;
        public bool[,] Walls;
        public Stack<Cell> CellStack;

        Cell Current = null;
        
        public Gen(int seed, Dictionary<StaticProbabilities, float> customProbs = null)
        {
            Rng = new Random();
            if (customProbs == null) StaticProbs = DEFAULT_STATPROBS;
            else StaticProbs = customProbs;
            CellStack = new Stack<Cell>();            
        }



        public Cell[,] Generate_DepthFirstBacktrak(int width, int height, int startX, out Point finish)
        {
            finish = new Point();
            Cell[,] ret = new Cell[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    ret[x, y] = new Cell(x, y);
                }
            }

            Current = ret[startX, height - 1];            
            while (true)
            {
                Cell[] ngh = GetUnvisitedNeighbors(Current.X, Current.Y, ret);
                if (ngh != null)
                {
                    int dir = Rng.Next(ngh.Length);
                    CellStack.Push(Current);
                    if (ngh[dir].X < Current.X) ngh[dir].Walls[0] = false;
                    else if (ngh[dir].X > Current.X) Current.Walls[0] = false;
                    if (ngh[dir].Y < Current.Y) ngh[dir].Walls[1] = false;
                    else if (ngh[dir].Y > Current.Y) Current.Walls[1] = false;
                    Current = ngh[dir];
                    Current.Visited = true;
                    if (Current.X == width - 1 || Current.Y == height - 1) finish = new Point(Current.X, Current.Y);
                    
                }
                else if (CellStack.Count > 0)
                {
                    Cell poped = CellStack.Pop();
                    while (CellStack.Count > 0 && GetUnvisitedNeighbors(poped.X, poped.Y, ret) == null)
                    {
                        poped = CellStack.Pop();
                    }
                    if (CellStack.Count == 0) break;
                    Current = poped;
                }
                else break;
            } 

            return ret;
        }

        private Cell[] GetUnvisitedNeighbors(int x, int y, Cell[,] arr)
        {
            List<Cell> ret = new List<Cell>();
            if (x < 0 || y < 0 || x >= arr.GetLength(0) || y >= arr.GetLength(1)) throw new Exception("out of bounds");
            if (x == 0) ret.Add(arr[1, y]);
            else if (x == arr.GetLength(0) - 1) ret.Add(arr[x - 1, y]);
            else
            {
                ret.Add(arr[x + 1, y]);
                ret.Add(arr[x - 1, y]);
            }
            if (y == 0) ret.Add(arr[x, 1]);
            else if (y == arr.GetLength(1) - 1) ret.Add(arr[x, y - 1]);
            else
            {
                ret.Add(arr[x, y - 1]);
                ret.Add(arr[x, y + 1]);
            }
            ret.RemoveAll(item => item.Visited == true);
            if (ret.Count == 0) return null;            
            return ret.ToArray();
        }


    }
}
