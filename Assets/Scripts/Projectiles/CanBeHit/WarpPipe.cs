using UnityEngine;
using UnityEngine.Assertions;
using Utilities.Debugging;

namespace Projectiles.CanBeHit
{
    public class WarpPipe : MonoBehaviour, ICanBeHit
    {
        [SerializeField]
        private WarpPipe warpPipeCompanion;

        [SerializeField]
        private Vector3 localExitDirection;

        [SerializeField]
        private Vector3 localOffsetPosition;

        private void Start()
        {
            Assert.IsNotNull(warpPipeCompanion);
        }

        public bool Hit(Projectile projectile)
        {
            //Tells the projectile to move to the others position
            warpPipeCompanion.Exit(projectile);

            return true;
        }


        private void Exit(Projectile projectile)
        {
            var exitDirection = transform.TransformDirection(localExitDirection).normalized;
            
            var exitPosition = transform.TransformPoint(localOffsetPosition);
            exitPosition += exitDirection * 0.5f;
            
            projectile.ForceSet(exitPosition, exitDirection);
        }
        

        private void OnDrawGizmos()
        {
            var pos = transform.TransformPoint(localOffsetPosition);
            var dir = transform.TransformDirection(localExitDirection);
            
            Draw.Arrow(pos, dir, Color.blue);
        }
    }
}