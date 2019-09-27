using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazzer
{

    class Node
    {
        public bool[] Walls;
        public Point Position;

        public int G;
        public int H;
        public int F;

        public Node Parent;

        public Node(bool[] walls, int x, int y, Node parent = null)
        {
            Walls = walls;
            Position = new Point(x, y);
            G = int.MaxValue;
            H = int.MaxValue;
            F = int.MaxValue;
            Parent = parent;
        }



    }

    class AStar
    {
        Maze Maze;
        Node[,] Grid;

        public AStar(Maze maze)
        {
            Maze = maze;
            Grid = new Node[Maze.Width, Maze.Height];
            for (int y = 0; y < Maze.Height; y++)
            {
                for (int x = 0; x < Maze.Width; x++)
                {
                    Grid[x, y] = new Node(Maze.Cells[x, y].Walls, x, y);
                }
            }
        }


        public Node[] CalculateRoute()
        {
            List<Node> Open = new List<Node>();
            List<Node> Closed = new List<Node>();

            Open.Add(Grid[Maze.Start.X, Maze.Start.Y]);
            Node current = Open[0];
            current.G = 0;
            current.H = Calc_H(current);
            current.F = current.G + current.H;
            while (true)
            {
                current = Open.Any() ? Open[0] : null;
                foreach (var n in Open)
                {
                    if (n.F < current.F) current = n; 
                }

                Open.Remove(current);
                Closed.Add(current);

                if (current.Position == Maze.Finish)
                {
                    return BacktrakRoute();
                }

                foreach (Node ngh in GetNeighbors(current.Position.X, current.Position.Y))
                {
                    if (Closed.Contains(ngh)) continue;
                    if (ngh.Position.X > current.Position.X) { if (current.Walls[0]) continue; }
                    else if (ngh.Position.X < current.Position.X) { if (ngh.Walls[0]) continue; }
                    if (ngh.Position.Y > current.Position.Y) { if (current.Walls[1]) continue; }
                    else if (ngh.Position.Y < current.Position.Y) { if (ngh.Walls[1]) continue; }

                    if (ngh.G < int.MaxValue && ngh.G > current.G + 10 || !Open.Contains(ngh))
                    {
                        ngh.G = Calc_G(ngh, current);
                        ngh.H = Calc_H(ngh);
                        ngh.F = ngh.G + ngh.H;
                        ngh.Parent = current;
                        if (!Open.Contains(ngh))
                        {
                            Open.Add(ngh);
                        }
                    }                    
                }



            }
        }

        public Node[] GetNeighbors(int x, int y)
        {
            List<Node> ret = new List<Node>();
            if (x < 0 || y < 0 || x >= Maze.Width || y >= Maze.Height) throw new Exception("out of range");
            if (x == 0) ret.Add(Grid[1, y]);            
            else if (x == Maze.Width - 1) ret.Add(Grid[x - 1, y]);             
            else
            {
                ret.Add(Grid[x - 1, y]);
                ret.Add(Grid[x + 1, y]);
            }
            if (y == 0) ret.Add(Grid[x, y + 1]);
            else if (y == Maze.Height - 1) ret.Add(Grid[x, y - 1]);
            else
            {
                ret.Add(Grid[x, y + 1]);
                ret.Add(Grid[x, y - 1]);
            }

            return ret.ToArray();
        }

        public int Calc_G(Node n, Node current)
        {
            return current.G + 10;
        }

        public int Calc_H(Node n)
        {
            return (int)Math.Round(10 * Math.Sqrt((Maze.Finish.X - n.Position.X) * (Maze.Finish.X - n.Position.X) + (Maze.Finish.Y - n.Position.Y) * (Maze.Finish.Y - n.Position.Y)));
        }

        

        public Node[] BacktrakRoute()
        {
            List<Node> ret = new List<Node>();
            Node cur = Grid[Maze.Finish.X, Maze.Finish.Y];
            ret.Add(cur);
            while (true)
            {
                if (cur.Parent == null) break;
                ret.Add(cur.Parent);
                cur = cur.Parent;               
            }
            ret.Reverse();
            return ret.ToArray();
        }

    }
}
