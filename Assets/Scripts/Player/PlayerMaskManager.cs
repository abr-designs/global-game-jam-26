using System.Diagnostics;
using System.Linq;
using GameInput;
using GGJ.Player.Enums;
using GGJ.Player.Interfaces;
using Interactables;
using UnityEngine;
using Utilities;

namespace GGJ.Player
{
    public class PlayerMaskManager : HiddenSingleton<PlayerMaskManager>
    {
        public static MASK_TYPE CurrentlyEquippedMask { get; private set; } = MASK_TYPE.NONE;
        
        [SerializeReference] 
        private MonoBehaviour[] abilities;
        private IAbility[] m_abilities;
        
        [SerializeField]
        private GameObject[] maskObjects = new GameObject[3];

        //Unity Functions
        //================================================================================================================//

        private void OnEnable()
        {
            GameInputDelegator.OnJumpPressed += OnJumpPressed;
            StageManager.OnPlayerReset += OnPlayerReset;
        }

        private void Start()
        {
            m_abilities = new IAbility[abilities.Length];
            for (var i = 0; i < abilities.Length; i++)
            {
                m_abilities[i] = abilities[i] as IAbility;
            }

            SetMaskVisual(CurrentlyEquippedMask);
        }
        
        private void OnDisable()
        {
            GameInputDelegator.OnJumpPressed -= OnJumpPressed;
            StageManager.OnPlayerReset -= OnPlayerReset;
        }

        //================================================================================================================//

        public static void EquipMask(MASK_TYPE maskType)
        {
            CurrentlyEquippedMask = maskType;

            Instance.SetMaskVisual(CurrentlyEquippedMask);
        }

        public static MASK_TYPE UnEquipMask(bool updateVisual)
        {
            var mask = CurrentlyEquippedMask;
            CurrentlyEquippedMask = MASK_TYPE.NONE;
            
            if(updateVisual)
                Instance.SetMaskVisual(CurrentlyEquippedMask);
            
            return mask;
        }
        
        private void SetMaskVisual(MASK_TYPE maskType)
        {
            //SetParticles(maskType);
            
            for (int i = 1; i < 3; i++)
            {
                maskObjects[i].SetActive(i == (int)maskType);
            }
        }

        //Functions
        //================================================================================================================//

        private void UseAbility()
        {
            if (CurrentlyEquippedMask == MASK_TYPE.NONE)
                return;

            if (InteractableManager.InteractablesInRange)
                return;
            
            m_abilities.FirstOrDefault(x => x.MaskType == CurrentlyEquippedMask)?
                .UseAbility();
        }
        
        //Callbacks
        //================================================================================================================//
        
        private void OnJumpPressed(bool pressed)
        {
            if (!pressed)
                return;
            
            UseAbility();
        }
        
        private void OnPlayerReset()
        {
            EquipMask(MASK_TYPE.NONE);
        }

    }
}