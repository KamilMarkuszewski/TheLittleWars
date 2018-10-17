using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public static class UnitPrefabsHelper
    {
        #region Services

        private static ResourcesService ResourcesService
        {
            get { return ServiceLocator.GetService<ResourcesService>(); }
        }

        #endregion

        private static Queue<Transform> _humanUnitPrefabs;
        public static Queue<Transform> HumanUnitPrefabs
        {
            get
            {
                if (_humanUnitPrefabs == null)
                {
                    _humanUnitPrefabs = new Queue<Transform>();
                    ResourcesService.LoadUnitPrefabs(ResourcesPaths.Units).Where(t => t.name.Contains("HumanUnit1")).ToList().ForEach(prefab => _humanUnitPrefabs.Enqueue(prefab));
                }
                return _humanUnitPrefabs;
            }
        }

        private static Queue<Transform> _aiUnitPrefabs;
        public static Queue<Transform> AiUnitPrefabs
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



        public static Transform GetNextFreeUnitPrefab(int playersCount, PlayerType type)
        {
            Queue<Transform> chosenQueue = GetPrefabsCollection(type);

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

        private static Queue<Transform> GetPrefabsCollection(PlayerType type)
        {
            switch (type)
            {
                case PlayerType.LocalPlayer:
                    return HumanUnitPrefabs;
                case PlayerType.RemotePlayer:
                    return HumanUnitPrefabs;
                case PlayerType.Ai:
                    return AiUnitPrefabs;
                default:
                    throw new UnityException("Wrong player type");
            }
        }


        public static string GetPrefabPath(PlayerType type)
        {
            switch (type)
            {
                case PlayerType.LocalPlayer:
                    return GetPrefabPath("HumanUnit1");
                case PlayerType.RemotePlayer:
                    return GetPrefabPath("HumanUnit1");
                case PlayerType.Ai:
                    return GetPrefabPath("AiUnit1");
                default:
                    throw new UnityException("Wrong player type");
            }
        }

        private static string GetPrefabPath(string prefabName)
        {
            return Path.Combine("GameObjects", Path.Combine("Units", Path.Combine(prefabName, prefabName + "Prefab")));
        }
    }
}
