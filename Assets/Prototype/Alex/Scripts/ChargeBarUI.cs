using System;
using System.Diagnostics;
using UnityEngine;

[ExecuteAlways]
public class ChargeBarUI : MonoBehaviour
{
    public float SafeAreaSize => safeAreaSize;
    
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

    public bool GetChargeSuccessState(float value, out bool hasBonus)
    {
        hasBonus = false;

        if (value < 0f || value > 1f)
            return false;

        //Gets the top middle of the charge bar
        var chargeRect = GetScreenRect(chargeBar);
        var topPoint = new Vector3(chargeRect.center.x, chargeRect.yMax, 0f);

        //Check first for the Safe area, since currently, there is never a situation where the bonus bar is in there
        if (RectTransformOverlaps(safeAreaBar, topPoint))
            return true;

        //If we hit the bonus, technically we've missed the safe area so we will still return false
        if (RectTransformOverlaps(bonusAreaBar, topPoint))
            hasBonus = true;

        return false;
    }

    public void SetSafeAreaSize(float value)
    {
        value = Math.Clamp(value, 0f, 1f);
        SetSize(safeAreaBar, 0f, -Height * (1f - value));
        safeAreaSize = value;
    }

    //================================================================================================================//

    private static bool RectTransformOverlaps(RectTransform a, Vector3 worldPoint)
    {
        var rectA = GetScreenRect(a);
        
        var overlaps = rectA.Contains(worldPoint);

        return overlaps;
    }

    private static readonly Vector3[] Corners = new Vector3[4];
    private static Rect GetScreenRect(RectTransform rt)
    {
        rt.GetWorldCorners(Corners);

        var min = RectTransformUtility.WorldToScreenPoint(null, Corners[0]);
        var max = RectTransformUtility.WorldToScreenPoint(null, Corners[2]);

        return new Rect(min, max - min);
    }

    //================================================================================================================//


    private void ApplyTransforms()
    {
        SetSafeAreaSize(safeAreaSize);
        
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
        
        SetSafeAreaSize(safeAreaSize);
        
        SetSize(bonusAreaBar, bonusBarSize);
        bonusAreaBar.anchoredPosition = new Vector2(0, -Height * bonusBarPosition);
    }

}
