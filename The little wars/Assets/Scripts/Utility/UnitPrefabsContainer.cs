using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Contollers
{
    public class UnitPrefabsContainer
    {
        #region Services

        private ResourcesService ResourcesService
        {
            get { return ServiceLocator.GetService<ResourcesService>(); }
        }

        #endregion

        private Queue<Transform> _unitPrefabs;
        public Queue<Transform> UnitPrefabs
        {
            get
            {
                if (_unitPrefabs == null)
                {
                    _unitPrefabs = new Queue<Transform>();
                    ResourcesService.LoadUnitPrefabs(ResourcesPaths.Units).ForEach(prefab => _unitPrefabs.Enqueue(prefab));
                }
                return _unitPrefabs;
            }
        }

        public Transform GetNextFreeUnitPrefab(int playersCount)
        {
            if (UnitPrefabs.Count >= playersCount)
            {
                if (_unitPrefabs.Any())
                {
                    return _unitPrefabs.Dequeue();
                }
                else
                {
                    throw new UnityException("Not enought unit prefabs in folder" + ResourcesPaths.Units);
                }
            }
            else if (UnitPrefabs.Any())
            {
                return UnitPrefabs.First();
            }
            else
            {
                throw new UnityException("No unit prefabs in folder " + ResourcesPaths.Units);
            }
        }
        
    }
}
