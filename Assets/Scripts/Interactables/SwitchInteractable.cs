using System;
using UnityEngine;
using UnityEngine.Events;
using Utilities.Debugging;

namespace Interactables
{
    public class SwitchInteractable : MonoBehaviour, IInteractable
    {
        public float InteractionDistance => interactionDistance;
        
        [SerializeField, Min(0.1f)]
        private float interactionDistance;

        [SerializeField]
        private UnityEvent onInteracted;
        
        public void OnEnable()
        {
            InteractableManager.Register(this);
        }

        public void OnDisable()
        {
            InteractableManager.DeRegister(this);
        }

        public void Interact()
        {
            onInteracted?.Invoke();
        }

        private void OnDrawGizmos()
        {
            var pos = transform.position;
            
            Draw.Circle(pos, Vector3.up, Color.white, interactionDistance);
        }
    }
}