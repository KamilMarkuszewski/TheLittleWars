using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.Services;
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

        private GameObjectsProviderService GameObjectsProviderService
        {
            get { return ServiceLocator.GetService<GameObjectsProviderService>(); }
        }

        #endregion

        private WeaponDefinitionHolder _weaponDefinitionHolder;
        private WeaponDefinitionHolder WeaponDefinitionHolder
        {
            get { return _weaponDefinitionHolder ?? (_weaponDefinitionHolder = transform.parent.GetComponent<WeaponDefinitionHolder>()); }
        }

        private Sprite _explSprite;

        private void Start()
        {
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

            Destroy(transform.parent.gameObject);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            ExplodeNow(collision.collider.gameObject);
        }

        private void ColideWithMap(GameObject actualCollider)
        {
            DestroyMap(actualCollider);
        }

        private void ColideWithUnit(GameObject actualCollider)
        {
            var moveScript = actualCollider.GetComponent<CharacterMoveScript>();
            GameObjectsProviderService.GameModel.ChangeHp(moveScript.Unit, -WeaponDefinitionHolder.WeaponDefinition.Damage);
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
