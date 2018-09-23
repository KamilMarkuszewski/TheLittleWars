using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class UnitPrefabsContainer
    {
        #region Services

        private ResourcesService ResourcesService
        {
            get { return ServiceLocator.GetService<ResourcesService>(); }
        }

        #endregion

        private Queue<Transform> _humanUnitPrefabs;
        public Queue<Transform> HumanUnitPrefabs
        {
            get
            {
                if (_humanUnitPrefabs == null)
                {
                    _humanUnitPrefabs = new Queue<Transform>();
                    ResourcesService.LoadUnitPrefabs(ResourcesPaths.Units).Where(t => t.name.Contains("HumanUnit")).ToList().ForEach(prefab => _humanUnitPrefabs.Enqueue(prefab));
                }
                return _humanUnitPrefabs;
            }
        }

        private Queue<Transform> _aiUnitPrefabs;
        public Queue<Transform> AiUnitPrefabs
        {
            get
            {
                if (_aiUnitPrefabs == null)
                {
                    _aiUnitPrefabs = new Queue<Transform>();
                    ResourcesService.LoadUnitPrefabs(ResourcesPaths.Units).Where(t => t.name.Contains("AiUnit")).ToList().ForEach(prefab => _aiUnitPrefabs.Enqueue(prefab));
                }
                return _aiUnitPrefabs;
            }
        }



        public Transform GetNextFreeUnitPrefab(int playersCount, PlayerType type)
        {
            Queue<Transform> chosenQueue = type == PlayerType.Human ? HumanUnitPrefabs : AiUnitPrefabs;

            if (chosenQueue.Count >= playersCount)
            {
                if (chosenQueue.Any())
                {
                    return chosenQueue.Dequeue();
                }
                else
                {
                    throw new UnityException("Not enought unit prefabs in folder" + ResourcesPaths.Units);
                }
            }
            else if (chosenQueue.Any())
            {
                return chosenQueue.First();
            }
            else
            {
                throw new UnityException("No unit prefabs in folder " + ResourcesPaths.Units);
            }
        }

    }
}
