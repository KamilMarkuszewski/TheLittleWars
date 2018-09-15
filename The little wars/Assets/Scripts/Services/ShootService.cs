using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Services.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class ShootService : IService
    {
        #region Services

        private SoundService SoundService
        {
            get { return ServiceLocator.GetService<SoundService>(); }
        }

        private CameraService CameraService
        {
            get { return ServiceLocator.GetService<CameraService>(); }
        }

        private ResourcesService ResourcesService
        {
            get { return ServiceLocator.GetService<ResourcesService>(); }
        }

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        #endregion

        private List<WeaponDefinition> _loadedWeapons;
        public List<WeaponDefinition> LoadedWeapons
        {
            get
            {
                if (_loadedWeapons == null)
                {
                    _loadedWeapons = ResourcesService.LoadScriptableObjects<WeaponDefinition>(ResourcesPaths.Weapons);
                }
                return _loadedWeapons;
            }
        }

        public WeaponDefinition GetNoneWeapon()
        {
            return LoadedWeapons.First(w => w.WeaponEnum == WeaponEnum.None);
        }

        public void Shoot(WeaponEnum weaponEnum, Vector3 position, Vector3 direction, int power)
        {
            var definition = LoadedWeapons.FirstOrDefault(w => w.WeaponEnum == weaponEnum);
            if (definition != null)
            {
                if (definition.Shoots > 0)
                {
                    SoundService.PlayClip(definition.ClipOnShot);
                    var bullet = UnityEngine.Object.Instantiate(definition.BulletPrefab, position + direction, Quaternion.identity);
                    bullet.parent = GameObjectsProviderService.BulletsParentObject.transform;
                    var rb = bullet.GetComponent<Rigidbody2D>();
                    rb.AddForce(direction * power * 100);
                    CameraService.SetCameraFollowTarget(bullet);
                }
                if (definition.Shoots < 0)
                {
                    SoundService.PlayClip(definition.ClipOnShot);
                }
            }
        }

        public bool ShouldRoundEnd(WeaponEnum weaponEnum)
        {
            var definition = LoadedWeapons.FirstOrDefault(w => w.WeaponEnum == weaponEnum);
            if (definition != null)
            {
                return definition.Shoots == 1 || definition.Shoots < 1;
            }
            return false;
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
