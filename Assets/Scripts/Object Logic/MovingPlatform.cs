using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector3 _rangeOfMotion;
    [SerializeField] private float _moveDuration = 3f;
    [SerializeField] private float _waitAtEnds = 2f;
    [SerializeField] private float _initialDelay = 0f;

    [Header("Gizmo")]
    [SerializeField] private Vector3 _objectBounds = new Vector3(4f, 1f, 4f);
    [SerializeField] private Color _gizmoColor = Color.cyan;

    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private void Start()
    {
        _startPosition = transform.position;
        _endPosition = _startPosition + _rangeOfMotion;

        StartCoroutine(MoveLoop());
    }

    private IEnumerator MoveLoop()
    {
        if (_initialDelay > 0f)
            yield return new WaitForSeconds(_initialDelay);

        while (true)
        {
            yield return Move(_startPosition, _endPosition);
            yield return new WaitForSeconds(_waitAtEnds);

            yield return Move(_endPosition, _startPosition);
            yield return new WaitForSeconds(_waitAtEnds);
        }
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / _moveDuration;
            transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }

        transform.position = to;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;

        Vector3 start = Application.isPlaying
            ? _startPosition
            : transform.position;

        Vector3 end = start + _rangeOfMotion;

        // Path extents
        Vector3 min = Vector3.Min(start, end);
        Vector3 max = Vector3.Max(start, end);

        // Expand by half the object bounds on all sides
        Vector3 halfBounds = _objectBounds * 0.5f;
        min -= halfBounds;
        max += halfBounds;

        Vector3 center = (min + max) * 0.5f;
        Vector3 size = max - min;

        // Ensure visibility for flat movement
        size.x = Mathf.Max(size.x, 0.05f);
        size.y = Mathf.Max(size.y, 0.05f);
        size.z = Mathf.Max(size.z, 0.05f);

        Gizmos.DrawWireCube(center, size);
    }
}
