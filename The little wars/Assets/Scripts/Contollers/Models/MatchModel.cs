using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Entities;
using Assets.Scripts.Services;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Contollers.Models
{
    public class MatchModel
    {
        public List<Player> Players = new List<Player>();
        public readonly Queue<PlayerQueue> PlayersQueue = new Queue<PlayerQueue>();
        public PlayerQueue CurrentPlayerQueue;
        public Unit CurrentUnit;
    }
}
