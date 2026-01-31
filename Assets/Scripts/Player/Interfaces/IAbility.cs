using GGJ.Player.Enums;
using Samples.CharacterController3D.Scripts;

namespace GGJ.Player.Interfaces
{
    public interface IAbility
    {
        public static CharacterController3D CharacterController3D;

        MASK_TYPE MaskType { get; }
        
        void UseAbility();
    }
}