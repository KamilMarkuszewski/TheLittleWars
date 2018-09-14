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
        public Sprite Sprite;
        public WeaponEnum WeaponEnum;
        public bool FriendlyFire;
        public Transform BulletPrefab;
        public int Shoots;
        public Sprite ExplodeSprite;
        public AudioClip ClipOnColide;
        public AudioClip ClipOnExplode;
        public AudioClip ClipOnTimerCount;
        public AudioClip ClipOnButtonClicked;
        public AudioClip ClipOnShot
            ;
        public int Damage;

        public bool CollideMapExplode;

        public float CollisionDetectionDelay;
    }
}
