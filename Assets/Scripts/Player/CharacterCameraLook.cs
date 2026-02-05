using System.Collections;
using GameInput;
using Unity.Cinemachine;
using Unity.Cinemachine.TargetTracking;
using UnityEngine;

public class CharacterCameraLook : MonoBehaviour
{
    CinemachineOrbitalFollow m_orbitalFollow;
    // Current value

    [SerializeField]
    private GameSettings settings;

    [SerializeField]
    private Vector2 maxSpeed = Vector2.one; // degrees per second 
    [SerializeField]
    private Vector2 gain = Vector2.one; // multiply input by this
    [SerializeField]
    private Vector2 accelerationTime = new Vector2(0.2f, 0.2f); // how long in seconds to reach max speed
    [SerializeField]
    private float deadZone = 0f;

    [SerializeField]
    private bool clampSpeed = false;


    private Vector2 currentVel = Vector2.zero;

    public static bool CameraInputLock { get; private set; }
    public static void SetCameraInputLock(bool lockState)
    {
        CameraInputLock = lockState;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        var input = GetCameraInput();

        float accelX = maxSpeed.x / Mathf.Max(accelerationTime.x, 0.01f);
        float accelY = maxSpeed.y / Mathf.Max(accelerationTime.y, 0.01f);


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

        var hDelta = currentVel.x * Time.deltaTime;
        var vDelta = currentVel.y * Time.deltaTime;
        m_orbitalFollow.HorizontalAxis.Value = m_orbitalFollow.HorizontalAxis.ClampValue(m_orbitalFollow.HorizontalAxis.Value + hDelta);
        m_orbitalFollow.VerticalAxis.Value = m_orbitalFollow.VerticalAxis.ClampValue(m_orbitalFollow.VerticalAxis.Value + vDelta);
    }


    // Process and filter the input from the controls
    private Vector2 GetCameraInput()
    {
        if (CameraInputLock) return Vector2.zero;

        var rawInput = GameInputDelegator.GetCameraLookRaw();

        // Clamp mouse values to -1 to 1 range
        if (clampSpeed)
        {
            rawInput.x = Mathf.Clamp(rawInput.x, -1f, 1f);
            rawInput.y = Mathf.Clamp(rawInput.y, -1f, 1f);
        }

        float mag = rawInput.magnitude;
        if (mag < deadZone) return Vector2.zero;

        var normalized = rawInput.normalized;
        float dotX = Mathf.Abs(Vector2.Dot(normalized, Vector2.right));
        float dotY = Mathf.Abs(Vector2.Dot(normalized, Vector2.up));

        // If movement is 90% in either axis, kill the secondary axis
        if (dotX > 0.9f) return new Vector2(rawInput.x, 0);
        if (dotY > 0.9f) return new Vector2(0, rawInput.y);

        return rawInput;
    }

    [ContextMenu("Recenter")]
    public void Recenter(float time = 0.1f)
    {
        IEnumerator RecenterRoutine()
        {
            SetCameraInputLock(true);
            var oldSettings = m_orbitalFollow.HorizontalAxis.Recentering;
            var newSettings = new InputAxis.RecenteringSettings() { Enabled = true, Time = time, Wait = 0};
            m_orbitalFollow.HorizontalAxis.Recentering = newSettings;
            yield return new WaitForSeconds(time);
            m_orbitalFollow.HorizontalAxis.Recentering = oldSettings;
            SetCameraInputLock(false);
        }

        StartCoroutine(RecenterRoutine());
    }


}
