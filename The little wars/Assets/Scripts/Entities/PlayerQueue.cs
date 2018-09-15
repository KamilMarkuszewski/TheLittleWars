using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Entities
{
    public class PlayerQueue
    {
        public readonly Player Player;
        public readonly Queue<Unit> UnitsQueue = new Queue<Unit>();

        public PlayerQueue(Player player)
        {
            Player = player;
            player.Units.ForEach(u => UnitsQueue.Enqueue(u));
        }
    }

}
