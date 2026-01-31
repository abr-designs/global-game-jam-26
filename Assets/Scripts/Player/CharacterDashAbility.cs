using GGJ.Player.Enums;
using GGJ.Player.Interfaces;
using Samples.CharacterController3D.Scripts;
using UnityEngine;

namespace GGJ.Player
{
    public class CharacterDashAbility : MonoBehaviour, IAbility
    {
        public MASK_TYPE MaskType => MASK_TYPE.DASH;
        
        [SerializeField]
        private CharacterMovement3DDataScriptableObject moveData;

        private void Start()
        {
            IAbility.CharacterController3D ??= FindFirstObjectByType<CharacterController3D>(FindObjectsInactive.Exclude);
        }

        public void UseAbility()
        {
            
        }
    }
}