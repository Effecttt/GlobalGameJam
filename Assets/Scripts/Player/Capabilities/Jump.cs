using System;
using Checks;
using Controllers;
using Player.Movement;
using UnityEngine;

namespace Player.Capabilities
{
    public class Jump : MonoBehaviour
    {
        [SerializeField] private InputController input = null;
        [SerializeField, Range(0, 10)] private float jumpHeight = 3;
        [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
        [SerializeField, Range(0, 5)] private float downwardMovementMultiplier = 3f;
        [SerializeField, Range(0, 5)] private float upwardMovementMultiplier = 1.7f;
        [SerializeField, Range(0, .3f)] private float coyoteTime = 0.2f;

        private Rigidbody2D rb;
        private Ground ground;
        private Vector2 velocity;

        private int jumpPhase;
        private float defaultGravityScale;
        private float coyoteCounter;
        
        private bool desiredJump;
        public bool onGround;
        private bool isJumping;
        private bool land;

        public static event Action landSound;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            ground = GetComponent<Ground>();
            defaultGravityScale = 1f;
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            desiredJump |= input.RetrieveJumpInput();
            if (!onGround)
            {
                land = true;
            } else if (onGround && land)
            {
                land = false;
                landSound?.Invoke();
            }
        }

        private void FixedUpdate()
        {
            onGround = ground.GetOnGround();
            velocity = rb.velocity;

            if (onGround && rb.velocity.y == 0)
            {
                jumpPhase = 0;
                coyoteCounter = coyoteTime;
                isJumping = false;
            }
            else
            {
                coyoteCounter -= Time.deltaTime;
            }

            if (desiredJump)
            {
                desiredJump = false;
                JumpAction();
            }

            if (input.RetrieveJumpHoldInput() && rb.velocity.y > 0)
            {
                rb.gravityScale = upwardMovementMultiplier;
            }else if (!input.RetrieveJumpHoldInput() || rb.velocity.y < 0)
            {
                rb.gravityScale = downwardMovementMultiplier;
            }
            else
            {
                rb.gravityScale = defaultGravityScale;
            }

            rb.velocity = velocity;
        }

        void JumpAction()
        {
            if (coyoteCounter > 0 || (jumpPhase < maxAirJumps && isJumping))
            {
                if (isJumping)
                {
                    jumpPhase++;
                }

                coyoteCounter = 0;
                float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
                isJumping = true;
                
                if (velocity.y > 0f)
                {
                    jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0);
                }

                velocity.y += jumpSpeed;
            }
        }
    }
}
