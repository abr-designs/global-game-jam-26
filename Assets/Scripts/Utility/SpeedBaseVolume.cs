using System;
using Samples.CharacterController3D.Scripts;
using UnityEngine;

namespace GGJ.Utility
{
    public class SpeedBaseVolume : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody characterRigidbody;
        [SerializeField]
        private CharacterMovement3DDataScriptableObject characterMovementData;

        [SerializeField, Range(0f, 1f)]
        private float maxVolume = 0.8f;

        [SerializeField]
        private AudioSource footStepsAudioSource;

        [SerializeField]
        private AnimationCurve curve;

        private float m_target_T;
        private float m_currentT;
        private float m_currentVelocity;
        
        private float m_sqrSpeed;
        
        private void Start()
        {
            m_sqrSpeed = characterMovementData.maxSpeed * characterMovementData.maxSpeed;
        }

        private void LateUpdate()
        {
            m_target_T = GetNormalizedSpeed();

            if (m_target_T == 0f)
            {
                m_currentT = 0f;
                m_currentVelocity = 0f;
            }
            
            m_currentT = Mathf.SmoothDamp(m_currentT, m_target_T, ref m_currentVelocity, 0.3f);
            footStepsAudioSource.volume = curve.Evaluate(m_currentT) * maxVolume;
        }

        private float GetNormalizedSpeed()
        {
            var velocity = characterRigidbody.linearVelocity;
            velocity.y = 0;
            return velocity.sqrMagnitude / m_sqrSpeed;
        }
        
    }
}