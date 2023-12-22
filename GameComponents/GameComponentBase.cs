using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlyeFarm1.GameComponents
{
    internal abstract class GameComponentBase
    {
        public string TextureName;
        public Texture2D Texture;
        public int TileSize = 50;

        public int GridPositionX { get; private set; }
        public int GridPositionY { get; private set; }
        public bool Passable { get; private set; }
        public bool Collidable { get; private set; }
        public bool Active { get; set; }

        public Vector2 VectorCoords
        {
            get
            {
                return new Vector2(GridPositionX * TileSize, GridPositionY * TileSize);
            }
        }

        public GameComponentBase(int x, int y, bool passable, bool collidable)
        {
            Active = true;
            GridPositionX = x;
            GridPositionY = y;
            Passable = passable;
            Collidable = collidable;
        }

        public void Draw(ref SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(Texture, new Rectangle((int)VectorCoords.X, (int)VectorCoords.Y, TileSize, TileSize), Color.White);
        }

        public delegate void CollisionEvent(int x, int y);
        public event CollisionEvent CollisionDetected;
        public virtual void Collision()
        {
            CollisionDetected?.Invoke(GridPositionX, GridPositionY);
        }
    }
}
