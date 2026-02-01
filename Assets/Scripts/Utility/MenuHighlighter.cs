using Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class MenuHighlighter : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSystem;

    [SerializeField]
    private UnityEvent onPressed;

    [SerializeField]
    private TextMeshPro text;

    [SerializeField]
    private float fadeSpeed;
    [SerializeField]
    private AnimationCurve fadeCurve;
    [SerializeField]
    private Color startColor;
    [SerializeField]
    private Color endColor;
    
    private float m_currentLerp;
    private bool m_hovered;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        
        particleSystem.Stop();
    }

    private void Update()
    {
        m_currentLerp = Mathf.Clamp01(m_currentLerp += Time.deltaTime * fadeSpeed * (m_hovered ? 1f :  -1f));

        text.color = Color.Lerp(startColor, endColor, fadeCurve.Evaluate(m_currentLerp));
    }

    private void OnMouseEnter()
    {
        SFXManager.PlaySound(SFX.HOVER_WOOM);
        particleSystem.Play();
        m_hovered = true;
    }

    private void OnMouseDown()
    {
        onPressed?.Invoke();
    }

    private void OnMouseExit()
    {
        particleSystem.Stop();
        m_hovered = false;
    }
}
