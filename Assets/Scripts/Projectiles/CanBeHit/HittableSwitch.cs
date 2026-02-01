using Audio;
using UnityEngine;
using UnityEngine.Events;

namespace Projectiles.CanBeHit
{
    public class HittableSwitch  : MonoBehaviour, ICanBeHit
    {
        [SerializeField]
        protected UnityEvent hitEvent;

        public virtual void Interact()
        {
            SFXManager.PlaySoundAtLocation(SFX.INTERACT_SWITCH, transform.position);

            hitEvent.Invoke();
        }
        
        public virtual bool Hit(Projectile projectile)
        {
            hitEvent.Invoke();
            return false;
        }
    }
}