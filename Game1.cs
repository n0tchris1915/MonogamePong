
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace maybepong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Declare textures
        Texture2D paddle;
        Texture2D ball;
        Texture2D three;
        Texture2D two;
        Texture2D one;
        Texture2D go;

        // Menu textures
        Texture2D start;
        Texture2D startHover;
        Texture2D onePlayer;
        Texture2D onePlayerHover;
        Texture2D twoPlayer;
        Texture2D twoPlayerHover;
        Texture2D quit;
        Texture2D quitHover;      

        // Declare positions
        Vector2 ballPos;
        Vector2 paddle1;
        Vector2 paddle2;
        Vector2 numbersPos;

        // Menu positions
        Vector2 startPos;
        Vector2 playerPos;
        Vector2 quitPos;

        Boolean playing;
        Boolean counting;
        Boolean showBall;
        Boolean players;
        Boolean clicked = false;
        Boolean hasReleased = true;

        Vector2 ballSpd;

        // Timer variables
        int counter = 0;
        private readonly float countDuration = 1f;
        float currentTime = 0f;

        Random rnd = new Random();
        int spdX;
        int spdY;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            paddle1 = new Vector2(20, graphics.GraphicsDevice.Viewport.Height / 2 - 50);
            paddle2 = new Vector2(graphics.GraphicsDevice.Viewport.Width - 35, graphics.GraphicsDevice.Viewport.Height / 2 - 50);
            ballPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 5, graphics.GraphicsDevice.Viewport.Height / 2 - 5);
            numbersPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 50, graphics.GraphicsDevice.Viewport.Height / 2 - 50);

            // Menu position initialization
            startPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 50, graphics.GraphicsDevice.Viewport.Height / 2 - 100);
            playerPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 50, graphics.GraphicsDevice.Viewport.Height / 2);
            quitPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 50, graphics.GraphicsDevice.Viewport.Height / 2 + 100);


            spdX = rnd.Next(-5, 6);
            spdY = rnd.Next(-5, 6);

            while (spdX == 0)
                spdX = rnd.Next(-5, 6);

            ballSpd = new Vector2(spdX, spdY);

            playing = false;
            counting = true;
            showBall = false;
            players = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            paddle = this.Content.Load<Texture2D>("paddle");
            ball = this.Content.Load<Texture2D>("ball");
            start = this.Content.Load<Texture2D>("start");
            three = this.Content.Load<Texture2D>("three");
            two = this.Content.Load<Texture2D>("two");
            one = this.Content.Load<Texture2D>("one");
            go = this.Content.Load<Texture2D>("go");

            // Load menu textures
            startHover = this.Content.Load<Texture2D>("startHover");
            onePlayer = this.Content.Load<Texture2D>("onePlayer");
            onePlayerHover = this.Content.Load<Texture2D>("onePlayerHover");
            twoPlayer = this.Content.Load<Texture2D>("twoPlayer");
            twoPlayerHover = this.Content.Load<Texture2D>("twoPlayerHover");
            quit = this.Content.Load<Texture2D>("quit");
            quitHover = this.Content.Load<Texture2D>("quitHover");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            IsMouseVisible = true;
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.W))
                paddle1.Y -= 5;
            if (state.IsKeyDown(Keys.S))
                paddle1.Y += 5;

            if (state.IsKeyDown(Keys.Up))
                paddle2.Y -= 5;
            if (state.IsKeyDown(Keys.Down))
                paddle2.Y += 5;

            if (playing && counting)
            {
                currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (currentTime >= countDuration)
                {
                    counter++;
                    currentTime -= countDuration;
                }
            }

            if (showBall)
            {
                ballPos.X += ballSpd.X;
                ballPos.Y += ballSpd.Y;

                paddle2.Y = ballPos.Y;
            }

            BallCollision();

            base.Update(gameTime);
        }
      
        void BallCollision()
        {
            // Paddle1 collision
            if (ballPos.X < paddle1.X + 15 && ballPos.X > paddle1.X && ballPos.Y + 10 > paddle1.Y && ballPos.Y < paddle1.Y + 100)
            {
                if (ballPos.Y > paddle1.Y && ballPos.Y < paddle1.Y + 33 || ballPos.Y > paddle1.Y + 67 && ballPos.Y < paddle1.Y + 100)
                {
                    if (ballSpd.Y > 0 && ballSpd.Y < 6)
                    {
                        ballSpd.Y++;
                        if (ballSpd.X > 0 && ballSpd.X < 6)
                            ballSpd.X++;
                        if (ballSpd.X < 0 && ballSpd.X > -6)
                            ballSpd.X--;
                    }
                    if (ballSpd.Y < 0 && ballSpd.Y > -6)
                    {
                        ballSpd.Y--;
                        if (ballSpd.X > 0 && ballSpd.X < 6)
                            ballSpd.X++;
                        if (ballSpd.X < 0 && ballSpd.X > -6)
                            ballSpd.X--;
                    }
                }
                if (ballPos.Y > paddle1.Y + 34 && ballPos.Y < paddle1.Y + 66)
                {
                    if (ballSpd.Y > 0 && ballSpd.Y < 6)
                    {
                        ballSpd.Y--;
                        if (ballSpd.X > 0 && ballSpd.X < 6)
                            ballSpd.X--;
                        if (ballSpd.X < 0 && ballSpd.X > -6)
                            ballSpd.X++;
                    }
                    if (ballSpd.Y < 0 && ballSpd.Y > -6)
                    {
                        ballSpd.Y++;
                        if (ballSpd.X > 0 && ballSpd.X < 6)
                            ballSpd.X--;
                        if (ballSpd.X < 0 && ballSpd.X > -6)
                            ballSpd.X++;
                    }
                }
                ballSpd.X = -ballSpd.X;
            }

            // Paddle2 collision
            if (ballPos.X + 10 > paddle2.X && ballPos.X + 10 < paddle2.X + 15 && ballPos.Y + 10 > paddle2.Y && ballPos.Y < paddle2.Y + 100)
                ballSpd.X = -ballSpd.X;

            // Top collision
            if (ballPos.Y < 0)
                ballSpd.Y = -ballSpd.Y;

            // Bottom collision
            if (ballPos.Y + 10 > graphics.GraphicsDevice.Viewport.Height)
                ballSpd.Y = -ballSpd.Y;

            // Left collision
            if (ballPos.X + 10 < 0)
            {
                ballPos.X = graphics.GraphicsDevice.Viewport.Width / 2 - 5;
                ballPos.Y = graphics.GraphicsDevice.Viewport.Height / 2 - 5;
            }

            // Right collision
            if (ballPos.X > graphics.GraphicsDevice.Viewport.Width)
            {
                ballPos.X = graphics.GraphicsDevice.Viewport.Width / 2 - 5;
                ballPos.Y = graphics.GraphicsDevice.Viewport.Height / 2 - 5;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            if (!playing)
                DrawMenu();

            if (playing)
                DrawGame();

            if (counter == 1)
                ShowThree();
            if (counter == 2)
                ShowTwo();
            if (counter == 3)
                ShowOne();
            if (counter == 4)
                ShowGo();
            if (counter == 5)
                showBall = true;
            if (counter == 6)
                counting = false;

            base.Draw(gameTime);
        }

        void DrawMenu()
        {
            MouseState mouse = Mouse.GetState();
            MouseState lastMouseState;
            Boolean clickOccured = false;

            mouse = Mouse.GetState();

            spriteBatch.Begin();

            spriteBatch.Draw(start, startPos, Color.White);
            if (!players)
                spriteBatch.Draw(onePlayer, playerPos, Color.White);

            if (players)
                spriteBatch.Draw(twoPlayer, playerPos, Color.White);

            spriteBatch.Draw(quit, quitPos, Color.White);

            // Draw start hover button
            if (mouse.X > startPos.X && mouse.X < startPos.X + 100 && mouse.Y > startPos.Y && mouse.Y < startPos.Y + 50)
            {
                spriteBatch.Draw(startHover, startPos, Color.White);
                if (mouse.LeftButton == ButtonState.Pressed)
                    playing = true;
            }

            // Draw one player hover button
            if (mouse.X > playerPos.X && mouse.X < playerPos.X + 100 && mouse.Y > playerPos.Y && mouse.Y < playerPos.Y + 50 && !players)
            {
                lastMouseState = mouse;
                spriteBatch.Draw(onePlayerHover, playerPos, Color.White);
                if (lastMouseState.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed && !clickOccured)
                {
                    clickOccured = true;
                    players = true;
                }
            }
            clickOccured = false;

            // Draw two player hover button
            if (mouse.X > playerPos.X && mouse.X < playerPos.X + 100 && mouse.Y > playerPos.Y && mouse.Y < playerPos.Y + 50 && players)
            {
                lastMouseState = mouse;
                spriteBatch.Draw(twoPlayerHover, playerPos, Color.White);
                if (lastMouseState.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed && !clickOccured)
                    clickOccured = false;
                    players = false;
            }
            clickOccured = true;

            // Draw wuit hover
            if (mouse.X > quitPos.X && mouse.X < quitPos.X + 100 && mouse.Y > quitPos.Y && mouse.Y < quitPos.Y + 50)
            {
                spriteBatch.Draw(quitHover, quitPos, Color.White);
                if (mouse.LeftButton == ButtonState.Pressed)
                    Exit();
            }

            spriteBatch.End();
        }

        void ShowThree()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(three, numbersPos, Color.White);
            spriteBatch.End();
        }

        void ShowTwo()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(two, numbersPos, Color.White);
            spriteBatch.End();
        }

        void ShowOne()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(one, numbersPos, Color.White);
            spriteBatch.End();
        }

        void ShowGo()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(go, numbersPos, Color.White);
            spriteBatch.End();
        }

        void DrawGame()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(paddle, paddle1, Color.White);
            spriteBatch.Draw(paddle, paddle2, Color.White);
            if (showBall)
                spriteBatch.Draw(ball, ballPos, Color.White);

            spriteBatch.End();
        }
    }
}
