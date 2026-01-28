using System;
using System.Diagnostics;
using NaughtyAttributes;
using UnityEngine;

[ExecuteAlways]
public class ChargeBarUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform mainBarBackground;
    [SerializeField]
    private RectTransform safeAreaBar;
    [SerializeField]
    private RectTransform bonusAreaBar;
    [SerializeField]
    private RectTransform chargeBar;

    [SerializeField, Range(0f, 1f)]
    private float safeAreaSize = 0.8f;

    [SerializeField, Range(0f, 1f)]
    private float bonusBarPosition;
    
    [SerializeField, Min(10f)]
    private Vector2 bonusBarSize;

    private float Height => mainBarBackground.rect.height;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        ApplyTransforms();
    }

#if UNITY_EDITOR
    
    private void Update()
    {
        UpdateDebug();

        
    }
#endif
    
    //================================================================================================================//

    public void SetChargeValue(float value)
    {
        value = Math.Clamp(value, 0f, 1f);
        SetSize(chargeBar, 0f, Height * value);
    }

    public bool GetChargeState(float value, out bool hasBonus)
    {
        hasBonus = false;

        if (value < 0f || value > 1f)
            return false;

        if (IsWithinBounds(safeAreaBar, value))
            return true;

        if (IsWithinBounds(bonusAreaBar, value))
            hasBonus = true;

        return false;
    }

    //================================================================================================================//

    private bool IsWithinBounds(RectTransform rect, float value)
    {
        var height = value * Height;

        var localY = rect.localPosition.y;
        var halfRectHeight = rect.rect.height / 2f;

        return height >= localY - halfRectHeight && height <= localY + halfRectHeight;
    }

    //================================================================================================================//


    private void ApplyTransforms()
    {
        SetSize(safeAreaBar, 0f, -Height * (1f - safeAreaSize));
        
        SetSize(bonusAreaBar, bonusBarSize);
        bonusAreaBar.anchoredPosition = new Vector2(0, -Height * bonusBarPosition);
        SetSize(chargeBar, 0,0);
    }

    private void SetSize(RectTransform target, Vector2 size) => SetSize(target, size.x, size.y);
    private void SetSize(RectTransform target, float x, float y)
    {
        var size = target.sizeDelta;
        size.x = x;
        size.y = y;

        target.sizeDelta = size;
    }

    //Unity Editor Functions
    //================================================================================================================//

    [Conditional("UNITY_EDITOR")]
    private void UpdateDebug()
    {
        if (Application.isPlaying)
            return;
        
        SetSize(safeAreaBar, 0f, -Height * (1f - safeAreaSize));
        
        SetSize(bonusAreaBar, bonusBarSize);
        bonusAreaBar.anchoredPosition = new Vector2(0, -Height * bonusBarPosition);
    }

}
