using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlyeFarm1.GameComponents
{
    internal class Player
    {
        public Texture2D Texture;
        public int TileSize = 50;

        public int Health = 25;

        private int gridPosX;
        public int GridPositionX
        {
            get { return gridPosX; }
            set
            {
                if (value < 0 || value > 9) return;
                gridPosX = value;
            }
        }

        private int gridPosY;
        public int GridPositionY
        {
            get { return gridPosY; }
            set
            {
                if (value < 0 || value > 9) return;
                gridPosY = value;
            }
        }

        public Vector2 VectorPlayerCoords
        {
            get
            {
                return new Vector2(GridPositionX * TileSize, GridPositionY * TileSize);
            }
        }

        public Player(int x, int y)
        {
            GridPositionX = x;
            GridPositionY = y;
        }

        public bool HandleInput(KeyboardState kstate, GameComponentBase[,] Grid)
        {
            bool moved = false;

            if (kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.W))
            {
                if (GridPositionY == 0) return false;
                var gridItem = Grid[GridPositionX, GridPositionY - 1];
                if (gridItem != null && gridItem.Collidable)
                    gridItem.Collision();
                if (gridItem != null && !gridItem.Passable)
                    return true;
                GridPositionY -= 1;
                moved = true;
            }

            if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S))
            {
                if (GridPositionY == 9) return false;
                var gridItem = Grid[GridPositionX, GridPositionY + 1];
                if (gridItem != null && gridItem.Collidable)
                    gridItem.Collision();
                if (gridItem != null && !gridItem.Passable)
                    return true;
                GridPositionY += 1;
                moved = true;
            }

            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A))
            {
                if (GridPositionX == 0) return false;
                var gridItem = Grid[GridPositionX -1, GridPositionY];
                if (gridItem != null && gridItem.Collidable)
                    gridItem.Collision();
                if (gridItem != null && !gridItem.Passable)
                    return true;
                GridPositionX -= 1;
                moved = true;
            }

            if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D))
            {
                if (GridPositionX == 9) return false;
                var gridItem = Grid[GridPositionX + 1, GridPositionY];
                if (gridItem != null && gridItem.Collidable)
                    gridItem.Collision();
                if (gridItem != null && !gridItem.Passable)
                    return true;
                
                GridPositionX += 1;
                moved = true;
            }

            return moved;
        }

        public void Draw(ref SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)VectorPlayerCoords.X, (int)VectorPlayerCoords.Y, TileSize, TileSize), Color.White);
        }
    }
}
