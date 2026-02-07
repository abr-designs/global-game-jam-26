using System;
using Samples.CharacterController3D.Scripts;
using UnityEngine;
using Utilities.Debugging;

public class InteractionHint : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private Color m_color;

    [SerializeField]
    private float displayDistance;
    [SerializeField]
    private float triggerDistance;
    
    [SerializeField]
    private float fadeSpeed;
    [SerializeField]
    private AnimationCurve fadeCurve;
    private float m_currentFade;

    private Vector3 m_moveVelocity;
    
    private static CharacterController3D s_characterController3D;
    private Transform m_cameraTransform;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if(s_characterController3D == null) s_characterController3D = FindFirstObjectByType<CharacterController3D>(FindObjectsInactive.Exclude);
        m_cameraTransform = FindFirstObjectByType<Camera>(FindObjectsInactive.Exclude).transform;
        m_color = spriteRenderer.color;
        UpdateFade(0f);
        
        SetPosition(s_characterController3D.transform.position, true);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var pos = s_characterController3D.transform.position;
        var dir = pos - transform.position;
        var dist = dir.magnitude;

        if (dist > triggerDistance)
        {
            if (m_currentFade > 0f)
            {
                m_currentFade -= fadeSpeed * Time.deltaTime * 2f;
                UpdateFade(m_currentFade);
            }
            
            return;
        }

        if (m_currentFade < 1f)
        {
            m_currentFade += fadeSpeed * Time.deltaTime;
            UpdateFade(m_currentFade);
        }

        SetPosition(pos);
    }

    private void SetPosition(Vector3 otherPosition, bool forceSet = false)
    {
        var dir = otherPosition - transform.position;
        
        var flatDirection = Vector3.ProjectOnPlane(dir.normalized, Vector3.up);
        var targetPosition = transform.position + (flatDirection * displayDistance);

        if (forceSet)
        {
            spriteRenderer.transform.position = targetPosition;
            return;
        }
        
        spriteRenderer.transform.position = Vector3.SmoothDamp(spriteRenderer.transform.position, targetPosition, ref m_moveVelocity, 0.5f);

        spriteRenderer.transform.forward = m_cameraTransform.forward;
    }

    private void UpdateFade(float t)
    {
        m_color.a = fadeCurve.Evaluate(t);
        spriteRenderer.color = m_color;
    }

    private void OnDrawGizmos()
    {
        var position = transform.position;
        Draw.Circle(position, Vector3.up, Color.yellow, triggerDistance);
        Draw.Circle(position, Vector3.up, Color.green, displayDistance);
    }
}
