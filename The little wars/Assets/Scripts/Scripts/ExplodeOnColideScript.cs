using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using UnityEngine;
using Assets.Scripts.ExtensionMethods;

namespace Assets.Scripts.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ExplodeOnColideScript : MonoBehaviour
    {
        public Transform ExplosionObject;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag.In(Tags.Map, Tags.Unit, Tags.Bullet))
            {
                ExplosionObject.gameObject.SetActive(true);
                var rb = GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;
            }
        }
    }
}
