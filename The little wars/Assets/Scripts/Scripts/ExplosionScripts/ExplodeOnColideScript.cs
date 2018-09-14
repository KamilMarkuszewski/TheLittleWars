using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using UnityEngine;
using Assets.Scripts.ExtensionMethods;
using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(WeaponDefinitionHolder))]
    public class ExplodeOnColideScript : MonoBehaviour
    {
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

        private bool _hasCollided = false;

        private WeaponDefinitionHolder _weaponDefinitionHolder;
        private WeaponDefinitionHolder WeaponDefinitionHolder
        {
            get { return _weaponDefinitionHolder ?? (_weaponDefinitionHolder = GetComponent<WeaponDefinitionHolder>()); }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.collider.tag.In(Tags.Map, Tags.Unit, Tags.Bullet))
            {
                if (!_hasCollided)
                {
                    ExplosionObject.gameObject.SetActive(true);
                    var rb = GetComponent<Rigidbody2D>();
                    rb.velocity = Vector2.zero;

                    _hasCollided = true;
                }
            }
        }
    }
}
