using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu]
    public class WeaponDefinition : ScriptableObject
    {
        public Sprite sprite;
        public WeaponEnum weaponEnum;
        public bool FriendlyFire;
        public Transform BulletPrefab;
        public int Shoots;
        public Sprite ExplodeSprite;
    }
}
