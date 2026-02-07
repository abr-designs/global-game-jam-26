using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using GameInput;
using UnityEngine.InputSystem;
using Samples.CharacterController3D.Scripts;

public class CharacterCameraLook : MonoBehaviour
{
    private CinemachineOrbitalFollow m_orbitalFollow;
    private CharacterController3D m_characterController;

    [SerializeField]
    private GameSettings settings;

    [SerializeField]
    private Vector2 maxSpeed = Vector2.one; // degrees per second 
    [SerializeField]
    private Vector2 gain = Vector2.one; // multiply input by this (good for inverting with negatives)
    [SerializeField]
    private Vector2 accelerationTime = new Vector2(0.1f, 0.1f); // how long in seconds to reach max speed
    [SerializeField]
    private Vector2 deadZone = Vector2.zero;

    [SerializeField]
    private float aimingFactor = 0.5f;

    [SerializeField]
    private bool UseInputBias; // If input is >90% in one direction, zero out the other

    private Vector2 currentVel = Vector2.zero;

    public static bool CameraInputLock { get; private set; }
    public static void SetCameraInputLock(bool lockState)
    {
        CameraInputLock = lockState;
    }

    void Awake()
    {
        m_orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
        m_characterController = FindFirstObjectByType<CharacterController3D>();
    }

    // Update is called once per frame
    void Update()
    {
        var input = GetCameraInput();
        bool isMouse = InputSystem.GetDevice<InputDevice>() is Pointer;

        float hDelta = 0.0f;
        float vDelta = 0.0f;

        // For mouse the input - 1.0 means 360 degrees instantly (no time involved)
        if (isMouse)
        {
            hDelta = input.x * maxSpeed.x * gain.x * settings.LookSensitivity;
            vDelta = input.y * maxSpeed.y * gain.y * settings.LookSensitivity;
        }
        else
        {
            // Gamepads / joysticks will accelerate over time

            float accelX = maxSpeed.x / Mathf.Max(accelerationTime.x, 0.001f);
            float accelY = maxSpeed.y / Mathf.Max(accelerationTime.y, 0.001f);

            Vector2 targetVel = new Vector2(
                input.x * maxSpeed.x * gain.x * settings.LookSensitivity,
                input.y * maxSpeed.y * gain.y * settings.LookSensitivity
            );
            currentVel.x = Mathf.MoveTowards(
                currentVel.x,
                targetVel.x,
                accelX * Time.deltaTime
            );
            currentVel.y = Mathf.MoveTowards(
                currentVel.y,
                targetVel.y,
                accelY * Time.deltaTime
            );
            hDelta = currentVel.x * Time.deltaTime;
            vDelta = currentVel.y * Time.deltaTime;
        }
        
        if(m_characterController.IsAiming)
        {
            hDelta *= aimingFactor;
            vDelta *= aimingFactor;
        }


        m_orbitalFollow.HorizontalAxis.Value = m_orbitalFollow.HorizontalAxis.ClampValue(m_orbitalFollow.HorizontalAxis.Value + hDelta);
        m_orbitalFollow.VerticalAxis.Value = m_orbitalFollow.VerticalAxis.ClampValue(m_orbitalFollow.VerticalAxis.Value + vDelta);
    }


    // Process and filter the input from the controls
    private Vector2 GetCameraInput()
    {
        if (CameraInputLock) return Vector2.zero;

        var rawInput = GameInputDelegator.GetCameraLookRaw();

        // Check for mouse
        InputDevice lastDevice = InputSystem.GetDevice<InputDevice>();
        if (lastDevice is Pointer)
        {
            // Normalize raw input based on screen size -- swiping whole screen should be value of 1
            rawInput = new Vector2(rawInput.x / Screen.width, rawInput.y / Screen.height);
            rawInput.x = Mathf.Clamp(rawInput.x, -1f, 1f);
            rawInput.y = Mathf.Clamp(rawInput.y, -1f, 1f);
        }

        if (Mathf.Abs(rawInput.x) < deadZone.x) rawInput.x = 0f;
        if (Mathf.Abs(rawInput.y) < deadZone.y) rawInput.y = 0f;

        if (UseInputBias)
        {
            var normalized = rawInput.normalized;
            float dotX = Mathf.Abs(Vector2.Dot(normalized, Vector2.right));
            float dotY = Mathf.Abs(Vector2.Dot(normalized, Vector2.up));

            // If movement is 90% in either axis, kill the secondary axis
            if (dotX > 0.9f) return new Vector2(rawInput.x, 0);
            if (dotY > 0.9f) return new Vector2(0, rawInput.y);
        }

        return rawInput;
    }

    [ContextMenu("Recenter")]
    public void Recenter(float time = 0.1f)
    {
        StartCoroutine(RecenterRoutine(time));
    }
    private IEnumerator RecenterRoutine(float time)
    {
        SetCameraInputLock(true);
        var oldSettings = m_orbitalFollow.HorizontalAxis.Recentering;
        var newSettings = new InputAxis.RecenteringSettings() { Enabled = true, Time = time, Wait = 0 };
        m_orbitalFollow.HorizontalAxis.Recentering = newSettings;
        yield return new WaitForSeconds(time);
        m_orbitalFollow.HorizontalAxis.Recentering = oldSettings;
        SetCameraInputLock(false);
    }



}
