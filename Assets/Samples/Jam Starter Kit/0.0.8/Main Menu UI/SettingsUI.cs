using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class SettingsUI : BaseUIWindow
    {
        [SerializeField]
        private GameSettings gameSettings;

        [SerializeField, Header("Volume Settings")] 
        private VolumeSettings masterVolume;
        [SerializeField] 
        private VolumeSettings musicVolume;
        [SerializeField] 
        private VolumeSettings sfxVolume;
        [SerializeField]
        private VolumeSettings lookSpeed; // TODO -- make volumesettings more generic

        private float MasterVolume => 0.75f;//TODO Get from a player pref or global location
        private float MusicVolume => 1f;//TODO Get from a player pref or global location
        private float SFXVolume => 1f;//TODO Get from a player pref or global location
        

        //Unity Functions
        //============================================================================================================//

        private void OnEnable()
        {
            gameSettings.LoadSettings();

            masterVolume.Init(OnMasterVolumeChanged, MasterVolume);
            musicVolume.Init(OnMusicVolumeChanged, MusicVolume);
            sfxVolume.Init(OnSFXVolumeChanged, SFXVolume);
            lookSpeed.Init(OnLookSpeedChanged, gameSettings.lookSensitivity);
            
        }

        private void OnDisable()
        {
            masterVolume.DeInit();
            musicVolume.DeInit();
            sfxVolume.DeInit();
            
            lookSpeed.DeInit();
            gameSettings.SaveSettings();

        }

        //Set Volume Functions
        //============================================================================================================//

        private void OnMasterVolumeChanged(float value)
        {
            Debug.LogError("MUST CONNECT MASTER VOLUME.\nChange no Saved...");
        }
        private void OnMusicVolumeChanged(float value)
        {
            Debug.LogError("MUST MUSIC MASTER VOLUME.\nChange no Saved...");
        }
        private void OnSFXVolumeChanged(float value)
        {
            Debug.LogError("MUST SFX MASTER VOLUME.\nChange no Saved...");
        }

        private void OnLookSpeedChanged(float value)
        {
            gameSettings.lookSensitivity = value;
        }

        //Volume Settings Class
        //============================================================================================================//

        #region Volume Settings Class

        [Serializable]
        private class VolumeSettings
        {
            //If you want the Volume settings to be displayed in a reorderable list, you may want to enable the name
            //[SerializeField]
            //private string name;
            [SerializeField] private Slider slider;
            [SerializeField] private TMP_Text text;

            [SerializeField, Range(0f, 1f)] private float startingValue = 1f;
            private UnityAction<float> _callback;

            private void FormatText(float value)
            {
                text.text = $"{value * 100:0}%";
            }

            public void Init(UnityAction<float> callback, float overrideValue = -1)
            {
                Assert.IsNotNull(slider);
                
                slider.wholeNumbers = false;
                slider.minValue = 0f;
                slider.maxValue = 1f;
                _callback = callback;
                slider.onValueChanged.AddListener(_callback);

                if (text != null)
                    slider.onValueChanged.AddListener(FormatText);

                var newValue = overrideValue >= 0f ? overrideValue : startingValue;
                
                slider.SetValueWithoutNotify(newValue);
                FormatText(newValue);
            }

            public void DeInit()
            {
                slider.onValueChanged.RemoveListener(_callback);
                _callback = null;
                if (text != null)
                    slider.onValueChanged.RemoveListener(FormatText);
            }
        }

        #endregion //Volume Settings Class

        //============================================================================================================//

    }
}