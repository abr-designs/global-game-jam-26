using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;
using UnityEngine.InputSystem;
using GameInput;
using Audio;
using UnityUtils;

namespace UI
{
    public class InGameMenuUI : MonoBehaviour
    {
        [SerializeField, Header("Window Settings")]
        private bool shouldStartOpen;

        [Header("In Game Menu")]
        [SerializeField]
        private Button restartButton;
        [SerializeField]
        private Button settingsButton;
        [SerializeField]
        private Button retireButton;
        //[SerializeField]
        //private Button closeButton;

        [SerializeField, Header("Windows")] 
        private BaseUIWindow settingsWindow;

        [SerializeField]
        private InGameMenuWindowUI inGameMenuWindow;

        private InputActions inputActions;
        private bool isOpen;

        public event Action RestartStage;
        public event Action ExitStage;

        //============================================================================================================//
        private void Awake()
        {
            inputActions = new InputActions();
        }

        private void OnEnable()
        {
            inputActions.Menu.ToggleMenu.performed += OnToggleMenu;
            inputActions.Menu.Enable();

            inGameMenuWindow.closeWindow += OnCloseWindow;
        }

        private void OnDisable()
        {
            inputActions.Menu.ToggleMenu.performed -= OnToggleMenu;
            inputActions.Menu.Disable();

            inGameMenuWindow.closeWindow -= OnCloseWindow;
        }


        // Start is called before the first frame update
        private void Start()
        {
            Assert.IsNotNull(settingsWindow);

            ScreenFader.ForceSetColorBlack();
            restartButton.onClick.AddListener(OnRestartButtonPressed);

            settingsButton.onClick.AddListener(OnSettingButtonPressed);

            retireButton.onClick.AddListener(OnRetireButtonPressed);

            //closeButton.onClick.AddListener(OnCloseButtonPressed);

            ScreenFader.FadeIn(1f, null);

            if (shouldStartOpen)
                OpenWindow();
            else
            {
                isOpen = false;
                inGameMenuWindow.gameObject.SetActive(false);
            }
        }   

        //============================================================================================================//
        private void OnToggleMenu(InputAction.CallbackContext context)
        {
            ToggleMenu();
        }

        private void ToggleMenu()
        {
            SFXManager.PlaySound(SFX.UI_BUTTON_CLICK);

            if (isOpen)
                CloseWindow();
            else
                OpenWindow();
        }

        //============================================================================================================//
        private void OnRestartButtonPressed()
        {
            SFXManager.PlaySound(SFX.UI_BUTTON_CLICK);

            RestartStage?.Invoke(); // TODO - need some delay while camera moves

            CloseWindow();
        }
        
        private void OnSettingButtonPressed()
        {
            SFXManager.PlaySound(SFX.UI_BUTTON_CLICK);

            settingsWindow.OpenWindow();
        }

        private void OnRetireButtonPressed()
        {
            SFXManager.PlaySound(SFX.UI_BUTTON_CLICK);

            ExitStage?.Invoke();

            ScreenFader.FadeOut(1f, () =>
            {
                SceneManager.LoadScene(0);
            });
        }

        //============================================================================================================//

        private void OpenWindow()
        {
            isOpen = true;
            inGameMenuWindow.OpenWindow();
        }

        private void CloseWindow()
        {
            isOpen = false;
            inGameMenuWindow.CloseWindow();
        }

        private void OnCloseWindow()
        {
            isOpen = false;
        }
    }
}
