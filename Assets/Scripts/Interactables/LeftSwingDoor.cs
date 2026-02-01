using System.Collections;
using UnityEngine;

public class LeftSwingDoor : MonoBehaviour
{
    [SerializeField] private float _doorOpenAngle = 110f;
    [SerializeField] private float _doorSwingDuration = 0.5f;

    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private bool _isOpen;

    private void Awake()
    {
        _closedRotation = transform.localRotation;
        _openRotation = _closedRotation * Quaternion.Euler(0f, _doorOpenAngle, 0f);
    }

    public void ToggleDoor()
    {
        StopAllCoroutines();
        StartCoroutine(SwingDoor(_isOpen ? _closedRotation : _openRotation));
        _isOpen = !_isOpen;
    }

    private IEnumerator SwingDoor(Quaternion targetRotation)
    {
        Quaternion startRotation = transform.localRotation;
        float elapsed = 0f;

        while (elapsed < _doorSwingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / _doorSwingDuration;

            transform.localRotation =
                Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        transform.localRotation = targetRotation;
    }
}