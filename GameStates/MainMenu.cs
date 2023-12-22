using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FlyeFarm1.GameStates
{
    internal class MainMenu : GameStateBase
    {
        private SpriteFont mainFont;
        
        public MainMenu(GraphicsDeviceManager graphics) : base(graphics)
        {
        }

        public override void AdjustScreenSize()
        {
            //throw new NotImplementedException();
        }

        public override void Initialize()
        {
            //throw new NotImplementedException();
        }

        public override void LoadContent(ContentManager Content)
        {
            mainFont = Content.Load<SpriteFont>("Fonts/Ariel");
        }

        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Escape))
                ExitGame();

            else if (kstate.IsKeyDown(Keys.D1) || kstate.IsKeyDown(Keys.NumPad1))
                LoadNewState(new LevelState(_graphics, "0"));

            else if (kstate.IsKeyDown(Keys.D2) || kstate.IsKeyDown(Keys.NumPad2))
                LoadNewState(new LevelState(_graphics, "1"));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(mainFont, "Welcome to FlyeFarms!", new(10, 0), Color.White);
            spriteBatch.DrawString(mainFont, "Press 1 to play level 1!", new(10, 100), Color.White);
            spriteBatch.DrawString(mainFont, "Press 2 to play level 2!", new(10, 140), Color.White);
            spriteBatch.DrawString(mainFont, "Press escape to quit!", new(10, 180), Color.White);
        }
    }
}
