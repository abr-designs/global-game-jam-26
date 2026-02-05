using System.Collections.Generic;
using System.Linq;
using GGJ.Player;
using GGJ.Player.Enums;
using Samples.CharacterController3D.Scripts;
using TMPro;
using UnityEngine;
using Utilities.Debugging;

public class TriggerHint : MonoBehaviour
{
    private static CharacterController3D s_characterController3D;
    private Transform m_cameraTransform;

    private List<SpriteRenderer> m_sprites;
    private List<TMP_Text> m_text;

    [SerializeField]
    private MASK_TYPE requiredMask = MASK_TYPE.NONE;
    [SerializeField]
    private float triggerDistance = 1.0f;

    [SerializeField]
    private float fadeSpeed;
    [SerializeField]
    private AnimationCurve fadeCurve;
    private float m_currentFade;
    private Color m_color;

    [SerializeField]
    private GameObject particleEffect;

    private bool m_isTriggered = false;

    void Start()
    {
        s_characterController3D ??= FindFirstObjectByType<CharacterController3D>(FindObjectsInactive.Exclude);
        m_cameraTransform = FindFirstObjectByType<Camera>(FindObjectsInactive.Exclude).transform;
        m_sprites = GetComponentsInChildren<SpriteRenderer>().ToList();
        m_text = GetComponentsInChildren<TMP_Text>().ToList();
        m_color = m_sprites[0].color;
        particleEffect.SetActive(false);
        UpdateFade(0f);
    }
    // Update is called once per frame
    private void LateUpdate()
    {

        if (m_isTriggered)
        {
            // Update fade
            if (m_currentFade < 1f)
            {
                m_currentFade += fadeSpeed * Time.deltaTime;
                UpdateFade(m_currentFade);
            }
            return;
        }

        var pos = s_characterController3D.transform.position;
        var dir = pos - transform.position;
        var dist = dir.magnitude;

        if (dist <= triggerDistance && PlayerMaskManager.CurrentlyEquippedMask == requiredMask)
        {
            Trigger();
        }
    }

    private void Trigger()
    {
        Debug.Log("Triggered!");
        m_isTriggered = true;
        particleEffect.SetActive(true);
    }

    private void UpdateFade(float t)
    {
        m_color.a = fadeCurve.Evaluate(t);
        for (int i = 0; i < m_sprites.Count; i++)
        {
            m_sprites[i].color = m_color;
        }
        for (int i = 0; i < m_text.Count; i++)
        {
            m_text[i].color = m_color;
        }
    }

    private void OnDrawGizmos()
    {
        var position = transform.position;
        Gizmos.DrawWireSphere(position, triggerDistance);
    }

}
