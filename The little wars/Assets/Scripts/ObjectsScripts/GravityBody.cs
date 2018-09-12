using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ObjectsScripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GravityBody : MonoBehaviour
    {
        private readonly List<GravityAttractor> _gravityAtractors = new List<GravityAttractor>();

        private Rigidbody2D _rigidbody2D;

        public float Dist;


        // Use this for initialization
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            var sources = GameObject.FindGameObjectsWithTag("GravitySource");
            foreach (var source in sources)
            {
                _gravityAtractors.Add(source.GetComponent<GravityAttractor>());
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            ApplyGravity(transform, _gravityAtractors);
        }

        private void ApplyGravity(Transform myTransform, List<GravityAttractor> gravityAtractors)
        {
            if (gravityAtractors.Any())
            {
                var closestGravityAttractor = GetClosestGravityAttractor(myTransform, gravityAtractors);
                closestGravityAttractor.ApplyRotation(myTransform);
                gravityAtractors.ForEach(a => a.Attract(myTransform, _rigidbody2D));
            }
        }

        private GravityAttractor GetClosestGravityAttractor(Transform myTransform, List<GravityAttractor> gravityAtractors)
        {
            var closestGravityAttractor = gravityAtractors.First();
            var closestAttractorDistance = closestGravityAttractor.GetDistance(myTransform);
            foreach (var attractor in gravityAtractors)
            {
                var dist = attractor.GetDistance(myTransform);
                if (dist < closestAttractorDistance)
                {
                    closestAttractorDistance = dist;
                    closestGravityAttractor = attractor;
                    Dist = dist;
                }
            }
            
            return closestGravityAttractor;
        }
    }
}
