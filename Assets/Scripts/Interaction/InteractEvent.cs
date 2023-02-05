using Misc.Interfaces;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class InteractEvent : InteractableObject
    {
        [SerializeField] protected bool canInteractMoreThanOnce;

        public UnityEvent interactEvent;
        public override void Interact()
        {
            interacted = true;
            interactEvent?.Invoke();
            if (!canInteractMoreThanOnce && interacted)
            {
                EndPurpose();
            }
        }
    }
}
