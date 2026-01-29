using UnityEngine;

public class UiChargePointIndicatorOverlayCanvas : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float positionOffset = 10f; // pixels above object
    [SerializeField] private float hoverAmplitude = 10f; // pixels
    [SerializeField] private float hoverSpeed = 2f;
    [SerializeField] private float screenMargin = 20f; // pixels from edge

    private Transform target;
    private Camera cam;

    public bool IsAssigned => target != null;
    [SerializeField] private bool ShowOffscreen;

    public void Assign(Transform targetTransform, Color color, Camera camera = null)
    {
        target = targetTransform;
        cam = camera ?? Camera.main;
        rectTransform.GetComponent<UnityEngine.UI.Image>().color = color;
        gameObject.SetActive(true);
    }

    public void Unassign()
    {
        target = null;
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (target == null || cam == null) return;

        // World position to screen
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);

        // Hover
        screenPos.y += positionOffset + Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;

        bool isBehind = screenPos.z < 0f;

        if (isBehind)
        {
            // Flip to opposite side
            screenPos.x = Screen.width - screenPos.x;
            screenPos.y = Screen.height - screenPos.y;
        }

        bool isOutsideView = screenPos.x < 0f || screenPos.x > Screen.width;

        // Clamp to screen edges
        float clampedX = Mathf.Clamp(screenPos.x, screenMargin, Screen.width - screenMargin);
        float clampedY = Mathf.Clamp(screenPos.y, screenMargin, Screen.height - screenMargin);

        // Determine if we are clamped horizontally
        bool clampedLeft = clampedX <= screenMargin + 0.01f;
        bool clampedRight = clampedX >= Screen.width - screenMargin - 0.01f;

        // Apply position
        rectTransform.position = new Vector3(clampedX, clampedY, 0f);

        if (ShowOffscreen)
        {
            // Rotation logic
            if (isBehind)
            {
                rectTransform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            else if (clampedLeft)
            {
                rectTransform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            else if (clampedRight)
            {
                rectTransform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }
            else
            {
                rectTransform.rotation = Quaternion.identity;
            }
        }

        // If offscreen indicators are disabled, hide the object and return
        if (!ShowOffscreen && (isBehind || isOutsideView))
        {
            rectTransform.gameObject.SetActive(false);
            return;
        }
        else
        {
            rectTransform.gameObject.SetActive(true);
        }
    }
}
