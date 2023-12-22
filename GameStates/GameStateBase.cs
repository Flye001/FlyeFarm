using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyeFarm1.GameStates
{
    internal abstract class GameStateBase
    {
        public GraphicsDeviceManager _graphics;
        public abstract void AdjustScreenSize();
        public abstract void Initialize();
        public abstract void LoadContent(ContentManager Content);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public delegate void ExitGameEvent();
        public event ExitGameEvent Exit;
        public void ExitGame()
        {
            Exit?.Invoke();
        }

        public delegate void LoadStateEvent(GameStateBase newState);
        public event LoadStateEvent LoadState;
        public void LoadNewState(GameStateBase newState)
        {
            LoadState?.Invoke(newState);
        }

        public GameStateBase(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
        }
}
}
