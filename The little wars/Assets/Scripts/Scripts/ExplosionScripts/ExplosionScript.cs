using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Services;
using Photon.Pun;
using UnityEngine;

namespace Assets.Scripts.Scripts.ExplosionScripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ExplosionScript : MonoBehaviour
    {
        #region Services

        private SoundService SoundService
        {
            get { return ServiceLocator.GetService<SoundService>(); }
        }

        private ObjectPoolingService ObjectPoolingService
        {
            get { return ServiceLocator.GetService<ObjectPoolingService>(); }
        }

        #endregion

        private WeaponDefinitionHolder _weaponDefinitionHolder;
        private WeaponDefinitionHolder WeaponDefinitionHolder
        {
            get { return _weaponDefinitionHolder ?? (_weaponDefinitionHolder = transform.parent.GetComponent<WeaponDefinitionHolder>()); }
        }

        private Sprite _explSprite;
        private bool _exploded = false;

        public void Initialize()
        {
            _exploded = false;
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                _explSprite = sr.sprite;
            }
        }

        public void ExplodeNow(GameObject actualCollider)
        {
            SoundService.PlayClip(WeaponDefinitionHolder.WeaponDefinition.ClipOnExplode);
            if (actualCollider != null)
            {
                switch (actualCollider.tag)
                {
                    case Tags.Map:
                        ColideWithMap(actualCollider);
                        break;

                    case Tags.Unit:
                        ColideWithUnit(actualCollider);
                        break;

                    case Tags.Bullet:
                        ColideWithBullet(actualCollider);
                        break;
                }
            }

            Invoke("DisableObject", 0.1f);

        }

        private void DisableObject()
        {
            ObjectPoolingService.GetWeaponObjectPool(WeaponDefinitionHolder.WeaponDefinition.WeaponEnum)
                .PutObject(transform.parent.gameObject);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_exploded)
            {
                ExplodeNow(collision.collider.gameObject);
                _exploded = true;
            }
        }

        private void ColideWithMap(GameObject actualCollider)
        {
            DestroyMap(actualCollider);
        }

        private void ColideWithUnit(GameObject actualCollider)
        {
            var unitModelScript = actualCollider.GetComponent<UnitModelScript>();
            unitModelScript.ChangeHp(-WeaponDefinitionHolder.WeaponDefinition.Damage);
        }

        private void ColideWithBullet(GameObject actualCollider)
        {

        }

        private void DestroyMap(GameObject map)
        {
            var mapDestroyScript = map.GetComponent<MapDestroyScript>();
            if (mapDestroyScript != null)
            {
                Vector2 explosionCenter = new Vector2(transform.position.x, transform.position.y);
                mapDestroyScript.MergeWithMainTexture(explosionCenter, _explSprite);
            }
        }
    }
}
