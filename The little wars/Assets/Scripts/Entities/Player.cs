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

        public Player(Color color, int unitsAmount, List<Vector3> spawns, Transform characterPrefab, Transform charactersParentObject)
        {
            Color = color;

            for (int i = 0; i < unitsAmount; i++)
            {
                var unit = new Unit();
                Units.Add(unit);

                unit.Instantiate(characterPrefab, charactersParentObject, SpawnsService.GetNextSpawn(spawns), Color);
            }
        }
    }

}
