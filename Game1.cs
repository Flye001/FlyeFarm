using FlyeFarm1.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FlyeFarm1
{
    public class FlyeFarm : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameStateBase gameState;

        public FlyeFarm()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 800
            };
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += ScreenSizeUpdate;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            gameState = new MainMenu(_graphics);
            gameState.Exit += ExitGame;
            gameState.LoadState += LoadNewState;
        }

        private void ScreenSizeUpdate(object sender, EventArgs e)
        {
            _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            AdjustScreenSize();
        }

        private void LoadNewState(GameStateBase newState)
        {
            gameState.Exit -= ExitGame;
            gameState.LoadState -= LoadNewState;
            gameState = newState;
            gameState.Exit += ExitGame;
            gameState.LoadState += LoadNewState;
            gameState.Initialize();
            gameState.LoadContent(Content);
            AdjustScreenSize();
        }

        private void ExitGame()
        {
            Exit();
        }

        private void AdjustScreenSize()
        {
            gameState.AdjustScreenSize();
        }

        protected override void Initialize()
        {
            AdjustScreenSize();
            
            gameState.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            gameState.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            gameState.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            gameState.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    /// <summary>
    /// FPS Class
    /// https://community.monogame.net/t/a-simple-monogame-fps-display-class/10545
    /// </summary>
    internal class SimpleFps
    {
        private double frames = 0;
        private double updates = 0;
        private double elapsed = 0;
        private double last = 0;
        private double now = 0;
        public double msgFrequency = 1.0f;
        public string msg = "";

        /// <summary>
        /// The msgFrequency here is the reporting time to update the message.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            now = gameTime.TotalGameTime.TotalSeconds;
            elapsed = (double)(now - last);
            if (elapsed > msgFrequency)
            {
                int f = (int)(frames / elapsed);
                //msg = " Fps: " + (frames / elapsed).ToString() + "\n Elapsed time: " + elapsed.ToString() + "\n Updates: " + updates.ToString() + "\n Frames: " + frames.ToString();
                //Console.WriteLine(msg);
                msg = "FPS: " + f;
                elapsed = 0;
                frames = 0;
                updates = 0;
                last = now;
            }
            updates++;
        }

        public void DrawFps(SpriteBatch spriteBatch, SpriteFont font, Vector2 fpsDisplayPosition, Color fpsTextColor)
        {
            spriteBatch.DrawString(font, msg, fpsDisplayPosition, fpsTextColor);
            frames++;
        }
    }
}