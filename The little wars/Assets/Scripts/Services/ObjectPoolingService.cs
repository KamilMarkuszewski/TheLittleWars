using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Entities;
using Assets.Scripts.Scripts.ExplosionScripts;
using Assets.Scripts.Scripts.Gravitation;
using Assets.Scripts.Services.Interfaces;
using Assets.Scripts.Utility;
using Photon.Pun;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class ObjectPoolingService : IService
    {
        #region Services

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        private ShootService ShootService
        {
            get { return ServiceLocator.GetService<ShootService>(); }
        }

        #endregion

        public ObjectPool<AudioSource> AudioSourcesPool;
        public ObjectPool<GameObject> GrenadesPool;
        public ObjectPool<GameObject> CommetPool;
        public ObjectPool<GameObject> FoxPool;
        public ObjectPool<GameObject> HollyGrenadePool;
        public ObjectPool<GameObject> MinePool;
        public ObjectPool<GameObject> MoralePool;
        public ObjectPool<GameObject> RosePool;
        public ObjectPool<GameObject> SheepPool;
        public ObjectPool<GameObject> SupriseBoxPool;

        public ObjectPool<GameObject> GetWeaponObjectPool(WeaponEnum weaponEnum)
        {
            switch (weaponEnum)
            {
                case WeaponEnum.Grenade:
                    return GrenadesPool;
                case WeaponEnum.HollyGrenade:
                    return HollyGrenadePool;
                case WeaponEnum.Mine:
                    return MinePool;
                case WeaponEnum.Morale:
                    return MoralePool;
                case WeaponEnum.Rose:
                    return RosePool;
                case WeaponEnum.Sheep:
                    return SheepPool;
                case WeaponEnum.SupriseBox:
                    return SupriseBoxPool;
                case WeaponEnum.Commet:
                    return CommetPool;
                case WeaponEnum.Fox:
                    return FoxPool;
                default:
                    throw new UnityException("No object pool for this weapon");
            }
        }

        private AudioSource AudioSourceGenerator()
        {
            var newGameObject = new GameObject("AudioSource", typeof(AudioSource));
            newGameObject.transform.parent = GameObjectsProviderService.AudioSourcesGameObject.transform;
            return newGameObject.GetComponent<AudioSource>();
        }

        private GameObject GrenadeGenerator()
        {
            return ShootService.BulletGenerator(WeaponEnum.Grenade);
        }

        private GameObject CommetGenerator()
        {
            return ShootService.BulletGenerator(WeaponEnum.Commet);
        }

        private GameObject FoxGenerator()
        {
            return ShootService.BulletGenerator(WeaponEnum.Fox);
        }

        private GameObject HollyGrenadeGenerator()
        {
            return ShootService.BulletGenerator(WeaponEnum.HollyGrenade);
        }

        private GameObject MineGenerator()
        {
            return ShootService.BulletGenerator(WeaponEnum.Mine);
        }

        private GameObject MoraleGenerator()
        {
            return ShootService.BulletGenerator(WeaponEnum.Morale);
        }

        private GameObject RoseGenerator()
        {
            return ShootService.BulletGenerator(WeaponEnum.Rose);
        }

        private GameObject SheepGenerator()
        {
            return ShootService.BulletGenerator(WeaponEnum.Sheep);
        }

        private GameObject SupriseBoxGenerator()
        {
            return ShootService.BulletGenerator(WeaponEnum.SupriseBox);
        }


        private void AudioSourceDisabler(AudioSource obj)
        {

        }

        private void AudioSourceEnabler(AudioSource obj)
        {

        }

        private void GameObjectDisabler(GameObject obj)
        {
            obj.SetActive(false);
        }

        private void GameObjectEnabler(GameObject obj)
        {
            obj.SetActive(true);
        }

        private void WeaponDisabler(GameObject obj)
        {
            var rb = obj.GetComponent<Rigidbody2D>();
            obj.transform.position = Vector3.zero;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0.0f;
            var explodeOnColideScript = obj.GetComponent<ExplodeOnColideScript>();
            explodeOnColideScript.Enabled = false;
            obj.GetComponent<GravityBodyScript>().Enabled = false;
            explodeOnColideScript.Reset();
            GameObjectDisabler(obj);
        }

        private void WeapinEnabler(GameObject obj)
        {
            GameObjectEnabler(obj);
        }


        #region IService

        public void Initialize()
        {
            Status = ServiceStatus.Initializing;

            AudioSourcesPool = new ObjectPool<AudioSource>(AudioSourceGenerator, AudioSourceEnabler, AudioSourceDisabler, 5);

            GrenadesPool = new ObjectPool<GameObject>(GrenadeGenerator, WeapinEnabler, WeaponDisabler, 10);
            //CommetPool = new ObjectPool<GameObject>(CommetGenerator, WeapinEnabler, WeaponDisabler, 10);
            //FoxPool = new ObjectPool<GameObject>(FoxGenerator, WeapinEnabler, WeaponDisabler, 10);
            //HollyGrenadePool = new ObjectPool<GameObject>(HollyGrenadeGenerator, WeapinEnabler, WeaponDisabler, 10);
            //MinePool = new ObjectPool<GameObject>(MineGenerator, WeapinEnabler, WeaponDisabler, 10);
            //MoralePool = new ObjectPool<GameObject>(MoraleGenerator, WeapinEnabler, WeaponDisabler, 10);
            //RosePool = new ObjectPool<GameObject>(RoseGenerator, WeapinEnabler, WeaponDisabler, 10);
            //SheepPool = new ObjectPool<GameObject>(SheepGenerator, WeapinEnabler, WeaponDisabler, 10);
            //SupriseBoxPool = new ObjectPool<GameObject>(SupriseBoxGenerator, WeapinEnabler, WeaponDisabler, 10);

            Status = ServiceStatus.Ready;
        }

        public ServiceStatus Status
        {
            get; private set;
        }

        #endregion
    }
}
