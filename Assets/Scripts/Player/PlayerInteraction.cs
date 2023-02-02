using System;
using System.Collections.Generic;
using System.Linq;
using Interaction;
using UnityEngine;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private readonly HashSet<InteractableObject> objects = new HashSet<InteractableObject>();
        private InteractableObject currentObject;
        
        private void OnEnable()
        {
            PlayerInput.InteractPressed += PlayerInputOnInteractPressed;
        }
        private void OnDisable()
        {
            PlayerInput.InteractPressed -= PlayerInputOnInteractPressed;
        }
        private void Update()
        {
            if (!objects.Contains(currentObject))
            {
                currentObject = null;
            }
            
            if (objects.Count > 1)
            {
                ChangeObject(GetNearestObject());
            }
            else if (objects.Count != 0)
            {
                ChangeObject(objects.FirstOrDefault());
            }
        }

        void ChangeObject(InteractableObject ob)
        {
            if (currentObject)
            {
                currentObject.CancelHighlighting();
            }

            currentObject = ob;
            currentObject.HighlightObject();
        }
        InteractableObject GetNearestObject()
        {
            InteractableObject io = null;
            float minimumDistance = Mathf.Infinity;
            Vector3 pos = transform.position;
            foreach (var ob in objects)
            {
                float distance = Vector3.Distance(ob.transform.position, pos);
                if (distance < minimumDistance)
                {
                    io = ob;
                    minimumDistance = distance;
                }
            }

            return io;
        }
        void PlayerInputOnInteractPressed()
        {
            if(currentObject) currentObject.Interact();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Interact"))
            {
                objects.Add(other.GetComponent<InteractableObject>());
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Interact"))
            {
                InteractableObject io = other.GetComponent<InteractableObject>();
                io.CancelHighlighting();
                objects.Remove(io);
            }
        }
    }
}