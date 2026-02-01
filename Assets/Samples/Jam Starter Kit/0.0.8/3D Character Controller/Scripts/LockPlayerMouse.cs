using Audio;
using GameInput;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Samples.CharacterController3D.Scripts
{
    public class LockPlayerMouse : MonoBehaviour
    {
        [SerializeField]
        private CursorLockMode stateOnStart = CursorLockMode.Confined;

        private InputActions inputActions;
        private bool isOpen;

        private void Awake()
        {
            inputActions = new InputActions();
        }

        private void OnEnable()
        {
            inputActions.Menu.ToggleMenu.performed += OnToggleMenu;
            inputActions.Menu.Enable();
        }

        private void OnDisable()
        {
            inputActions.Menu.ToggleMenu.performed -= OnToggleMenu;
            inputActions.Menu.Disable();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            Cursor.lockState = stateOnStart;
            isOpen = false;
        }

        private void OnToggleMenu(InputAction.CallbackContext context)
        {
            ToggleMenu();
        }

        private void ToggleMenu()
        {
            if (isOpen)
                CloseWindow();
            else
                OpenWindow();
        }

        private void OpenWindow()
        {
            isOpen = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        public void CloseWindow()
        {
            isOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnCloseWindow()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
