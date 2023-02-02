using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        //Main
        [SerializeField] private float speed, jumpForce, leapForce, extraHeightTest, climbDir;
        [SerializeField] private LayerMask platformLayerMask;
        

        //Dependencies
        public State state;
        
        private Rigidbody2D rb;
        private BoxCollider2D body;
        private Dictionary<State, float> velocityChanges;

        //misc
        private Vector2 movement;
        private bool isSwinging = false, isClimbing = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            body = GetComponent<BoxCollider2D>();
            
            velocityChanges = new Dictionary<State, float>()
            {
                {State.Idle, 30},
                {State.Moving, 30},
                {State.Jumping, 0},
                {State.Falling, 10},
                {State.Swinging, 10},
                {State.Climbing, 30},
                {State.Default, 30}
            };
        }

        private void OnEnable()
        {
            PlayerInput.JumpPressed += PlayerInputOnJumpPressed;
            GrapplingGun.GrapplingGun.Swing += GrapplingGunOnSwing;
            GrapplingGun.GrapplingGun.ExitSwing += GrapplingGunOnExitSwing;
            GrapplingGun.GrapplingGun.EndRope += OnEndRope;
            state = State.Idle;
        }

        private void OnDisable()
        {
            PlayerInput.JumpPressed -= PlayerInputOnJumpPressed;
            GrapplingGun.GrapplingGun.Swing -= GrapplingGunOnSwing;
            GrapplingGun.GrapplingGun.ExitSwing -= GrapplingGunOnExitSwing;
            GrapplingGun.GrapplingGun.EndRope -= OnEndRope;
            isSwinging = false;
        }

        private void Update()
        {
            InputGathering();
            StateChecker();
            speed = velocityChanges[state];
        }

        private void FixedUpdate()
        {
            if(state != State.Jumping)Move();
        }

        void InputGathering()
        {
            float x = Input.GetAxisRaw("Horizontal");

            movement = new Vector2(x,0).normalized;
        }
        void Move()
        {
            if (movement.sqrMagnitude > 0.01 && state != State.Climbing)
            {
                rb.AddForce(movement * speed);
            }

            if (state == State.Climbing)
            {
                rb.MovePosition(rb.position + new Vector2(movement.x,climbDir * speed/10) * Time.deltaTime);
            }
        }
        public bool GroundCheck()
        {
            Bounds col = body.bounds;
            Vector3 offset = new Vector3(0.1f, 0f, 0f);
            RaycastHit2D hit = Physics2D.BoxCast(col.center, col.size - offset, 0f, 
                Vector2.down, extraHeightTest, platformLayerMask);
            if (hit)
            {
                if (hit.transform.CompareTag("Ground"))
                {
                    return true;
                }
            }
            return false;
        }
        void PlayerInputOnJumpPressed()
        {
            if (GroundCheck())
            {
                rb.AddForce(Vector2.up * jumpForce);
            }
        }

        private void StateChecker()
        {
            if (isClimbing)
            {
                state = State.Climbing;
                return;
            }
            if (isSwinging)
            {
                state = State.Swinging;
                return;
            }
            if (rb.velocity.sqrMagnitude < 0.01)
            {
                state = State.Idle;
            }
            else if (!GroundCheck() && state != State.Climbing && rb.velocity.y > 0)
            {
                state = State.Jumping;
            } 
            else if (!GroundCheck() && state != State.Climbing && rb.velocity.y < 0)
            {
                state = State.Falling;
            } 
            else if (rb.velocity.x != 0 && GroundCheck())
            {
                state = State.Moving;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Vine"))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    rb.gravityScale = 0;
                    climbDir = 1;
                    isClimbing = true;
                } else if (Input.GetKey(KeyCode.S))
                {
                    rb.gravityScale = 0;
                    climbDir = -1;
                }
                else
                {
                    if (GroundCheck())
                    {
                        rb.gravityScale = 2;
                        isClimbing = false;
                    }
                    else
                    {
                        rb.gravityScale = 0;
                    }
                    climbDir = 0;
                    
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Vine"))
            {
                climbDir = 0;
                isClimbing = false;
                state = State.Default;
                rb.gravityScale = 2;
            }
        }

        private void GrapplingGunOnSwing()
        {
            isSwinging = true;
        }
        
        private void GrapplingGunOnExitSwing()
        {
            isSwinging = false;
            Vector2 forceDirection = movement.x > 0 ? new Vector2(.7f,.7f) : new Vector2(-.7f,.7f);
            if(movement.x == 0 || GroundCheck()) forceDirection = Vector2.zero;
            rb.AddForce(forceDirection * leapForce);
        }

        void OnEndRope()
        {
            isSwinging = false;
        }
    }

    public enum State
    {
        Idle,
        Moving,
        Jumping,
        Falling,
        Swinging,
        Climbing,
        Default
    }
}
