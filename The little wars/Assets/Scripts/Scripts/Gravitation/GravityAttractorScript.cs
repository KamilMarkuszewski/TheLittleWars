﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Scripts
{
    public class GravityAttractorScript : MonoBehaviour
    {
        public float Gravity = -400;
        public float AtmosphereThickness = 3.0f;
        public Transform AtmosphereObject;


        private void Start()
        {
            AtmosphereObject.transform.localScale = new Vector3(AtmosphereThickness, AtmosphereThickness, AtmosphereThickness);
        }

        public void ApplyRotation(Transform body)
        {
            var h = Vector3.Distance(body.transform.position, transform.position);
            if (h < AtmosphereThickness)
            {
                var bodyUp = body.up;
                var gravityUp = GravityUp(body);
                body.rotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
            }
            else
            {
                var bodyUp = body.up;
                var gravityUp = GravityUp(body);
                var targetRot = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
                body.rotation = Quaternion.RotateTowards(body.rotation, targetRot, Time.deltaTime * 500 / h);
            }

        }

        public void Attract(Transform body, Rigidbody2D rigidbodyBody)
        {
            var gravityUp = GravityUp(body);
            var h = GetDistance(body);

            if (h < AtmosphereThickness + AtmosphereThickness/3)
            {
                h -= (4.0f - h) / 3.0f;
            }
            if (h < AtmosphereThickness - AtmosphereThickness / 3)
            {
                h = 2.0f;
            }
            var force = gravityUp * Gravity / (h * h);
            rigidbodyBody.gravityScale = 0;
            rigidbodyBody.AddForce(force);
        }

        private Vector3 GravityUp(Transform body)
        {
            Vector3 vector = (body.position - transform.position).normalized;
            vector = new Vector3(vector.x, vector.y, 0);
            return vector;
        }

        public float GetDistance(Transform body)
        {
            return (body.position - transform.position).magnitude;
        }
    }
}
