using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.ExtensionMethods;
using UnityEngine;

namespace Assets.Scripts
{
    public class SpawnsHelper
    {

        public static Vector3 GetNextSpawn(List<Vector3> spawns)
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

        public static List<Vector3> GetSpawns()
        {
            var spawns = new List<GameObject>(GameObject.FindGameObjectsWithTag("Spawn"));
            return spawns.Select(sp => sp.transform.position).ToList().Shuffle();
        }


    }
}
