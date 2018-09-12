using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ObjectsScripts
{
    public class ExplodeOnColideScript : MonoBehaviour
    {
        public Sprite ExplSprite;

        // Use this for initialization
        void Start()
        {

        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var mapDestroyScript = collision.collider.gameObject.GetComponent<DestroyScript>();
            if (mapDestroyScript != null)
            {
                Vector2 explosionCenter = new Vector2(transform.position.x, transform.position.y);
                mapDestroyScript.MergeWithMainTexture(explosionCenter, ExplSprite);
                Destroy(gameObject);
            }
        }
    }
}
