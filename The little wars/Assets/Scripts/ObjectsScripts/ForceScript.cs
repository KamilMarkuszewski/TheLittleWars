using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ObjectsScripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ForceScript : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        // Use this for initialization
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.AddForce(transform.right * 750);
        }

    }
}
