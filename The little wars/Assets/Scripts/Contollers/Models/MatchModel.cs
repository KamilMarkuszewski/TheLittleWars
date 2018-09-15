using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Entities;
using Assets.Scripts.Services;

namespace Assets.Scripts.Contollers.Models
{
    public class MatchModel
    {
        #region Services

        private SpawnsService SpawnsService
        {
            get { return ServiceLocator.GetService<SpawnsService>(); }
        }

        #endregion

        public List<Player> Players = new List<Player>();
        public readonly Queue<PlayerQueue> PlayersQueue = new Queue<PlayerQueue>();
        public PlayerQueue CurrentPlayerQueue;
        public Unit CurrentUnit;


        public MatchModel(PlayerInitValues[] playerInitValues)
        {
            var prefabs = new UnitPrefabsContainer();
            var spawns = SpawnsService.GetSpawns();
            foreach (var initValues in playerInitValues)
            {
                var p = new Player(initValues.Color, initValues.UnitsCount, spawns, prefabs.GetNextFreeUnitPrefab(playerInitValues.Length));
                Players.Add(p);
                PlayersQueue.Enqueue(new PlayerQueue(p));
            }
        }



    }
}
