using System.Collections;
using UnityEngine;

public class TestDoor : MonoBehaviour
{
    [SerializeField] private float _openedDoorHeight = 4f;
    [SerializeField] private float _doorOpenAngle = 110f;
    [SerializeField] private float _doorSwingDuration = 0.5f;

    private bool _openedDoor;

    public void OpenDoor()
    {
        if (_openedDoor)
            return;

        _openedDoor = true;
        StartCoroutine(SwingDoor());
    }

    private IEnumerator SwingDoor()
    {
        float elapsed = 0f;

        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation =
            startRotation * Quaternion.Euler(0f, _doorOpenAngle, 0f);

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
