using UnityEngine;

namespace Projectiles.CanBeHit
{
    public class Destructible : MonoBehaviour, ICanBeHit
    {
        public bool Hit(Projectile _)
        {
            Destroy(gameObject);
            return false;
        }
    }
}