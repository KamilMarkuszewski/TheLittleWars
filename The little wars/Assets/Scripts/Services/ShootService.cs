using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Entities;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Scripts;
using Assets.Scripts.Scripts.ExplosionScripts;
using Assets.Scripts.Scripts.Gravitation;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.Utility;
using Photon.Pun;
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

        private ObjectPoolingService ObjectPoolingService
        {
            get { return ServiceLocator.GetService<ObjectPoolingService>(); }
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

        public GameObject BulletGenerator(WeaponEnum enu)
        {
            var definition = LoadedWeapons.First(d => d.WeaponEnum == enu);
            var bullet = UnityEngine.Object.Instantiate(definition.BulletPrefab, Vector3.zero, Quaternion.identity,
                GameObjectsProviderService.BulletsParentObject.transform);
            int viewId = UniqueIdHelper.GetNextForWeapon(enu);
            bullet.GetComponent<PhotonView>().ViewID = viewId;
            return bullet.gameObject;
        }

        public WeaponDefinition GetNoneWeapon()
        {
            return LoadedWeapons.First(w => w.WeaponEnum == WeaponEnum.None);
        }

        public void Shoot(WeaponEnum weaponEnum, Vector3 position, Vector3 direction, int power)
        {
            Vector3 spawnPos = direction + position;
            var definition = LoadedWeapons.FirstOrDefault(w => w.WeaponEnum == weaponEnum);
            if (definition != null)
            {
                if (definition.Shoots > 0)
                {
                    var bullet = ObjectPoolingService.GrenadesPool.GetObject();
                    bullet.SetActive(true);
                    bullet.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                    bullet.GetComponent<BulletMovementScript>().OwnerInitialize(spawnPos);
                    SoundService.PlayClip(definition.ClipOnShot);
                    CameraService.SetCameraFollowTarget(bullet.transform);
                    var rb = bullet.GetComponent<Rigidbody2D>();
                    rb.AddForce(direction * power * 100);
                    
                    if (!PhotonNetwork.OfflineMode)
                    {
                        GameObjectsProviderService.MainPhotonView.RPC("RPC_WeaponShoot", RpcTarget.Others, weaponEnum, spawnPos);
                    }
                    else
                    {
                        SoundService.PlayClip(definition.ClipOnShot);
                        CameraService.SetCameraFollowTarget(bullet.transform);
                    }

                    CheckIfShouldEndRound();
                }
                if (definition.Shoots < 0)
                {
                    SoundService.PlayClip(definition.ClipOnShot);
                }
            }
        }

        public void WeaponShoot(WeaponEnum weaponEnum, Vector3 position)
        {
            Debug.Log("WeaponShoot");
            var pool = ObjectPoolingService.GetWeaponObjectPool(weaponEnum);
            var bullet = pool.GetObject();
            bullet.SetActive(true);
            bullet.GetComponent<BulletMovementScript>().Initialize(position);

            var definition = LoadedWeapons.FirstOrDefault(w => w.WeaponEnum == weaponEnum);
            if (definition != null)
            {
                SoundService.PlayClip(definition.ClipOnShot);
                CameraService.SetCameraFollowTarget(bullet.transform);
            }
            
            CheckIfShouldEndRound();
        }

        private void CheckIfShouldEndRound()
        {
            if (PhotonHelper.PlayerIsMultiplayerHost() || PhotonHelper.PlayerIsSinglePlayer())
            {
                if (ShouldRoundEnd(GameObjectsProviderService.CurrentWeaponController.GetCurrentWeapon()))
                {
                    GameObjectsProviderService.MainPhotonView.RPC("RPC_SetTimeTo3Sec", RpcTarget.Others);
                    GameObjectsProviderService.MainGameController.SetTimeTo3Sec();
                }
            }
        }

        private static string GetPrefabPath(string prefabName)
        {
            return Path.Combine("GameObjects", Path.Combine("Weapons", Path.Combine(prefabName, prefabName + "Prefab")));
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
