using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using Assets.Scripts.ExtensionMethods;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Scripts.Gravitation;
using Assets.Scripts.Services;
using Assets.Scripts.Utility;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Scripts.ExplosionScripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(WeaponDefinitionHolder))]
    [RequireComponent(typeof(PhotonView))]
    public class ExplodeOnColideScript : MonoBehaviour
    {
        private Vector3 _lastPosition;
        private Vector3 _previousPosition;

        void OnEnable()
        {
            Assert.IsNotNull(ExplosionObject, "Object " + gameObject.name + " missing child " + "explosion");
        }

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

        private Rigidbody2D _rigidbody2D;
        public Rigidbody2D Rigidbody2D
        {
            get { return _rigidbody2D ?? (_rigidbody2D = GetComponent<Rigidbody2D>()); }
        }

        private GravityBodyScript _gravityBodyScript;
        public GravityBodyScript GravityBodyScript
        {
            get { return _gravityBodyScript ?? (_gravityBodyScript = GetComponent<GravityBodyScript>()); }
        }



        void Update()
        {
            var curPos = transform.position;
            if (curPos == _lastPosition && curPos == _previousPosition)
            {
                EnableExplode();
            }
            _previousPosition = _lastPosition;
            _lastPosition = curPos;
        }

        public void Reset()
        {
            _hasCollided = false;
            ExplosionObject.gameObject.SetActive(false);
        }

        private bool _hasCollided;
        public bool Enabled = true;

        void OnTriggerEnter2D(Collider2D col)
        {
            OnCollide(col);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollide(collision.collider);
        }

        private void OnCollide(Collider2D col)
        {
            if (col.tag.In(Tags.Map, Tags.Unit, Tags.Bullet) && Enabled)
            {
                if (!_hasCollided)
                {
                    Rigidbody2D.velocity = Vector2.zero;
                    Rigidbody2D.angularVelocity = 0.0f;
                    GravityBodyScript.Enabled = false;
                    EnableExplode();
                    GetComponent<PhotonView>().RPC("RPC_EnableExplodeWithPositionSet", RpcTarget.Others, transform.position);
                }
            }
        }

        [PunRPC]
        public void RPC_EnableExplodeWithPositionSet(Vector3 position)
        {
            EnableExplodeWithPositionSet(position);
        }

        private void EnableExplodeWithPositionSet(Vector3 position)
        {
            transform.position = position;
            EnableExplode();
        }

        private void EnableExplode()
        {
            ExplosionObject.gameObject.SetActive(true);
            ExplosionObject.GetComponent<ExplosionScript>().Initialize();
            var rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.simulated = true;

            _hasCollided = true;
        }
    }
}
