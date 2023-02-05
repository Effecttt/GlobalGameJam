using System;
using UnityEngine;

namespace Checks
{
    public class Ground : MonoBehaviour
    {
        private bool onGround;
        private float friction;
        private PhysicsMaterial2D material;

        private void OnCollisionEnter2D(Collision2D other)
        {
            EvaluateCollision(other);
            RetrieveFriction(other);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            EvaluateCollision(other);
            RetrieveFriction(other);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            onGround = false;
            friction = 0;
        }



        void EvaluateCollision(Collision2D other)
        {
            if (!other.transform.CompareTag("Ground")) return;
            for (int i = 0; i < other.contactCount; i++)
            {
                Vector2 normal = other.GetContact(i).normal;
                onGround |= normal.y >= .9f;
            }
        }

        void RetrieveFriction(Collision2D other)
        {
            if (!other.transform.CompareTag("Ground")) return;
            material = other.rigidbody.sharedMaterial;
            friction = 0;

            if (material != null)
            {
                friction = material.friction;
            }
        }

        public bool GetOnGround()
        {
            return onGround;
        }

        public float GetFriction()
        {
            return friction;
        }
    }
}