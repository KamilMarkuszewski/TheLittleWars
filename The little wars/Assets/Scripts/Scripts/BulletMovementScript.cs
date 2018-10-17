using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Scripts.ExplosionScripts;
using Assets.Scripts.Scripts.Gravitation;
using Photon.Pun;
using UnityEngine;

namespace Assets.Scripts.Scripts
{
    [RequireComponent(typeof(PhotonView))]
    public class BulletMovementScript : MonoBehaviourPun, IPunObservable
    {
        #region Components

        private GravityBodyScript _gravityBodyScript;
        private GravityBodyScript GravityBodyScript
        {
            get { return _gravityBodyScript ?? (_gravityBodyScript = GetComponent<GravityBodyScript>()); }
        }

        private Rigidbody2D _rigidbody2D;
        private Rigidbody2D Rigidbody2D
        {
            get { return _rigidbody2D ?? (_rigidbody2D = GetComponent<Rigidbody2D>()); }
        }

        private ExplodeOnColideScript _explodeOnColideScript;
        private ExplodeOnColideScript ExplodeOnColideScript
        {
            get { return _explodeOnColideScript ?? (_explodeOnColideScript = GetComponent<ExplodeOnColideScript>()); }
        }

        #endregion

        private Vector3 _targetPosition;
        private Quaternion _targetRotation;
        private bool _isOwner; // PhotonNetwork.IsMine seems need to much time in my case

        public void OwnerInitialize(Vector3 position)
        {
            InitializePosition(position);
            SetPhotonSendRate();
            _isOwner = true;

            if (GravityBodyScript != null) // i assume this can be not attached at all (different weapon types)
            {
                GravityBodyScript.Enabled = true;
            }
            if (Rigidbody2D != null) // i assume this can be not attached at all (different weapon types)
            {
                Rigidbody2D.simulated = true;
            }
            if (ExplodeOnColideScript != null) // i assume this can be not attached at all (different weapon types)
            {
                ExplodeOnColideScript.Enabled = true;
            }
        }

        public void Initialize(Vector3 position)
        {
            InitializePosition(position);
            SetPhotonSendRate();
            _isOwner = false;

            if (GravityBodyScript != null) // i assume this can be not attached at all (different weapon types)
            {
                GravityBodyScript.Enabled = false;
            }
            if (Rigidbody2D != null) // i assume this can be not attached at all (different weapon types)
            {
                Rigidbody2D.simulated = false;
            }
            if (ExplodeOnColideScript != null) // i assume this can be not attached at all (different weapon types)
            {
                ExplodeOnColideScript.Enabled = false;
            }
        }

        private void InitializePosition(Vector3 position)
        {
            transform.position = position;
            _targetPosition = position;
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isOwner)
            {
                transform.position = Vector3.Lerp(transform.position, _targetPosition, 0.25f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 500 * Time.deltaTime);
            }
        }

        #region Photon

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                _targetPosition = (Vector3)stream.ReceiveNext();
                _targetRotation = (Quaternion)stream.ReceiveNext();
            }
        }

        private static void SetPhotonSendRate()
        {
            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 40;
        }

        #endregion

    }
}
