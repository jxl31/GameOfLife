using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameOfLife
{
    public class CoolBoard
    {
        public Point Size { get; private set; }

        private Cell[,] cell;
        private bool[,] cellState;

        // needed help to update board all at once
        private TimeSpan timer;

        private int i, j;
        private KeyboardState lastKState;

        public CoolBoard()
        {
            // size of the 2d array playing board
            Size = new Point(Game1.cellnoX, Game1.cellnoY);

            lastKState = Keyboard.GetState();

            // setting it up like cell[400,200] and cellState[bool[400],bool[200]];
            cell = new Cell[Size.X, Size.Y];
            cellState = new bool[Size.X, Size.Y];

            //filling up the playing board with cell in place
            //initializing every cell state to be false by default
            for (i = 0; i < Size.X; i++)
            {
                for (j = 0; j < Size.Y; j++)
                {
                    cell[i, j] = new Cell(new Point(i, j));
                    cellState[i, j] = false;
                }
            }

            //Gun
            cell[5, 10].Alive = true; cell[5, 11].Alive = true; cell[6, 10].Alive = true; cell[6, 11].Alive = true;
            cell[16, 10].Alive = true; cell[16, 9].Alive = true; cell[17, 8].Alive = true; cell[18, 7].Alive = true;
            cell[19, 8].Alive = true; cell[20, 9].Alive = true; cell[21, 9].Alive = true; cell[20, 10].Alive = true;
            cell[21, 10].Alive = true; cell[20, 11].Alive = true; cell[21, 11].Alive = true; cell[19, 12].Alive = true;
            cell[18, 13].Alive = true; cell[17, 12].Alive = true; cell[16, 11].Alive = true;
            cell[26, 9].Alive = true; cell[26, 8].Alive = true; cell[26, 7].Alive = true; cell[27, 7].Alive = true;
            cell[27, 6].Alive = true; cell[28, 7].Alive = true; cell[28, 6].Alive = true; cell[29, 7].Alive = true;
            cell[29, 6].Alive = true; cell[30, 6].Alive = true; cell[30, 5].Alive = true; cell[29, 8].Alive = true;
            cell[29, 9].Alive = true; cell[29, 10].Alive = true; cell[30, 10].Alive = true; cell[30, 11].Alive = true;
            cell[28, 9].Alive = true; cell[28, 10].Alive = true; cell[27, 9].Alive = true; cell[27, 10].Alive = true;
            cell[35, 7].Alive = true; cell[35, 6].Alive = true;
            cell[39, 9].Alive = true; cell[40, 9].Alive = true; cell[39, 8].Alive = true; cell[40, 8].Alive = true;

            //diamond thingy
            //left propeler
            cell[70, 25].Alive = true; cell[70, 26].Alive = true; cell[71, 24].Alive = true; cell[71, 27].Alive = true; cell[72, 25].Alive = true; cell[72, 26].Alive = true; cell[72, 27].Alive = true;
            //top propeler
            cell[78, 19].Alive = true; cell[79, 19].Alive = true; cell[80, 19].Alive = true; cell[78, 18].Alive = true; cell[81, 18].Alive = true; cell[79, 17].Alive = true; cell[80, 17].Alive = true;
            //right propeler
            cell[86, 25].Alive = true; cell[86, 26].Alive = true; cell[86, 27].Alive = true; cell[87, 25].Alive = true; cell[87, 28].Alive = true; cell[88, 26].Alive = true; cell[88, 27].Alive = true;
            //bottom propeler
            cell[78, 33].Alive = true; cell[79, 33].Alive = true; cell[80, 33].Alive = true; cell[77, 34].Alive = true; cell[80, 34].Alive = true; cell[78, 35].Alive = true; cell[79, 35].Alive = true;
            //box
            cell[74, 23].Alive = true; cell[74, 24].Alive = true; cell[74, 25].Alive = true; cell[74, 26].Alive = true; cell[74, 27].Alive = true; cell[74, 28].Alive = true; cell[74, 29].Alive = true; cell[75, 30].Alive = true;
            cell[76, 31].Alive = true; cell[77, 31].Alive = true; cell[78, 31].Alive = true; cell[79, 31].Alive = true; cell[80, 31].Alive = true; cell[81, 31].Alive = true; cell[82, 31].Alive = true; cell[83, 30].Alive = true;
            cell[84, 29].Alive = true; cell[84, 28].Alive = true; cell[84, 27].Alive = true; cell[84, 26].Alive = true; cell[84, 25].Alive = true; cell[84, 24].Alive = true; cell[84, 23].Alive = true; cell[83, 22].Alive = true;
            cell[82, 21].Alive = true; cell[81, 21].Alive = true; cell[80, 21].Alive = true; cell[79, 21].Alive = true; cell[78, 21].Alive = true; cell[77, 21].Alive = true; cell[76, 21].Alive = true; cell[75, 22].Alive = true;
            cell[76, 23].Alive = true; cell[77, 23].Alive = true; cell[76, 24].Alive = true;
            cell[76, 28].Alive = true; cell[76, 29].Alive = true; cell[77, 29].Alive = true;
            cell[81, 29].Alive = true; cell[82, 29].Alive = true; cell[82, 28].Alive = true;
            cell[82, 24].Alive = true; cell[82, 23].Alive = true; cell[81, 23].Alive = true;

            //cool shape
            cell[100, 50].Alive = true; cell[99, 51].Alive = true; cell[101, 51].Alive = true; cell[99, 52].Alive = true; cell[101, 52].Alive = true;
            cell[99, 53].Alive = true; cell[101, 53].Alive = true; cell[100, 54].Alive = true; cell[100, 51].Alive = true; cell[100, 53].Alive = true;

            //mouse
            cell[175, 30].Alive = true; cell[174, 31].Alive = true; cell[176, 31].Alive = true; cell[173, 32].Alive = true; cell[177, 32].Alive = true;
            cell[175, 32].Alive = true; cell[175, 33].Alive = true; cell[175, 34].Alive = true; cell[175, 35].Alive = true;

            //something cool again  
            for (i = 75; i < 84; i++)
                cell[150, i + 1].Alive = true;

            ////cool stuff again
            for (i = 75; i < 83; i++)
                cell[165, i + 1].Alive = true;
            
            //rabbit-ish
            for (i = 50; i < 93; i++)
                cell[215, i + 1].Alive = true;
            cell[214, 93].Alive = true; cell[216, 93].Alive = true; cell[213, 93].Alive = true; cell[217, 93].Alive = true;

            //setting clock to 0
            timer = TimeSpan.Zero;
        }//end of Board()

        public void Reset()
        {
            for (i = 0; i < Size.X; i++)
            {
                for (j = 0; j < Size.Y; j++)
                {
                    cell[i, j] = new Cell(new Point(i, j));
                    cellState[i, j] = false;
                }
            }

            //Gun
            cell[5, 10].Alive = true; cell[5, 11].Alive = true; cell[6, 10].Alive = true; cell[6, 11].Alive = true;
            cell[16, 10].Alive = true; cell[16, 9].Alive = true; cell[17, 8].Alive = true; cell[18, 7].Alive = true;
            cell[19, 8].Alive = true; cell[20, 9].Alive = true; cell[21, 9].Alive = true; cell[20, 10].Alive = true;
            cell[21, 10].Alive = true; cell[20, 11].Alive = true; cell[21, 11].Alive = true; cell[19, 12].Alive = true;
            cell[18, 13].Alive = true; cell[17, 12].Alive = true; cell[16, 11].Alive = true;
            cell[26, 9].Alive = true; cell[26, 8].Alive = true; cell[26, 7].Alive = true; cell[27, 7].Alive = true;
            cell[27, 6].Alive = true; cell[28, 7].Alive = true; cell[28, 6].Alive = true; cell[29, 7].Alive = true;
            cell[29, 6].Alive = true; cell[30, 6].Alive = true; cell[30, 5].Alive = true; cell[29, 8].Alive = true;
            cell[29, 9].Alive = true; cell[29, 10].Alive = true; cell[30, 10].Alive = true; cell[30, 11].Alive = true;
            cell[28, 9].Alive = true; cell[28, 10].Alive = true; cell[27, 9].Alive = true; cell[27, 10].Alive = true;
            cell[35, 7].Alive = true; cell[35, 6].Alive = true;
            cell[39, 9].Alive = true; cell[40, 9].Alive = true; cell[39, 8].Alive = true; cell[40, 8].Alive = true;

            //diamond thingy
            //left propeler
            cell[70, 25].Alive = true; cell[70, 26].Alive = true; cell[71, 24].Alive = true; cell[71, 27].Alive = true; cell[72, 25].Alive = true; cell[72, 26].Alive = true; cell[72, 27].Alive = true;
            //top propeler
            cell[78, 19].Alive = true; cell[79, 19].Alive = true; cell[80, 19].Alive = true; cell[78, 18].Alive = true; cell[81, 18].Alive = true; cell[79, 17].Alive = true; cell[80, 17].Alive = true;
            //right propeler
            cell[86, 25].Alive = true; cell[86, 26].Alive = true; cell[86, 27].Alive = true; cell[87, 25].Alive = true; cell[87, 28].Alive = true; cell[88, 26].Alive = true; cell[88, 27].Alive = true;
            //bottom propeler
            cell[78, 33].Alive = true; cell[79, 33].Alive = true; cell[80, 33].Alive = true; cell[77, 34].Alive = true; cell[80, 34].Alive = true; cell[78, 35].Alive = true; cell[79, 35].Alive = true;
            //box
            cell[74, 23].Alive = true; cell[74, 24].Alive = true; cell[74, 25].Alive = true; cell[74, 26].Alive = true; cell[74, 27].Alive = true; cell[74, 28].Alive = true; cell[74, 29].Alive = true; cell[75, 30].Alive = true;
            cell[76, 31].Alive = true; cell[77, 31].Alive = true; cell[78, 31].Alive = true; cell[79, 31].Alive = true; cell[80, 31].Alive = true; cell[81, 31].Alive = true; cell[82, 31].Alive = true; cell[83, 30].Alive = true;
            cell[84, 29].Alive = true; cell[84, 28].Alive = true; cell[84, 27].Alive = true; cell[84, 26].Alive = true; cell[84, 25].Alive = true; cell[84, 24].Alive = true; cell[84, 23].Alive = true; cell[83, 22].Alive = true;
            cell[82, 21].Alive = true; cell[81, 21].Alive = true; cell[80, 21].Alive = true; cell[79, 21].Alive = true; cell[78, 21].Alive = true; cell[77, 21].Alive = true; cell[76, 21].Alive = true; cell[75, 22].Alive = true;
            cell[76, 23].Alive = true; cell[77, 23].Alive = true; cell[76, 24].Alive = true;
            cell[76, 28].Alive = true; cell[76, 29].Alive = true; cell[77, 29].Alive = true;
            cell[81, 29].Alive = true; cell[82, 29].Alive = true; cell[82, 28].Alive = true;
            cell[82, 24].Alive = true; cell[82, 23].Alive = true; cell[81, 23].Alive = true;

            //cool shape
            cell[100, 50].Alive = true; cell[99, 51].Alive = true; cell[101, 51].Alive = true; cell[99, 52].Alive = true; cell[101, 52].Alive = true;
            cell[99, 53].Alive = true; cell[101, 53].Alive = true; cell[100, 54].Alive = true; cell[100, 51].Alive = true; cell[100, 53].Alive = true;

            //mouse
            cell[175, 30].Alive = true; cell[174, 31].Alive = true; cell[176, 31].Alive = true; cell[173, 32].Alive = true; cell[177, 32].Alive = true;
            cell[175, 32].Alive = true; cell[175, 33].Alive = true; cell[175, 34].Alive = true; cell[175, 35].Alive = true;

            //something cool again  
            for (i = 75; i < 84; i++)
                cell[150, i + 1].Alive = true;

            ////cool stuff again
            for (i = 75; i < 83; i++)
                cell[165, i + 1].Alive = true;

            //crab-ish
            for (i = 50; i < 95; i++)
                cell[215, i + 1].Alive = true;
            cell[214, 95].Alive = true; cell[216, 95].Alive = true; cell[213, 95].Alive = true; cell[217, 95].Alive = true;

            for (i = 0; i < Size.X; i++)
            {
                for (j = 0; j < Size.Y; j++)
                {
                    if (cell[i, j].Alive)
                    {
                        cellState[i, j] = true;
                    }
                }
            }
            
            nextState();
        }//end of Reset()

        public void Update(GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();
            KeyboardState kState = Keyboard.GetState();

            foreach (Cell cells in cell)
                cells.Update(mState);

            //if paused then nothing happens
            if (Game1.Instance.Paused)
                return;

            timer += gameTime.ElapsedGameTime;

            if (timer.TotalMilliseconds > 1000 / Game1.UpdatePerSecond)
            {
                timer = TimeSpan.Zero;

                //checking every cell's state in the board
                for (i = 0; i < Size.X; i++)
                {
                    for (j = 0; j < Size.Y; j++)
                    {
                        bool isAlive = cell[i, j].Alive;
                        //get the count of living cells around the current cell
                        int count = getCount(i, j);
                        bool nextGeneration = false;

                        //if amount of cells alive around the checked cell is less than 2 meaning underpopulation
                        //set nextGeneration to false
                        if (isAlive && count < 2)
                            nextGeneration = false;

                        //if amount of cells alive around the checked cell is 2 or 3 meaning it can survive
                        //set nextGeneration to true
                        if (isAlive && (count == 2 || count == 3))
                            nextGeneration = true;

                        //if amount of cells alive around the checked cell is more than 3 meaning overpopulation
                        //set nextGeneration to false
                        if (isAlive && count > 3)
                            nextGeneration = false;

                        //if the checked cell is dead but have exactly 3 alive cells around it then it
                        //survives to nextGeneration cause of reproduction
                        //set nextGeneration to true
                        if (!isAlive && count == 3)
                            nextGeneration = true;

                        cellState[i, j] = nextGeneration;
                    }//end for(j)
                }//end for(i)

                lastKState = kState;
                nextState();
            }//end of if
        }//end of update()

        public int getCount(int x, int y)
        {
            int count = 0;

            //check top of cell
            if (y != 0)
                if (cell[x, y - 1].Alive)
                    count++;
            //check top-right of cell
            if (y != 0 && x != Size.X - 1)
                if (cell[x + 1, y - 1].Alive)
                    count++;
            //check right of cell
            if (x != Size.X - 1)
                if (cell[x + 1, y].Alive)
                    count++;
            //check bottom-right of cell
            if (x != Size.X - 1 && y != Size.Y - 1)
                if (cell[x + 1, y + 1].Alive)
                    count++;
            //check bottom of cell
            if (y != Size.Y - 1)
                if (cell[x, y + 1].Alive)
                    count++;
            //check bottom-left of cell
            if (x != 0 && y != Size.Y - 1)
                if (cell[x - 1, y + 1].Alive)
                    count++;
            //check left of cell
            if (x != 0)
                if (cell[x - 1, y].Alive)
                    count++;
            //check top-left of cell
            if (x != 0 && y != 0)
                if (cell[x - 1, y - 1].Alive)
                    count++;

            return count;
        }

        //declare wether cell is alive or not for next frame
        public void nextState()
        {
            for (i = 0; i < Size.X; i++)
                for (j = 0; j < Size.Y; j++)
                    cell[i, j].Alive = cellState[i, j];
        }

        public void Draw()
        {
            foreach (Cell cells in cell)
                cells.Draw();
        }
    }
}
