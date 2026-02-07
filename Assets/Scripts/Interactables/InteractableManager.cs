using System;
using System.Collections.Generic;
using GameInput;
using Samples.CharacterController3D.Scripts;
using UnityEngine;
using Utilities.Debugging;

namespace Interactables
{
    public class InteractableManager : MonoBehaviour
    {
        public static bool InteractablesInRange { get; private set; }
        private static List<IInteractable> s_interactables;

        private int m_inRangeCount;
        private IInteractable[] m_interactablesInRange;

        [SerializeField]
        private CharacterController3D characterController3D;

        [SerializeField]
        private float interactionRange;

        [SerializeField]
        private float dotThreshold;

        [SerializeField, Min(1)]
        private int checkFrequency = 1;

        private int m_currentCheckCount;

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

        //Unity Functions
        //================================================================================================================//

        private void OnEnable()
        {
            GameInputDelegator.OnJumpPressed += OnJumpPressed;
        }

        private void Start()
        {
            m_interactablesInRange = new IInteractable[10];
        }

        private void Update()
        {
            if (checkFrequency > 1 && m_currentCheckCount++ >= checkFrequency)
                UpdateInteractablesInRange();
        }

        private void OnDisable()
        {
            GameInputDelegator.OnJumpPressed -= OnJumpPressed;
        }

        //================================================================================================================//

        private void UpdateInteractablesInRange()
        {
            if (s_interactables == null || s_interactables.Count < 0)
                return;

            var playerFacingDirection =
                Vector3.ProjectOnPlane(characterController3D.transform.forward.normalized, Vector3.up);
            var playerPosition = characterController3D.transform.position;

            m_inRangeCount = 0;
            for (int i = 0; i < s_interactables.Count; i++)
            {
                var interactable = s_interactables[i];
                var pos = interactable.transform.position;
                var dir = pos - playerPosition;

                if (dir.magnitude > interactionRange + interactable.InteractionDistance)
                    continue;

                var flatDir = Vector3.ProjectOnPlane(dir.normalized, Vector3.up);

                //Determine if the interactable is in front of the player
                //TODO This might be optional
                if (Vector3.Dot(playerFacingDirection, flatDir) < dotThreshold)
                    continue;

                //TODO Might need to determine if there is a wall between the player & the object

                m_interactablesInRange[m_inRangeCount++] = interactable;
            }

            InteractablesInRange = m_inRangeCount > 0;
        }

        private void TryUseInteractable()
        {
            if (m_inRangeCount == 0)
                return;

            var playerPosition = characterController3D.transform.position;

            var foundIndex = -1;
            var shortestDist = float.MaxValue;

            for (int i = 0; i < m_inRangeCount; i++)
            {
                var interactable = m_interactablesInRange[i];
                var pos = interactable.transform.position;
                var dir = pos - playerPosition;
                var dist = dir.sqrMagnitude;
                if (dist >= shortestDist)
                    continue;

                shortestDist = dist;
                foundIndex = i;
            }

            if (foundIndex < 0)
                return;

            m_interactablesInRange[foundIndex].Interact();
        }

        //Callbacks
        //================================================================================================================//

        private void OnJumpPressed(bool pressed)
        {
            if (!pressed)
                return;

            TryUseInteractable();
        }

        //Unity Editor Functions
        //================================================================================================================//

        private void OnDrawGizmos()
        {
            var pos = characterController3D.transform.position;

            Draw.Circle(pos, Vector3.up, Color.white, interactionRange);

            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.green;
            for (int i = 0; i < m_inRangeCount; i++)
            {
                var interactable = m_interactablesInRange[i];
                if (interactable == null)
                    continue;

                Gizmos.DrawLine(pos, interactable.transform.position);
            }
        }
        //================================================================================================================//

    }
}