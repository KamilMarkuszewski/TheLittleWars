using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Player
    {
        #region Services

        private SpawnsService SpawnsService
        {
            get { return ServiceLocator.GetService<SpawnsService>(); }
        }

        #endregion

        public Color Color;
        public int Team;
        public PlayerType PlayerType;
        public List<Unit> Units = new List<Unit>();

        public Player(PlayerCreationEntity playerCreationEntity, List<Vector3> spawns, Transform characterPrefab)
        {
            Team = playerCreationEntity.Team;
            PlayerType = playerCreationEntity.PlayerType;
            Color = playerCreationEntity.Color;

            for (int i = 0; i < playerCreationEntity.UnitsNumber; i++)
            {
                var unit = new Unit();
                Units.Add(unit);

                unit.Instantiate(characterPrefab, SpawnsService.GetNextSpawn(spawns), Color);
            }
        }

        public bool HasAliveUnits()
        {
            return Units.Any(u => u.IsAlive());
        }
    }
}
