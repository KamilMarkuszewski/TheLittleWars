using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Helpers;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts
{
    public class ShootHelper
    {
        private static List<WeaponDefinition> _loadedWeapons;
        public static List<WeaponDefinition> LoadedWeapons
        {
            get
            {
                if (_loadedWeapons == null)
                {
                    _loadedWeapons = ResourcesHelper.LoadScriptableObjects<WeaponDefinition>(ResourcesPaths.Weapons);
                }
                return _loadedWeapons;
            }
        }

        public static WeaponDefinition GetNoneWeapon()
        {
            return LoadedWeapons.First(w => w.weaponEnum == WeaponEnum.None);
        }

        private static Sprite GetWeaponSprite(WeaponEnum weapon)
        {
            return LoadedWeapons.First(w => w.weaponEnum == weapon).sprite;
        }

        public static void Shoot(WeaponEnum weaponEnum, Vector3 position, Vector3 direction, int power)
        {
            var definition = LoadedWeapons.FirstOrDefault(w => w.weaponEnum == weaponEnum);
            if (definition != null)
            {
                if (definition.Shoots > 0)
                {
                    var bullet = UnityEngine.Object.Instantiate(definition.BulletPrefab, position + direction, Quaternion.identity);
                    var rb = bullet.GetComponent<Rigidbody2D>();
                    rb.AddForce(direction * power * 100);
                    CameraHelper.SetCameraFollowTarget(bullet);
                }
            }
        }

        public static bool ShouldEndRound(WeaponEnum weaponEnum)
        {
            var definition = LoadedWeapons.FirstOrDefault(w => w.weaponEnum == weaponEnum);
            if (definition != null)
            {
                return definition.Shoots == 1 || definition.Shoots < 1;
            }
            return false;
        }

    }
}
