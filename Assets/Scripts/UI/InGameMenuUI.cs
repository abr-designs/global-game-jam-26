using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;
using UnityEngine.InputSystem;
using GameInput;

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
        private Button quitButton;
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

#if UNITY_WEBGL
            quitButton.gameObject.SetActive(false);
#else
            quitButton.onClick.AddListener(OnQuitButtonPressed);
#endif

            //closeButton.onClick.AddListener(OnCloseButtonPressed);

            ScreenFader.FadeIn(1f, null);

            if (shouldStartOpen)
                OpenWindow();
            else
                CloseWindow();
        }   

        //============================================================================================================//
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

        //============================================================================================================//
        private void OnRestartButtonPressed()
        {
            ScreenFader.FadeOut(1f, () =>
            {
                RestartStage?.Invoke(); // TODO - need some delay while camera moves

                Debug.LogWarning("Completed restart?");
                CloseWindow();

                ScreenFader.FadeIn(null);
            });
        }
        
        private void OnSettingButtonPressed()
        {
            settingsWindow.OpenWindow();
        }

        private void OnQuitButtonPressed()
        {
            ExitStage?.Invoke();

            ScreenFader.FadeOut(1f, () =>
            {
                SceneManager.LoadScene(0);
            });
        }

        private void OnCloseButtonPressed()
        {
            CloseWindow();
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
