using UnityEngine;
using Interactables;

namespace Projectiles.CanBeHit
{
    public class HittableSwitchInteractable : HittableSwitch, IInteractable
    {
        public float InteractionDistance => interactionDistance;
        [SerializeField, Min(0.1f)]
        private float interactionDistance = 0.1f;
        
        
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
        }
        
        public override bool Hit(Projectile _)
        {
            Interact();
            return false;
        }


    }
}