using System.Collections;
using Audio;
using GGJ.Player.Enums;
using GGJ.Player.Interfaces;
using Interactables;
using Samples.CharacterController3D.Scripts;
using UnityEngine;
using Utilities.Debugging;

namespace GGJ.Player
{
    public class CharacterDashAbility : MonoBehaviour, IAbility
    {
        private static readonly int DodgingAnimationHash = Animator.StringToHash("Dodging");
        public bool IsBusy { get; private set; }
        public MASK_TYPE MaskType => MASK_TYPE.DASH;
        
        [SerializeField]
        private CharacterMovement3DDataScriptableObject moveData;

        [SerializeField]
        private CapsuleCollider playerCollider;

        [SerializeField]
        private Animator playerAnimator;

        private RaycastHit[] m_raycastHits;
        private float m_lastUsed;

        private void Start()
        {
            IAbility.CharacterController3D ??= FindFirstObjectByType<CharacterController3D>(FindObjectsInactive.Exclude);
        }

        //TODO Consider if we should be checking the Grounded state or the Coyote Time
        public void UseAbility()
        {
            //TODO Optional if ability & Interactable 
            //if (InteractableManager.InteractablesInRange)
            //    return;
            
            if (IsBusy)
                return;

            if (Time.timeSinceLevelLoad - m_lastUsed < moveData.DashCooldown)
                return;

            var playerTransform = IAbility.CharacterController3D.transform;
            
            m_raycastHits ??= new RaycastHit[10];
            
            var startPosition = playerTransform.position;
            var forward = Vector3.ProjectOnPlane(playerTransform.forward, Vector3.up).normalized;
            var dir = moveData.DashDistance * forward;
            var dest = startPosition + dir;

            var startBehind = startPosition - (forward * playerCollider.radius * 2f);
            var ray = new Ray(startBehind, forward);
            var collision = Physics.SphereCastNonAlloc(ray, playerCollider.radius, m_raycastHits, moveData.DashDistance, moveData.dashCollisionLayer.value);

            var maxT = 1f;
            //If we're going to hit a wall, set that as the max travel distance
            if (collision > 0)
            {
                Vector3 contactDir = Vector3.zero;
                for (int i = 0; i < collision; i++)
                {
                    if (m_raycastHits[i].distance < 0.01f)
                        continue;
                    
                    contactDir = Vector3.ProjectOnPlane(m_raycastHits[i].point - startPosition, Vector3.up);

                    //When there is a contact in front of the player, then we can use that as the target Max T
                    var dot = Vector3.Dot(contactDir, forward);
                    if (dot > 0f)
                        break;
                }
                
                //Stop at point
                maxT = contactDir.magnitude / moveData.DashDistance;
            }

            SFXManager.PlaySound(SFX.DASH);

            StartCoroutine(DashCoroutine(playerTransform, startPosition, dest, moveData.DashTime, maxT));
        }

        private IEnumerator DashCoroutine(Transform targetTransform, Vector3 startPos, Vector3 destination, float totalTime, float maxT = 1f)
        {
            IsBusy = true;
            playerAnimator.SetBool(DodgingAnimationHash, true);
            IAbility.CharacterController3D.TogglePhysics(false);
            
            for (var t = 0f; t < totalTime && t / totalTime <= maxT; t += Time.deltaTime)
            {
                Draw.Circle(startPos, Vector3.up, Color.green, playerCollider.radius);
                Draw.Circle(destination, Vector3.up, Color.red, playerCollider.radius);
                
                var dt = moveData.DashCurve.Evaluate(t / totalTime);

                targetTransform.position = Vector3.Lerp(startPos, destination, dt);
                yield return null;
            }
            
            IAbility.CharacterController3D.TogglePhysics(true);
            playerAnimator.SetBool(DodgingAnimationHash, false);

            m_lastUsed = Time.timeSinceLevelLoad;
            
            IsBusy = false;
        }
    }
}