using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_DylanCassell
{
    public class PlayerStats : GameComponent
    {
        public string PlayerName { get; set; }
        public int HealthPoints { get; set; }
        public int MagicPoints { get; set; }

        public PlayerStats(Game game, string playerName, int healthPoints, int magicPoints) : base(game)
        {
            this.PlayerName = playerName;
            this.HealthPoints = healthPoints;
            this.MagicPoints = magicPoints;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
