using System;
using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        public static event Action InteractPressed, SkillPressed;
        public KeyCode interact = KeyCode.E, skill = KeyCode.Q;

        private void Update()
        {
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