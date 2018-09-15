using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public List<Unit> Units = new List<Unit>();

        public Player(Color color, int unitsAmount, List<Vector3> spawns, Transform characterPrefab)
        {
            Color = color;

            for (int i = 0; i < unitsAmount; i++)
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



    public class PlayerInitValues
    {
        public Color Color;
        public int UnitsCount;

        public PlayerInitValues(Color color, int unitsCount)
        {
            Color = color;
            UnitsCount = unitsCount;
        }
    }
}
