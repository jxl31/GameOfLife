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
    public class Cell
    {

        private bool alive;
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        public Point Position { get; set; }

        public Rectangle boundingBox;

        //Constructor for cell with point as the paremeter which will be used in board to set where the cell will be placed in the board
        //also everytime a cell is created it's bounding box is already created
        public Cell(Point position)
        {
            Position = position;
            boundingBox = new Rectangle((int)Position.X * Game1.cellSize, (int)Position.Y * Game1.cellSize, Game1.cellSize, Game1.cellSize);
            Alive = false;
        }

        public void Update(MouseState mState)
        {
            if (boundingBox.Contains(new Point(mState.X, mState.Y)))
            {
                // Make cells come alive with left-click, or kill them with right-click.
                if (mState.LeftButton == ButtonState.Pressed)
                    Alive = true;
                else if (mState.RightButton == ButtonState.Pressed)
                    Alive = false;
            }
        }

        public void Draw()
        {
            //if cell is alive draw the alive sprite
            if (Alive)
                Game1.Instance.spriteBatch.Draw(Game1.Instance.ACell, boundingBox, Color.Black);
            //if cell is dead draw the dead sprite
            if(!Alive)
                Game1.Instance.spriteBatch.Draw(Game1.Instance.DCell, boundingBox, Color.White);
        }
    }
}
