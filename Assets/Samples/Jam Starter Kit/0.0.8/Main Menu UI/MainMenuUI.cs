using System;
using Audio;
using Audio.SoundFX;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        //============================================================================================================//

        [SerializeField]
        private CinemachineCamera beginCamera;
        [SerializeField]
        private CinemachineCamera menuCamera;
        
        // Start is called before the first frame update
        private void Start()
        {
            ScreenFader.FadeIn(2f, null);
            menuCamera.Priority = -100;
        }

        private void Update()
        {
            if (menuCamera.Priority < 0 && Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
                menuCamera.Priority = 100;
            else if(menuCamera.Priority > 0 && Input.GetKeyDown(KeyCode.Escape))
                menuCamera.Priority = -100;
        }

        //============================================================================================================//
        
        public void OnPlayButtonPressed()
        {
            SFXManager.PlaySound(SFX.UI_BUTTON_CLICK);

            ScreenFader.FadeOut(1f, () =>
            {
                SceneManager.LoadScene(1);
            });
        }

        public void OnQuitButtonPressed()
        {
            SFXManager.PlaySound(SFX.UI_BUTTON_CLICK);

            Application.Quit();
        }
        
        //============================================================================================================//
    }
}
