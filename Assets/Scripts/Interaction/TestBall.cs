using UnityEngine;

namespace Interaction
{
    public class TestBall : InteractableObject
    {
        private Rigidbody2D rb;

        protected override void Initialize()
        {
            rb = parent.GetComponent<Rigidbody2D>();
        }

        public override void Interact()
        {
            rb.AddForce(Vector2.up * 400);
        }
    }
}