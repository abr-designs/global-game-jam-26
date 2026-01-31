using UnityEngine;

namespace Interactables
{
    public interface IInteractable
    {
        float InteractionDistance { get; }
        Transform transform { get; }
        
        void OnEnable();
        void OnDisable();
        
        void Interact();
    }
}