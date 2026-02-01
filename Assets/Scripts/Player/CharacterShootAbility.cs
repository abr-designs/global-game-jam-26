using System.Collections.Generic;
using Audio;
using GGJ.Player.Enums;
using GGJ.Player.Interfaces;
using Projectiles;
using Samples.CharacterController3D.Scripts;
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

        //Unity Functions
        //================================================================================================================//

        private void Start()
        {
            m_activeProjectiles ??= new List<ICustomUpdate>();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;

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


        public void UseAbility()
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