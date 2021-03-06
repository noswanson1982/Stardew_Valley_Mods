using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame.SSCEnemies
{
    public class KillZone
    {
        private Vector2 position;
        public Rectangle hitBox;

        /// <summary>
        /// Sets the position of the kill zone as well as the position of the kill zone hit box.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
                this.hitBox.X = (int)this.position.X;
                this.hitBox.Y = (int)this.position.Y;
            }
        }

        public KillZone()
        {

        }

        public KillZone(Vector2 Position,Rectangle HitBox)
        {
            this.hitBox = HitBox;
            this.Position = Position;
        }

        public void onCollision(SSCProjectiles.SSCProjectile projectile)
        {
            //Do nothing???
        }
    }
}
