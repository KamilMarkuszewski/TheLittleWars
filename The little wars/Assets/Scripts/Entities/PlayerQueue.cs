using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Scripts;
using UnityEngine.UI;

namespace Assets.Scripts.Entities
{
    public class PlayerQueue
    {
        public readonly Player Player;
        public readonly Queue<UnitModelScript> UnitsQueue = new Queue<UnitModelScript>();

        public PlayerQueue(Player player)
        {
            Player = player;
            player.Units.ForEach(u => UnitsQueue.Enqueue(u));
        }
    }

}
