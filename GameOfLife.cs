using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    class Game
    {
        private int GridL { get; set; }
        private int GridH { get; set; }
        private bool[,] Grid;
        private bool[,] Buffer;
        private bool perFound = false;
        List<bool[,]> permuations = new List<bool[,]>();

        public bool Check4Copy()
        {
            return perFound;
        }
        public bool[,] GetCurrentState()
        {
            return this.Buffer;
        }
        public Game (bool[,] grid,int x,int y)
        {
            this.Grid = new bool[x, y];
            this.Buffer = new bool[x, y];
            Array.Copy(grid, this.Grid, grid.Length);
            Array.Copy(grid, this.Buffer, grid.Length);
            GridL = x;
            GridH = y;
        }
        public void StartRound()
        {           
            // run trough every cell
            for (int x = 0; x < GridL; x++)
            {
                for (int y = 0; y < GridH; y++)
                {
                    ExecuteEvent(CountCells(x, y),x,y);
                }
            }
           // check for permutations
            for (int c = 0; c < permuations.Count; c++)
            {
                bool isSame = true;
                for (int x = 0; x < GridL; x++)
                {
                    for (int y = 0; y < GridH; y++)
                    {
                        if (permuations.ElementAt(c)[x, y] != Buffer[x, y])
                        {
                            isSame = false;
                            break;                        
                        }
                    }
                }
                if(isSame)
                {
                    perFound = true;
                }
            }                         
            permuations.Add(Grid);
            Array.Copy(Buffer, Grid, Grid.Length);
        }
        private int CountCells(int pl,int ph)
        {
            // prevent IndexOutOfRangeError
            int n = 0;
            int l = pl;
            int h = ph;
            int k1 = 2;// for 3x3
            int k2 = 2;
            if (l == 0)
            {
                l++;
                k1--;
            }
            else if (l == GridL - 1)
            {
                l--;
                k1--;
            }
            if (h == 0)
            {
                h++;
                k2--;
            }
            else if (h == GridH - 1)
            {
                h--;
                k2--;
            }
            for (int x = l-1; x <l+k1; x++)
            {
                for (int y = h-1; y < h+k2; y++)
                {
                    if (Grid[x, y] )
                        n++;
                }
            }
            return n-Convert.ToInt16(Grid[pl,ph]);
        }
        private void ExecuteEvent(int numberOfCells,int x,int y)
        {
            // Program logic
            if (numberOfCells > 3 || numberOfCells < 2)
                Buffer[x, y] = false;
            else if (!this.Grid[x, y]  && numberOfCells ==3)
                Buffer[x, y] = true;            
        } 
    }
}
