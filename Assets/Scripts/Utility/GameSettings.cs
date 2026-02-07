using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings")]
public class GameSettings : ScriptableObject
{

    public float lookSensitivity = 0.5f;
    public float lookMin = 0.1f;
    public float lookMax = 3f;

    // Return the multiplier based on the range
    public float LookSensitivity => (lookSensitivity * ((lookMax - lookMin) + lookMin));

    // Call this to save to disk
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("LookSensitivity", lookSensitivity);
        PlayerPrefs.Save();
    }

    // Call this on game startup
    public void LoadSettings()
    {
        lookSensitivity = Mathf.Clamp01(PlayerPrefs.GetFloat("LookSensitivity", 0.5f)); // 0.5 is default
    }
}