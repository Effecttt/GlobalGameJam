using System;
using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        //Main
        [SerializeField] private float speed, jumpForce, extraHeightTest;
        [SerializeField] private LayerMask platformLayerMask;
        

        //Dependencies
        private Rigidbody2D rb;
        private BoxCollider2D body;
        
        //misc
        private Vector2 movement;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            body = GetComponent<BoxCollider2D>();
        }

        private void OnEnable()
        {
            PlayerInput.JumpPressed += PlayerInputOnJumpPressed;
        }
        private void OnDisable()
        {
            PlayerInput.JumpPressed -= PlayerInputOnJumpPressed;
        }

        private void Update()
        {
            InputGathering();
        }

        private void FixedUpdate()
        {
            Move();
        }

        void InputGathering()
        {
            float x = Input.GetAxisRaw("Horizontal");

            movement = new Vector2(x,0).normalized;
        }
        void Move()
        {
            if (movement.sqrMagnitude > 0.01)
            {
                rb.AddForce(movement * speed);
            }
        }
        bool GroundCheck()
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
    }
}
