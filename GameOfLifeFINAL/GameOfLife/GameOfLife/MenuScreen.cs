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
    public class MenuScreen
    {
        private Texture2D Screen;
        private KeyboardState lastKState;
        
        //Strings that will be printed in the screen
        string[] menuOptions = { "1. Random Board", "2. Clear Board", "3. Cool Board", "4. Quit" };

        //constructor for MenuScreen class
        //loads the menu screen image and get the last keyboard state
        public MenuScreen()
        {
            Screen = Game1.Instance.Content.Load<Texture2D>("MenuScreen");
            lastKState = Keyboard.GetState();
        }

        public void Update()
        {
            KeyboardState kState = Keyboard.GetState();

            //position for where the strings will be placed
            int startAt = 200;
            int border = 50;
            for (int i = 0; i < menuOptions.Count(); i++)
            {
                Vector2 textSize = Game1.Instance.font.MeasureString(menuOptions[0]);
                Vector2 pos = new Vector2(50, startAt + ((textSize.Y + border) * i));
            }
            //if one of these keys are pressed then the user will be redirected to screens with respect to their choice
            if (kState.IsKeyDown(Keys.D1) && lastKState.IsKeyUp(Keys.D1))
                Game1.Instance.selectedRandom();

            if (kState.IsKeyDown(Keys.D2) && lastKState.IsKeyUp(Keys.D2))
                Game1.Instance.selectedBoard();

            if (kState.IsKeyDown(Keys.D3) && lastKState.IsKeyUp(Keys.D3))
                Game1.Instance.selectedCool();

            if (kState.IsKeyDown(Keys.D4) && lastKState.IsKeyUp(Keys.D4))
                Game1.Instance.selectedQuit();
        }

        public void Draw()
        {
            //making the menu screen image into the scale of the board size
            float scale = (float)Game1.Instance.boardSize.X / (float)Game1.Instance.boardSize.Y;
            //drawing the image with respect to the scale
            Game1.Instance.spriteBatch.Draw(Screen, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(scale, scale), SpriteEffects.None, 0);
            //drawing the text in the screen
            Vector2 textSize = Game1.Instance.font.MeasureString("Hello");
            int startAt = 200;
            int border = 50;

            for (int i = 0; i < menuOptions.Count(); i++)
            {
                //position for each string goes down by the calculation of the y-axis (startAt+((textSize.Y+border)*i)))
                Vector2 pos = new Vector2(Game1.Instance.Window.ClientBounds.Width/2, startAt + ((textSize.Y + border) * i));
                Game1.Instance.spriteBatch.DrawString(Game1.Instance.font, menuOptions[i], pos, Color.Red);
            }
        }
    }
}

