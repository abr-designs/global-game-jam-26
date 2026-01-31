using System.Linq;
using GameInput;
using GGJ.Player.Enums;
using GGJ.Player.Interfaces;
using UnityEngine;

namespace GGJ.Player
{
    public class MaskEmulator : MonoBehaviour, IAbility
    {
        public MASK_TYPE MaskType => maskType;
        
        [SerializeField] 
        private MASK_TYPE maskType = MASK_TYPE.NONE;

        [SerializeReference] 
        private MonoBehaviour[] abilities;
        private IAbility[] m_abilities;

        //Unity Functions
        //================================================================================================================//

        private void OnEnable()
        {
            GameInputDelegator.OnJumpPressed += OnJumpPressed;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            m_abilities = new IAbility[abilities.Length];
            for (var i = 0; i < abilities.Length; i++)
            {
                m_abilities[i] = abilities[i] as IAbility;
            }
        }

        private void OnDisable()
        {
            GameInputDelegator.OnJumpPressed -= OnJumpPressed;
        }

        //Functions
        //================================================================================================================//

        public void UseAbility()
        {
            m_abilities.FirstOrDefault(x => x.MaskType == MaskType)?
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
    }
}
