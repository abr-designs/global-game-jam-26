using UnityEngine;

public class UiChargePointIndicator : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float hoverAmplitude = 0.25f;
    [SerializeField] private float hoverSpeed = 2f;

    private Transform target;
    private Transform cameraTransform;
    private Vector3 offset;

    public bool IsAssigned => target != null;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    public void Assign(Transform targetTransform, Color color, Vector3 worldOffset)
    {
        target = targetTransform;
        offset = worldOffset;
        spriteRenderer.color = color;
        gameObject.SetActive(true);
    }

    public void Unassign()
    {
        target = null;
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        float hover = Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;

        transform.position = target.position + offset + Vector3.up * hover;
        canvas.transform.forward = cameraTransform.forward;
    }
}
