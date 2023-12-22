using System;

namespace FlyeFarm1.GameComponents
{
    internal class Coin : GameComponentBase
    {
        public Coin(int x, int y) : base(x, y, true, true)
        {
            TextureName = "Items/Coin";
        }

        public override void Collision()
        {
            Active = false;
            base.Collision();
        }
    }
}
