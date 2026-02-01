using Audio;
using VisualFX;
using UnityEngine;

namespace Projectiles.CanBeHit
{
    public class Destructible : MonoBehaviour, ICanBeHit
    {
        public bool Hit(Projectile _)
        {
            SFXManager.PlaySoundAtLocation(SFX.DRESTROY_WALL, transform.position);
            GameObject effect = VFXManager.PlayAtLocation(VFX.WALL_EXPLODE, transform.position + Vector3.up * 2f);
            effect.transform.forward = transform.forward;
        
            Destroy(gameObject);
            return false;
        }
    }
}