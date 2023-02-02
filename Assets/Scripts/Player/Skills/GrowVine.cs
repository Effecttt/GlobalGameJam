using System;
using UnityEngine;

namespace Player.Skills
{
    public class GrowVine : MonoBehaviour
    {
        [SerializeField] private GameObject vinePrefab;
        [SerializeField] private float maxVineSize, vineCorrectionOffset;
        [SerializeField] private LayerMask mask;
        [SerializeField] private Vector3 offset;
        private void OnEnable()
        {
            PlayerInput.SkillPressed += PlayerInputOnSkillPressed;
        }
        private void OnDisable()
        {
            PlayerInput.SkillPressed -= PlayerInputOnSkillPressed;
        }
        
        private void PlayerInputOnSkillPressed()
        {
            Instantiate(vinePrefab, transform.position - offset, Quaternion.identity).transform.localScale = new Vector3(1,VineSize()+vineCorrectionOffset,1) + offset;
        }

        float VineSize()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, mask);
            if (hit)
            {
                return hit.point.y - transform.position.y;
            }

            return maxVineSize;
        }
    }
}