using FlyeFarm1.GameComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;

namespace FlyeFarm1.GameStates
{
    internal class LevelState : GameStateBase
    {
        const int inputDelay = 200;
        DateTime inputCooldown;
        Texture2D grassTexture;
        SoundEffect grunt;
        SoundEffect coinSfx;
        SpriteFont font;
        private int tileSize;
        private string levelName;

        SimpleFps fps = new();

        Player Player;
        GameComponentBase[,] Grid = new GameComponentBase[10, 10];
        List<GameComponentBase> RenderList = new();
        private int score = 0;

        public LevelState(GraphicsDeviceManager graphics, string levelName) : base(graphics)
        {
            this.levelName = levelName;
        }

        public override void AdjustScreenSize()
        {
            if (_graphics.PreferredBackBufferWidth > _graphics.PreferredBackBufferHeight)
                tileSize = _graphics.PreferredBackBufferHeight / 10;
            else
                tileSize = _graphics.PreferredBackBufferWidth / 10;

            foreach (var i in Grid)
            {
                if (i != null)
                    i.TileSize = tileSize;
            }
            Player.TileSize = tileSize;
        }

        private void CactusColission(int x, int y)
        {
            grunt.Play();
            Player.Health -= 5;
            if (Player.Health <= 0)
                LoadNewState(new MainMenu(_graphics));
            //ExitGame();
        }

        private void CoinColission(int x, int y)
        {
            coinSfx.Play();
            score += 1;
            Grid[x, y] = null;
        }

        public override void Initialize()
        {
            // Setup Grid
            var map = File.ReadLines($"Maps/{levelName}.txt").ToList();
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    char icon = map[y][x];
                    if (icon == 't')
                    {
                        Grid[x, y] = new Tree(x, y);
                    }
                    else if (icon == 's')
                    {
                        Grid[x, y] = new Coin(x, y);
                        Grid[x, y].CollisionDetected += CoinColission;
                    }
                    else if (icon == 'c')
                    {
                        Grid[x, y] = new Cactus(x, y);
                        Grid[x, y].CollisionDetected += CactusColission;
                    }
                    else if (icon == 'p')
                    {
                        Player = new(x, y);
                    }
                }
            }

            inputCooldown = DateTime.Now;
        }

        public override void LoadContent(ContentManager Content)
        {
            Player.Texture = Content.Load<Texture2D>("Player/PlayerStill");
            grassTexture = Content.Load<Texture2D>("Environment/Grass");
            grunt = Content.Load<SoundEffect>("Audio/Grunt");
            coinSfx = Content.Load<SoundEffect>("Audio/Coin");
            font = Content.Load<SpriteFont>("Fonts/Ariel");

            foreach (var obj in Grid)
            {
                if (obj != null)
                {
                    obj.Texture = Content.Load<Texture2D>(obj.TextureName);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            fps.Update(gameTime);
            var kstate = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kstate.IsKeyDown(Keys.Escape))
                LoadNewState(new MainMenu(_graphics));

            if (DateTime.Now < inputCooldown)
                return;

            var moved = Player.HandleInput(kstate, Grid);
            if (moved)
                inputCooldown = DateTime.Now.AddMilliseconds(inputDelay);

            RenderList = new();
            foreach (var obj in Grid)
            {
                if (obj != null && obj.Active)
                {
                    RenderList.Add(obj);
                }
            }
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(grassTexture, new Rectangle(0, 0, tileSize * 10, tileSize * 10), Color.White);

            foreach (var obj in RenderList)
            {
                obj.Draw(ref _spriteBatch);
            }

            Player.Draw(ref _spriteBatch);

            _spriteBatch.DrawString(font, "Score: " + score, new(10, 10), Color.White);
            _spriteBatch.DrawString(font, "Health: " + Player.Health, new(10, 50), Color.White);
            fps.DrawFps(_spriteBatch, font, new(10, 90), Color.White);
        }

    }
}
