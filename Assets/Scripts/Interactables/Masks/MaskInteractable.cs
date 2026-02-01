using System;
using Audio;
using GGJ.Player;
using GGJ.Player.Enums;
using UnityEngine;
using Utilities.Debugging;

namespace Interactables.Masks
{
    public class MaskInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private MASK_TYPE currentMaskType;

        [SerializeField]
        private GameObject[] maskObjects = new GameObject[3];
        
        public float InteractionDistance => interactionDistance;
        
        [SerializeField, Min(0.1f)]
        private float interactionDistance = 0.1f;

        [SerializeField]
        private ParticleSystem particleSystem;
        [SerializeField]
        private Gradient[] maskGradients;
        
        public void OnEnable()
        {
            InteractableManager.Register(this);
        }

        private void Start()
        {
            SetMaskVisual(currentMaskType);
        }

        public void OnDisable()
        {
            InteractableManager.DeRegister(this);
        }

        public void Interact()
        {
            if (PlayerMaskManager.CurrentlyEquippedMask == MASK_TYPE.NONE && currentMaskType == MASK_TYPE.NONE)
                return;

            SFXManager.PlaySoundAtLocation(SFX.PICKUP_OBJECT, transform.position);

            if (PlayerMaskManager.CurrentlyEquippedMask == MASK_TYPE.NONE)
            {
                //Move my mask onto the player
                PlayerMaskManager.EquipMask(currentMaskType);
                currentMaskType = MASK_TYPE.NONE;
            }
            else if (PlayerMaskManager.CurrentlyEquippedMask != MASK_TYPE.NONE && currentMaskType == MASK_TYPE.NONE)
            {
                currentMaskType = PlayerMaskManager.UnEquipMask(true);
            }
            else if (PlayerMaskManager.CurrentlyEquippedMask != MASK_TYPE.NONE && currentMaskType != MASK_TYPE.NONE)
            {
                var playersOldMask = PlayerMaskManager.UnEquipMask(false);
                var myPreviousMask = currentMaskType;

                currentMaskType = playersOldMask;
                PlayerMaskManager.EquipMask(myPreviousMask);
            }

            SetMaskVisual(currentMaskType);
        }

        private void SetMaskVisual(MASK_TYPE maskType)
        {
            SetParticles(maskType);
            
            for (int i = 1; i < 3; i++)
            {
                maskObjects[i].SetActive(i == (int)maskType);
            }
        }

        private void SetParticles(MASK_TYPE maskType)
        {
            if (particleSystem == null)
                return;
            
            if(maskType == MASK_TYPE.NONE)
                particleSystem.Stop();
            else
            {
                var mainModule = particleSystem.main;
                var startColor = mainModule.startColor;
                startColor.mode = ParticleSystemGradientMode.RandomColor;
                startColor.gradient = maskGradients[(int)maskType];

                mainModule.startColor = startColor;
                particleSystem?.Play();
            }
        }
        
        private void OnDrawGizmos()
        {
            Draw.Circle(transform.position, Vector3.up, Color.white, InteractionDistance);
        }
    }
}