using System;
using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        public static event Action JumpPressed, InteractPressed, SkillPressed;
        public KeyCode jump, interact, skill = KeyCode.Q;

        private void Update()
        {
            if (Input.GetKeyDown(jump))
            {
                JumpPressed?.Invoke();
            }

            if (Input.GetKeyDown(interact))
            {
                InteractPressed?.Invoke();
            }

            if (Input.GetKeyDown(skill))
            {
                SkillPressed?.Invoke();
            }
        }
    }
}