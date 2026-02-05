using System.Collections.Generic;
using Audio;
using GameInput;
using GGJ.Player.Enums;
using GGJ.Player.Interfaces;
using Projectiles;
using Samples.CharacterController3D.Scripts;
using Unity.Cinemachine;
using UnityEngine;
using VisualFX;

namespace GGJ.Player
{
    public class CharacterShootAbility : MonoBehaviour, IAbility
    {
        public MASK_TYPE MaskType => MASK_TYPE.SHOOT;

        [SerializeField]
        private Projectile projectilePrefab;

        [SerializeField]
        private CharacterMovement3DDataScriptableObject characterMovement3DData;

        [SerializeField, Min(0f)]
        private float fireCooldown;
        private float m_coolDown;

        [SerializeField, Min(0)]
        private int maxActiveProjectiles;
        private List<ICustomUpdate> m_activeProjectiles;

        [Header("Aim Mode Settings")]
        [SerializeField]
        private GameObject aimVFX;
        
        [SerializeField]
        private CinemachineCamera cinemachineCamera;
        private CinemachineOrbitalFollow m_orbitalFollow;
        

        [SerializeField]
        private float aimingCameraDistance;
        private float m_targetCameraRadialAxis;
        private float m_originalCameraRadialAxis;
        private float m_cameraRadialVelocity;


        //Unity Functions
        //================================================================================================================//

        private void Start()
        {
            IAbility.CharacterController3D ??= FindFirstObjectByType<CharacterController3D>(FindObjectsInactive.Exclude);
            m_orbitalFollow = cinemachineCamera.GetComponent<CinemachineOrbitalFollow>();
            m_targetCameraRadialAxis = m_originalCameraRadialAxis = m_orbitalFollow.RadialAxis.Value;
            m_activeProjectiles ??= new List<ICustomUpdate>();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;

            m_orbitalFollow.RadialAxis.Value = Mathf.SmoothDamp(m_orbitalFollow.RadialAxis.Value, m_targetCameraRadialAxis, ref m_cameraRadialVelocity, 0.2f);
            
            if(m_isButtonPressed)
            {
                m_buttonHeldTimer += deltaTime;
                if(m_buttonHeldTimer >= buttonHeldThreshold)
                {
                    toggleAim(true);
                }
            } else
            {
                toggleAim(false);
            }

            if (fireCooldown > 0f && m_coolDown > 0f)
                m_coolDown -= deltaTime;
            
            if (m_activeProjectiles == null || m_activeProjectiles.Count == 0)
                return;
            
            for (int i = m_activeProjectiles.Count - 1; i >= 0; i--)
            {
                if(!m_activeProjectiles[i].CustomUpdate(deltaTime))
                    m_activeProjectiles.RemoveAt(i);
            }
        }
        
        //================================================================================================================//

        private bool m_isButtonPressed = false;
        [SerializeField]
        private float m_buttonHeldTimer = 0f;
        [SerializeField]
        private float buttonHeldThreshold = 0.5f;
        public void UseAbility(bool buttonPressed)
        {
            bool buttonReleased = m_isButtonPressed && !buttonPressed;
            m_isButtonPressed = buttonPressed;
            
            if(buttonReleased)
            {
                m_buttonHeldTimer = 0f;
                activateAbility();
            }
        }

        private void toggleAim(bool state)
        {
            IAbility.CharacterController3D.ToggleAim(state);

            // TODO -- maybe scale to appear?
            if(aimVFX.activeSelf != state)
            {                
                m_targetCameraRadialAxis = state ? aimingCameraDistance : m_originalCameraRadialAxis;
                aimVFX.SetActive(state);
                // CharacterCameraLook.SetCameraInputLock(show);
            }
        }

        private void activateAbility()
        {
            if (fireCooldown > 0f && m_coolDown > 0f)
                return;
            
            //Cannot shoot because of the current limits
            if (maxActiveProjectiles > 0 && m_activeProjectiles.Count >= maxActiveProjectiles)
                return;

            m_coolDown = fireCooldown;

            var characterControllerTransform = IAbility.CharacterController3D.transform;
            var speed = characterMovement3DData.maxSpeed * 2f;
            var startPosition = characterControllerTransform.position + characterControllerTransform.forward.normalized * 0.5f;
            
            var projectile = Instantiate(projectilePrefab, startPosition, Quaternion.identity);
            projectile.Launch(speed, characterControllerTransform.forward.normalized);
            
            m_activeProjectiles.Add(projectile);
            
            SFXManager.PlaySound(SFX.PROJECTILE);
            VFX.BOUNCE.PlayAtLocation(startPosition);
        }

    }
}