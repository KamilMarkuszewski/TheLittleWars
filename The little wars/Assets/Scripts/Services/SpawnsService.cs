using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.ExtensionMethods;
using Assets.Scripts.Services.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class SpawnsService : IService
    {

        public Vector3 GetNextSpawn(List<Vector3> spawns)
        {
            if (spawns.Count > 0)
            {
                Vector3 spawnPosition = spawns[0];
                spawns.RemoveAt(0);

                return spawnPosition;
            }
            else
            {
                throw new UnityException("Not enought spawns");
            }
        }

        public List<Vector3> GetSpawns()
        {
            var spawns = new List<GameObject>(GameObject.FindGameObjectsWithTag(Tags.Spawn));
            return spawns.Select(sp => sp.transform.position).ToList().Shuffle();
        }

        #region IService

        public void Initialize()
        {
            Status = ServiceStatus.Initializing;

            Status = ServiceStatus.Ready;
        }

        public ServiceStatus Status
        {
            get; private set;
        }

        #endregion
    }
}
