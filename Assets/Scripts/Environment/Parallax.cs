using System;
using UnityEngine;

namespace Environment
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField] private Transform cam;
        [SerializeField] private float parallaxEffect;
        [SerializeField] private bool IsY;
        
        private float length, startPos;

        private void Awake()
        {
            startPos = transform.position.x;
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void Update()
        {
            float distance = cam.transform.position.x * parallaxEffect;
            float rePos = cam.transform.position.x * (1 - parallaxEffect);
            float y = IsY ? cam.transform.position.y : transform.position.y;
            transform.position = new Vector3(startPos + distance, y, transform.position.z);

            if (rePos > startPos + length)
            {
                startPos += length;
            }else if (rePos < startPos - length)
            {
                startPos -= length;
            }
            
        }
    }
}