using System;
using Player.Movement;
using UnityEngine;

namespace Player
{
    public class PlayerParameters : MonoBehaviour
    {
        #region Singleton

        public static PlayerParameters Instance;
        private void Awake()
        {
            Instance = this;
        }

        #endregion

        public float rigidbodyGravityDefaultValue = 2;
        public float defaultSpeed, fallingSpeed, jumpingSpeed, swingSpeed;

    }
}