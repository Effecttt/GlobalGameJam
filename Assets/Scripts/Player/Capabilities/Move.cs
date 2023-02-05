using System;
using Checks;
using Controllers;
using UnityEngine;

namespace Player.Capabilities
{
    public class Move : MonoBehaviour
    {
        [SerializeField] private InputController input = null;
        [SerializeField, Range(0, 100)] private float maxSpeed = 4f;
        [SerializeField, Range(0, 100)] private float maxAcceleration = 35f;
        [SerializeField, Range(0, 100)] private float maxAirAcceleration = 20f;

        private Vector2 direction;
        private Vector2 desiredVelocity;
        private Vector2 velocity;
        private Rigidbody2D rb;
        private Ground ground;
        private Animator anim;

        private float maxSpeedChange;
        private float acceleration;
        private bool onGround;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            ground = GetComponent<Ground>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            direction.x = input.RetrieveMoveInput();
            desiredVelocity = new Vector2(direction.x, 0) * Mathf.Max(maxSpeed - ground.GetFriction(),0f);
            if (direction.x != 0)
            {
                anim.SetBool("IsRunning", true);
                if (direction.x > 0)
                {
                    anim.SetBool("IsRight", true);
                }
                else
                {
                    anim.SetBool("IsRight", false);
                }
            }
            else
            {
                anim.SetBool("IsRunning", false);
            }
        }

        private void FixedUpdate()
        {
            onGround = ground.GetOnGround();
            velocity = rb.velocity;

            acceleration = onGround ? maxAcceleration : maxAirAcceleration;
            maxSpeedChange = acceleration * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

            rb.velocity = velocity;
        }
    }
}