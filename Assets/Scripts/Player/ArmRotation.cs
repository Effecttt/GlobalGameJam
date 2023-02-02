using System;
using UnityEngine;

namespace Player
{
    public class ArmRotation : MonoBehaviour
    {
        [SerializeField]private Transform gunPivot;
        [SerializeField]private Camera cam;

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            RotateGun(mousePos);
        }

        void RotateGun(Vector3 lookPoint)
        {
            Vector3 distanceVector = lookPoint - gunPivot.position;

            float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}