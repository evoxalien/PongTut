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

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameRoot : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Ball theBall;
        Paddle paddle1;
        Paddle paddle2;
        Texture2D ballTexture;
        Texture2D paddleTexture;
        const int SCREEN_WIDTH = 640;
        const int SCREEN_HEIGHT = 480;
        SoundEffect hitSound;
        private bool RectsCollide(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
        {
            if (x2 - w2 <= x1 && x1 <= x2 + w2
            && y1 - h2 <= y2 && y2 <= y1 + h1)
                return true;
            else
                return false;
        }

        private bool CollisionWithPaddle()
        {
            if (theBall.motion.X > 0)
            {//Moving Left must hit player2
                return (
                    RectsCollide(paddle2.position.X,
                    paddle2.position.Y,
                    paddle2.Width,
                    paddle2.Height,
                    theBall.position.X,
                    theBall.position.Y,
                    theBall.Width,
                    theBall.Height));
            }
            else
                return (
                    RectsCollide(paddle1.position.X,
                    paddle1.position.Y,
                    paddle1.Width,
                    paddle1.Height,
                    theBall.position.X,
                    theBall.position.Y,
                    theBall.Width,
                    theBall.Height));
        }

        public GameRoot()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any componentsg
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferMultiSampling = false;
            graphics.ApplyChanges();

            //Initializing Ball
            theBall = new Ball();
            theBall.position = new Vector2(30, 30);
            theBall.motion = new Vector2(5f, 2f);
            theBall.Width = 5;
            theBall.Height = 5;

            paddle1 = new Paddle();
            paddle1.position = new Vector2(10, 10);
            paddle1.motion = new Vector2(0, 3);
            paddle1.Width = 4;
            paddle1.Height = 120;

            paddle2 = new Paddle();
            paddle2.position = new Vector2(630, 30);
            paddle2.motion = new Vector2(0, 3);
            paddle2.Width = 4;
            paddle2.Height = 120;


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

            ballTexture = Content.Load<Texture2D>("ball");
            paddleTexture = Content.Load<Texture2D>("bar");

            ContentManager contentManager = new ContentManager(this.Services, @"Content\Sound\");
            hitSound = contentManager.Load<SoundEffect>("Windows Pop-up Blocked");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            theBall.position += theBall.motion;
            if (theBall.position.X <= 0 || theBall.position.X >= SCREEN_WIDTH)
                theBall.motion = new Vector2(-theBall.motion.X, theBall.motion.Y); // reverse X dir
            if (theBall.position.Y <= 0 || theBall.position.Y >= SCREEN_HEIGHT - theBall.Height)
                theBall.motion = new Vector2(theBall.motion.X, -theBall.motion.Y); // reverse X dir
            
            //Paddle1
            paddle1.position += paddle1.motion;
            if (paddle1.position.Y <= 0)
            {
                paddle1.position = new Vector2(paddle1.position.X, 0);
                paddle1.motion = new Vector2(paddle1.motion.X, -paddle1.motion.Y);
            }
            if (paddle1.position.Y >= SCREEN_HEIGHT - paddle1.Height)
            {
                paddle1.position = new Vector2(paddle1.position.X, SCREEN_HEIGHT - paddle1.Height);
                paddle1.motion = new Vector2(paddle1.motion.X, -paddle1.motion.Y);            
            }
            paddle2.position += paddle2.motion;
            if (paddle2.position.Y <= 0)
            {
                paddle2.position = new Vector2(paddle2.position.X, 0);
                paddle2.motion = new Vector2(paddle2.motion.X, -paddle2.motion.Y);
            }

            if (paddle2.position.Y >= SCREEN_HEIGHT - paddle2.Height)
            {
                paddle2.position = new Vector2(paddle2.position.X, SCREEN_HEIGHT - paddle2.Height);
                paddle2.motion = new Vector2(paddle2.motion.X, -paddle2.motion.Y);
            }

            if (theBall.position.Y <= 0 || theBall.position.Y >= SCREEN_HEIGHT - theBall.Height)
            {
                hitSound.Play();
            }

            if (CollisionWithPaddle())
            {
                hitSound.Play();
                theBall.motion = new Vector2(theBall.motion.X * 1.03f, theBall.motion.Y * 1.03f);
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(ballTexture, theBall.rect, Color.White);
            spriteBatch.Draw(paddleTexture, paddle1.rect, Color.Blue);
            spriteBatch.Draw(paddleTexture, paddle2.rect, Color.Red);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
