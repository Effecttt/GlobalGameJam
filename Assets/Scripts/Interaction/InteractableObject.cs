using System;
using Misc.Interfaces;
using UnityEngine;

namespace Interaction
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] protected Transform origin;
        private SpriteRenderer render;

        private void Awake()
        {
            render = origin.GetComponent<SpriteRenderer>();
            Initialize();
        }

        protected virtual void Initialize()
        {
            
        }

        public virtual void Interact()
        {
            
        }

        public virtual void HighlightObject()
        {
            render.color = Color.green;
        }

        public virtual void CancelHighlighting()
        {
            render.color = Color.white;
        }
    }
}