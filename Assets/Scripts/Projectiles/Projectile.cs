using System;
using UnityEngine;

namespace Projectiles
{
    public class Projectile : MonoBehaviour, ICustomUpdate
    {
        [SerializeField]
        private LayerMask collisionLayer;
        [SerializeField, Min(0)]
        private int maxBounces;
        [SerializeField, Min(0f)]
        private float lifeTime;
        [SerializeField]
        private float collisionRadius;

        [SerializeField, Min(0f)] 
        private float activationDelay;

        private float m_currentSpeed;
        private Vector3 m_currentDirection;
        private float m_lifeTimeRemaining;
        private int m_bouncesRemaining;
        private float m_startTime;
        private RaycastHit[] m_raycastHits;

        public void Launch(float speed, Vector3 direction)
        {
            m_startTime = Time.timeSinceLevelLoad;
            
            m_currentSpeed = speed;
            m_currentDirection = direction.normalized;
            m_bouncesRemaining = maxBounces;
            m_lifeTimeRemaining = lifeTime;

            m_raycastHits = new RaycastHit[1];
        }
        
        public bool CustomUpdate(float deltaTime)
        {
            if (lifeTime > 0f && m_lifeTimeRemaining - deltaTime <= 0f)
            {
                Destroy(gameObject);
                return false;
            }
            
            m_lifeTimeRemaining -= deltaTime;

            //If we're allowed to check for collisions, and it says something happened, we can destroy & return
            if (Time.timeSinceLevelLoad - m_startTime > activationDelay && !CheckForCollisions())
            {
                Destroy(gameObject);
                return false;
            }

            //TODO Play some VFX for the bounce
            //TODO Play some SFX for the bounce
            
            transform.position += m_currentDirection * (m_currentSpeed * Time.deltaTime);
            
            return true;
        }

        private bool CheckForCollisions()
        {
            var ray = new Ray(transform.position, in m_currentDirection);

            var hitCount = Physics.SphereCastNonAlloc(ray, 
                collisionRadius, 
                m_raycastHits, 
                m_currentSpeed * Time.deltaTime, 
                collisionLayer.value, 
                QueryTriggerInteraction.Collide);

            if (hitCount > 0)
            {
                var canBeHit = m_raycastHits[0].transform.GetComponent<ICanBeHit>();
                if (canBeHit != null)
                {
                    return canBeHit.Hit(this);
                }
                
                
                if (maxBounces > 0 && m_bouncesRemaining-- == 1)
                    return false;
                
                m_currentDirection = Vector3.Reflect(m_currentDirection, m_raycastHits[0].normal);
            }

            return true;
        }

        public void ForceSet(Vector3 position, Vector3 newDirection)
        {
            transform.position = position;
            m_currentDirection = newDirection.normalized;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, collisionRadius);
        }
    }
}