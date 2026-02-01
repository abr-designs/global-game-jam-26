using Audio;
using UnityEngine;

namespace Projectiles.CanBeHit
{
    public class Destructible : MonoBehaviour, ICanBeHit
    {
        public bool Hit(Projectile _)
        {
            SFXManager.PlaySound(SFX.DRESTROY_WALL);
            Destroy(gameObject);
            return false;
        }
    }
}