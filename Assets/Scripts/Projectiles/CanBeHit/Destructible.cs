using Audio;
using UnityEngine;

namespace Projectiles.CanBeHit
{
    public class Destructible : MonoBehaviour, ICanBeHit
    {
        public bool Hit(Projectile _)
        {
            SFXManager.PlaySoundAtLocation(SFX.DRESTROY_WALL, transform.position);
            Destroy(gameObject);
            return false;
        }
    }
}