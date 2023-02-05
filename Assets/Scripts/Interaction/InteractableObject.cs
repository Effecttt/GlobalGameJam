using System;
using Misc;
using Misc.Interfaces;
using UnityEngine;

namespace Interaction
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] protected Transform parent;
        
        protected bool interacted;

        protected bool isPurposeEnded;
        private GameObject button;
        private BoxCollider2D col;
        private Bounds b;
        private SpriteRenderer render;

        private void Awake()
        {
            render = parent.GetComponent<SpriteRenderer>();
            col = parent.GetComponent<BoxCollider2D>();
            b = col.bounds;
            Initialize();
            if (!button)
            {
                var instance = (GameObject)Resources.Load("Prefabs/button", typeof(GameObject));
                button = Instantiate(instance);
                button.SetActive(false);
            }
        }

        protected virtual void Initialize()
        {
            
        }

        public virtual void Interact()
        {
            EndPurpose();
        }

        public virtual void HighlightObject()
        {
            if(isPurposeEnded) return;
            if(!button.activeSelf) button.SetActive(true);
            Vector3 newPos = new Vector3(transform.position.x,transform.position.y + .2f,transform.position.z);
            button.transform.position = newPos;
        }

        public virtual void CancelHighlighting()
        {
            button.SetActive(false);
        }

        protected virtual void EndPurpose()
        {
            isPurposeEnded = true;
            CancelHighlighting();
        }
    }
}