using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_DylanCassell
{
    internal class EnemyStats : DrawableGameComponent
    {
        public int HealthPoints { get; set; }

        public EnemyStats(Game game, int healthPoints) : base(game)
        {
            this.HealthPoints = healthPoints;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
