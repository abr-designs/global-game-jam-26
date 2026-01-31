using GGJ.Player.Enums;
using GGJ.Player.Interfaces;
using UnityEngine;

namespace GGJ.Player
{
    public class CharacterShootAbility : MonoBehaviour, IAbility
    {
        public MASK_TYPE MaskType => MASK_TYPE.SHOOT;
        
        public void UseAbility()
        {
            throw new System.NotImplementedException();
        }
    }
}