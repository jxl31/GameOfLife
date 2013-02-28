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
    public class Board
    {
        public Point Size { get; private set; }

        //initializing 2d array of cell and bool data type
        private Cell[,] cell;
        private bool[,] cellState;

        // needed help to update board all at once
        private TimeSpan timer;

        //index
        private int i, j;
        private KeyboardState lastKState;

        public Board()
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

            //setting clock to 0
            timer = TimeSpan.Zero;
        }//end of Board()

        public void Reset()
        {
            // if the player wants to start a new board set everything to false
            for (i = 0; i < Size.X; i++)
                for (j = 0; j < Size.Y; j++)
                    cellState[i, j] = false;

            nextState();
        }//end of Reset()

        public void Update(GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();
            KeyboardState kState = Keyboard.GetState();

            //going through each cell updating if the cell has been pressed or not
            foreach (Cell cells in cell)
                cells.Update(mState);

            //if paused then nothing happens
            if (Game1.Instance.Paused)
                return;

            //resets board
            if (kState.IsKeyDown(Keys.Back) && lastKState.IsKeyUp(Keys.Back))
                Reset();

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
