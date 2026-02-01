using System;
using UnityEngine;
using Interactables;
using Utilities.Debugging;
using VisualFX;

namespace Projectiles.CanBeHit
{
    public class HittableSwitchInteractable : HittableSwitch, IInteractable
    {
        public float InteractionDistance => interactionDistance;
        [SerializeField, Min(0.1f)]
        private float interactionDistance = 0.1f;

        [SerializeField]
        private VFX vfxOnActivate;
        
        
        public void OnEnable()
        {
            InteractableManager.Register(this);
        }

        public void OnDisable()
        {
            InteractableManager.DeRegister(this);
        }
        
        public override void Interact()
        {
            base.Interact();
            
            if(vfxOnActivate != VFX.NONE)
                vfxOnActivate.PlayAtLocation(transform.position);
        }
        
        public override bool Hit(Projectile _)
        {
            Interact();
            return false;
        }

        private void OnDrawGizmos()
        {
            Draw.Circle(transform.position, Vector3.up, Color.white, InteractionDistance);
        }
    }
}