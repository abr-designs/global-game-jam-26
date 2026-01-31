using System;
using System.Collections.Generic;
using Samples.CharacterController3D.Scripts;
using UnityEngine;
using Utilities.Debugging;

namespace Interactables
{
    public class InteractableManager : MonoBehaviour
    {
        private static List<IInteractable> s_interactables;

        [SerializeField]
        private CharacterController3D characterController3D;

        [SerializeField]
        private float interactionRange;

        [SerializeField]
        private float dotThreshold;

        [SerializeField, Min(1)]
        private int checkFrequency;

        //================================================================================================================//

        public static void Register(IInteractable interactable)
        {
            s_interactables ??= new List<IInteractable>();
            
            s_interactables.Add(interactable);
        }

        public static void DeRegister(IInteractable interactable)
        {
            s_interactables.Remove(interactable);
        }

        private void Update()
        {
            
        }

        private void CheckForInteractables()
        {
            
        }

        private void OnDrawGizmos()
        {
            var pos = characterController3D.transform.position;
            
            Draw.Circle(pos, Vector3.up, Color.white, interactionRange);
        }
    }
}