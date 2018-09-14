using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.ExtensionMethods;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.Scripts.ExplosionScripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(WeaponDefinitionHolder))]
    public class RunExplodeTimerOnColide : MonoBehaviour
    {
        #region Services

        private SoundService SoundService
        {
            get { return ServiceLocator.GetService<SoundService>(); }
        }

        #endregion

        #region Fields and Properties

        private bool _hasCollided = false;

        public int Seconds = 3;

        private float _collisionDetectionStart;

        private Transform ExplosionObject
        {
            get
            {
                foreach (Transform child in transform)
                {
                    if (child.name == "explosion")
                    {
                        return child;
                    }
                }
                throw new UnityException("Object " + gameObject.name + " missing child " + "explosion");
            }
        }

        private WeaponDefinitionHolder _weaponDefinitionHolder;
        private WeaponDefinitionHolder WeaponDefinitionHolder
        {
            get { return _weaponDefinitionHolder ?? (_weaponDefinitionHolder = GetComponent<WeaponDefinitionHolder>()); }
        }

        #endregion

        private void Start()
        {
            _collisionDetectionStart = Time.time + WeaponDefinitionHolder.WeaponDefinition.CollisionDetectionDelay;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            Collide(collision.collider.gameObject);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            Collide(col.gameObject);
        }

        private void Collide(GameObject actualCollider)
        {
            if (actualCollider.tag.In(Tags.Map, Tags.Unit, Tags.Bullet) && _collisionDetectionStart < Time.time)
            {
                if (!_hasCollided)
                {
                    StartCoroutine(TimerCourutine());
                    _hasCollided = true;
                }
            }
        }

        private IEnumerator TimerCourutine()
        {
            for (var i = 0; i < Seconds; i++)
            {
                yield return Tick();
            }

            ExplosionObject.gameObject.SetActive(true);
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            var explosionScript = ExplosionObject.GetComponent<ExplosionScript>();
            if (explosionScript != null)
            {
                explosionScript.ExplodeNow(null);
            }
            var rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }
        }

        private object Tick()
        {
            SoundService.PlayClip(WeaponDefinitionHolder.WeaponDefinition.ClipOnTimerCount);
            return new WaitForSeconds(1);
        }
    }
}
