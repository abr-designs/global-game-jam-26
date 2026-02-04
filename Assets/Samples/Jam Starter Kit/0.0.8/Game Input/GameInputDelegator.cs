using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameInput
{
    public class GameInputDelegator : MonoBehaviour, InputActions.IGameplayActions
    {
        public static event Action<bool> InputLockChanged;
        public static event Action<Vector2> OnMovementChanged;
        public static event Action<Vector2> OnCameraLookChanged;
        public static event Action<bool> OnJumpPressed;

        public static event Action<bool> OnLeftClick;
        public static event Action<bool> OnRightClick;

        public static bool LockInputs { get; private set; }

        private Vector2 _currentInput;
        private Vector2 _cameraLookInput;
        //============================================================================================================//\

        private void OnEnable()
        {
            Inputs.Input.Gameplay.Enable();
            Inputs.Input.Gameplay.AddCallbacks(this);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        private void OnDisable()
        {
            Inputs.Input.Gameplay.Disable();
            Inputs.Input.Gameplay.RemoveCallbacks(null);
        }

        //Lock Input
        //============================================================================================================//

        public static void SetInputLock(bool lockState)
        {
            LockInputs = lockState;

            InputLockChanged?.Invoke(lockState);
        }

        //============================================================================================================//

        public void OnMovement(InputAction.CallbackContext context)
        {
            if (LockInputs)
            {
                _currentInput = Vector2.zero;
                OnMovementChanged?.Invoke(_currentInput);
                return;
            }

            _currentInput = context.ReadValue<Vector2>();
            OnMovementChanged?.Invoke(_currentInput);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (LockInputs)
                return;

            Debug.Log($"Jump event -- performed:({context.performed}) -- canceled:({context.canceled}) -- value:({context.ReadValueAsButton()}) ");

            if (!context.performed && !context.canceled)
                return;

            var pressed = context.ReadValueAsButton();
            OnJumpPressed?.Invoke(pressed);
        }

        public void OnMouseLeftClick(InputAction.CallbackContext context)
        {
            if (LockInputs)
            {
                OnLeftClick?.Invoke(false);
                return;
            }

            var pressed = context.ReadValueAsButton();
            OnLeftClick?.Invoke(pressed);
        }

        public void OnMouseRightClick(InputAction.CallbackContext context)
        {
            if (LockInputs)
            {
                OnRightClick?.Invoke(false);
                return;
            }

            var pressed = context.ReadValueAsButton();
            OnRightClick?.Invoke(pressed);
        }

        public void OnCameraLook(InputAction.CallbackContext context)
        {
            if (LockInputs)
            {
                _cameraLookInput = Vector2.zero;
                OnCameraLookChanged?.Invoke(_cameraLookInput);
                return;
            }

            _cameraLookInput = context.ReadValue<Vector2>();
            OnCameraLookChanged?.Invoke(_cameraLookInput);
        }

        public static Vector2 GetCameraLookRaw()
        {
            // We read the action directly from the instance the Delegator enabled
            return Inputs.Input.Gameplay.CameraLook.ReadValue<Vector2>();
        }

        //============================================================================================================//
    }
}
