using System;
using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class MovableBlock : Grappable
    {
        [SerializeField] private float moveSpeed = 1;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void MoveBlock(Vector2 mousePos)
        {
            Vector3 direction = mousePos.x > transform.position.x ? Vector2.right : Vector2.left;
            if (Vector3.Distance(transform.position, mousePos) > 0.1)
            {
                transform.position += direction/2 * Time.deltaTime * moveSpeed;
            }
        }
    }
}
