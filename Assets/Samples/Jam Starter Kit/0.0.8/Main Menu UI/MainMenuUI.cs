using Audio;
using Audio.SoundFX;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Main Menu")]
        [SerializeField]
        private Button playButton;
        [SerializeField]
        private Button settingsButton;
        [SerializeField]
        private Button quitButton;

        [SerializeField, Header("Windows")] 
        private BaseUIWindow settingsWindow;
        //============================================================================================================//
        
        // Start is called before the first frame update
        private void Start()
        {
            Assert.IsNotNull(settingsWindow);
            
            ScreenFader.ForceSetColorBlack();
            playButton.onClick.AddListener(OnPlayButtonPressed);
            
            settingsButton.onClick.AddListener(OnSettingButtonPressed);

#if UNITY_WEBGL
            quitButton.gameObject.SetActive(false);
#else
            quitButton.onClick.AddListener(OnQuitButtonPressed);
#endif

            ScreenFader.FadeIn(1f, null);
        }

        //============================================================================================================//
        
        private void OnPlayButtonPressed()
        {
            SFXManager.PlaySound(SFX.UI_BUTTON_CLICK);

            ScreenFader.FadeOut(1f, () =>
            {
                SceneManager.LoadScene(1);
            });
        }
        
        private void OnSettingButtonPressed()
        {
            SFXManager.PlaySound(SFX.UI_BUTTON_CLICK);

            settingsWindow.OpenWindow();
        }

        private void OnQuitButtonPressed()
        {
            SFXManager.PlaySound(SFX.UI_BUTTON_CLICK);

            Application.Quit();
        }
        
        //============================================================================================================//
    }
}
