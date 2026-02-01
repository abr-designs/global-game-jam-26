using System;
using UnityEngine;

namespace Samples.CharacterController3D.Scripts
{
    public class CharacterAnimationController : MonoBehaviour
    {
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int IsGroundedHash = Animator.StringToHash("Grounded");
        private static readonly int IsDeadHash = Animator.StringToHash("Dead");

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private CharacterController3D characterController3D;
        [SerializeField]
        private Rigidbody characterRigidbody;
        [SerializeField]
        private CharacterMovement3DDataScriptableObject characterMovementData;

        private float m_sqrSpeed;

        //Unity Functions
        //============================================================================================================//

        private void OnEnable()
        {
            StageLogicalObject.OnCharacterDamaged += OnCharacterDamaged;
            StageManager.OnPlayerReset += OnPlayerReset;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            m_sqrSpeed = characterMovementData.maxSpeed * characterMovementData.maxSpeed;
        }

        // Update is called once per frame
        private void Update()
        {
            animator.SetBool(IsGroundedHash, characterController3D.IsGrounded);

            //If we're in the air, we don't need to worry about setting the speed
            if (!characterController3D.IsGrounded)
                return;
            
            animator.SetFloat(SpeedHash, GetNormalizedSpeed());
        }
        
        private void OnDisable()
        {
            StageLogicalObject.OnCharacterDamaged -= OnCharacterDamaged;
            StageManager.OnPlayerReset -= OnPlayerReset;
        }

        //CharacterAnimationController Functions
        //============================================================================================================//
        
        private float GetNormalizedSpeed()
        {
            var velocity = characterRigidbody.linearVelocity;
            velocity.y = 0;
            return velocity.sqrMagnitude / m_sqrSpeed;
        }

        //Callbacks
        //================================================================================================================//

        private void OnCharacterDamaged()
        {
            animator.SetBool(IsDeadHash, true);
        }
        
        private void OnPlayerReset()
        {
            animator.SetBool(IsDeadHash, false);
        }
        
        
    }
}
