using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameOfLife
{
    enum GAMESTATE
    {
        MENU,
        CLEAR,
        RANDOM,
        COOL,
        QUIT
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        private static Game1 instance;
        public static Game1 Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        //general measurements for the playing board
        public Vector2 boardSize;
        public const int cellSize = 5;
        public const int cellnoX = 250; //number of cell that can fit in the x-axis
        public const int cellnoY = 125; //number of cell that can fit in the y-axis

        //speed of each generations
        public const int UpdatePerSecond = 15;
        public const int FramePerSecond = 55;

        public bool Paused = true;

        private KeyboardState kState, lastKState;

        //different type of screens
        private MenuScreen menuScreen;
        private Board board;
        private RandomBoard randomBoard;
        private CoolBoard coolBoard;

        //properties for the sprites
        private Texture2D aCell;
        public Texture2D ACell { get { return aCell; } set { aCell = value; } }
        private Texture2D dCell;
        public Texture2D DCell { get { return dCell; } set { dCell = value; } }

        //main music for the game and font
        Song music;
        public SpriteFont font;

        GAMESTATE gamestate;

        //when the player wants the random board
        //changes gamestate to RANDOM and making other boards null
        public void selectedRandom()
        {
            randomBoard = new RandomBoard();
            gamestate = GAMESTATE.RANDOM;

            menuScreen = null;
            board = null;
            coolBoard = null;
        }//end of selectedRandom()

        //when the player wants the clear board
        //changes gamestate to CLEAR and making other boards null
        public void selectedBoard()
        {
            board = new Board();
            gamestate = GAMESTATE.CLEAR;

            menuScreen = null;
            randomBoard = null;
            coolBoard = null;
        }//end of selectedBoard()

        //when player wants to exit the game
        //makes everything null and changes gamestate to QUIT
        public void selectedQuit()
        {
            gamestate = GAMESTATE.QUIT;
            menuScreen = null;
            randomBoard = null;
            board = null;
            coolBoard = null;
        }//end of selectedQuit

        //when the player wants the cool board
        //changes gamestate to COOL and making other boards null
        public void selectedCool()
        {
            coolBoard = new CoolBoard();
            gamestate = GAMESTATE.COOL;

            menuScreen = null;
            board = null;
            randomBoard = null;

        }//end of selectedCool()

        //when player wants to select a different board or wants to exit
        public void goToMenu()
        {
            menuScreen = new MenuScreen();
            gamestate = GAMESTATE.MENU;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            instance = this;

            //time management
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / FramePerSecond);

            //size of the board with respect to width and height multiplied by the width of the sprite
            boardSize = new Vector2(cellnoX, cellnoY) * cellSize;

            //setting the window screen to be the same size as the board size
            graphics.PreferredBackBufferWidth = (int)boardSize.X;
            graphics.PreferredBackBufferHeight = (int)boardSize.Y;
            graphics.ApplyChanges();

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            kState = lastKState = Keyboard.GetState();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //loading alive and dead sprites
            ACell = Content.Load<Texture2D>("alivecell");
            DCell = Content.Load<Texture2D>("deadcell");

            //loading music and spritefont
            font = Content.Load<SpriteFont>("font");
            music = Content.Load<Song>("backgroundMusic");
            MediaPlayer.IsRepeating = true;

            MediaPlayer.Play(music);

            //when the game is started the gamestate is set to menu which brings the user to the menu screen
            menuScreen = new MenuScreen();
            gamestate = GAMESTATE.MENU;

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            kState = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //switch operator for different gamestates
            /*
             * Menu Screen
             * Clear Board
             * Random Board
             * Cool Board
             * Exit Game
             */
            switch (gamestate)
            {
                case GAMESTATE.MENU:
                    if (menuScreen != null)
                        if (Paused)
                            MediaPlayer.Pause();
                        menuScreen.Update();
                    break;
                case GAMESTATE.CLEAR:
                    if (board != null)
                    {
                        //pressing space will pause and unpause the game
                        if (kState.IsKeyDown(Keys.Space) && lastKState.IsKeyUp(Keys.Space))
                            Paused = !Paused;

                        if (kState.IsKeyDown(Keys.Back) && lastKState.IsKeyUp(Keys.Back))
                            board.Reset();

                        //brings user to menu screen
                        if (kState.IsKeyDown(Keys.Escape) && lastKState.IsKeyUp(Keys.Escape))
                            goToMenu();

                        //Music handler
                        if (Paused)
                            MediaPlayer.Pause();

                        if (!Paused)
                            MediaPlayer.Resume();

                        board.Update(gameTime);
                    }
                    break;
                case GAMESTATE.RANDOM:
                    if (randomBoard != null)
                        if (kState.IsKeyDown(Keys.Space) && lastKState.IsKeyUp(Keys.Space))
                            Paused = !Paused;

                        if (kState.IsKeyDown(Keys.Back) && lastKState.IsKeyUp(Keys.Back))
                            randomBoard.Reset();

                        //brings user to menu screen
                        if (kState.IsKeyDown(Keys.Escape) && lastKState.IsKeyUp(Keys.Escape))
                            goToMenu();
                        
                        //music handler
                        if (Paused)
                            MediaPlayer.Pause();

                        if (!Paused)
                            MediaPlayer.Resume();

                        randomBoard.Update(gameTime);
                    break;
                case GAMESTATE.COOL:
                    if(coolBoard != null)
                        if (kState.IsKeyDown(Keys.Space) && lastKState.IsKeyUp(Keys.Space))
                            Paused = !Paused;

                        if (kState.IsKeyDown(Keys.Back) && lastKState.IsKeyUp(Keys.Back))
                            coolBoard.Reset();
                        
                        //brings users to menu screen
                        if (kState.IsKeyDown(Keys.Escape) && lastKState.IsKeyUp(Keys.Escape))
                            goToMenu();

                        //music handler
                        if (Paused)
                            MediaPlayer.Pause();

                        if (!Paused)
                            MediaPlayer.Resume();

                        coolBoard.Update(gameTime);
                        break;
                case GAMESTATE.QUIT:
                    this.Exit();
                    break;
            }

            lastKState = kState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            switch (gamestate)
            {
                case GAMESTATE.MENU:
                    if (menuScreen != null)
                        menuScreen.Draw();
                    break;
                case GAMESTATE.CLEAR:
                    if (board != null)
                        board.Draw();
                    break;
                case GAMESTATE.RANDOM:
                    if (randomBoard != null)
                        randomBoard.Draw();
                    break;
                case GAMESTATE.COOL:
                    if(coolBoard != null)
                        coolBoard.Draw();
                    break;

            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
