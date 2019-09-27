using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazzer
{

    public class Cell
    {
        public bool[] Walls;
        public bool Visited;
        public int X;
        public int Y;
        

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            Walls = new bool[2]; //right and bottom
            Walls[0] = true;
            Walls[1] = true;
            Visited = false;
        }

    }

    class Maze
    {
        public static Gen Generator = new Gen(new Random().Next());


        public int Width;
        public int Height;
        public Point Start;
        public Point Finish;
        public Cell[,] Cells;
        
        public Rectangle Bounds;
        public Texture2D CellTex;
        public Texture2D Background;
        

        public Maze(int width, int height, int startX, Rectangle bounds)
        {
            Width = width;
            Height = height;
            Bounds = bounds;
            Start = new Point(startX, height - 1);
            Cells = Generator.Generate_DepthFirstBacktrak(width, height, startX, out Finish);           
        }

        public void Load(ContentManager content)
        {
            CellTex = content.Load<Texture2D>("cellTex");
            Background = content.Load<Texture2D>("background");
        }


        public void Draw(SpriteBatch batch, Point[] route)
        {
            batch.Begin();
            batch.Draw(Background, Bounds, Color.Black);
            Rectangle r;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int cwidth = Bounds.Width / Width;
                    int csmWidth = cwidth - 2;
                    int cheight = Bounds.Height / Height;
                    int csmHeight = cheight - 2;
                    if (!Cells[x, y].Walls[0])
                    {
                        csmWidth += 2;
                    }
                    if (!Cells[x, y].Walls[1])
                    {
                        csmHeight += 2;
                    }
                    r = new Rectangle(Bounds.Left + 1 + x * cwidth, Bounds.Top + 1 + y * cheight, csmWidth, csmHeight);
                    if (Start.X == x && Start.Y == y)
                    {
                        batch.Draw(CellTex, r, Color.Green);
                    }
                    else if (Finish.X == x && Finish.Y == y)
                    {
                        batch.Draw(CellTex, r, Color.Gold);
                    }
                    else
                    {
                        if (route.Contains(new Point(x, y)))
                        {
                            batch.Draw(CellTex, r, Color.Blue);
                        }
                        else
                        {
                            batch.Draw(CellTex, r, Color.White);
                        }                       
                    }
                    
                }

            }
            batch.End();
        }




    }
}
