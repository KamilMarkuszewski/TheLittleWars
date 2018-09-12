using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GravityBodyScript : MonoBehaviour
    {
        private readonly List<GravityAttractorScript> _gravityAtractors = new List<GravityAttractorScript>();

        private Rigidbody2D _rigidbody2D;

        public float Dist;


        // Use this for initialization
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            var sources = GameObject.FindGameObjectsWithTag(Tags.GravitySource);
            foreach (var source in sources)
            {
                _gravityAtractors.Add(source.GetComponent<GravityAttractorScript>());
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            ApplyGravity(transform, _gravityAtractors);
        }

        private void ApplyGravity(Transform myTransform, List<GravityAttractorScript> gravityAtractors)
        {
            if (gravityAtractors.Any())
            {
                var closestGravityAttractor = GetClosestGravityAttractor(myTransform, gravityAtractors);
                closestGravityAttractor.ApplyRotation(myTransform);
                gravityAtractors.ForEach(a => a.Attract(myTransform, _rigidbody2D));
            }
        }

        private GravityAttractorScript GetClosestGravityAttractor(Transform myTransform, List<GravityAttractorScript> gravityAtractors)
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
