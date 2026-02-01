using UnityEngine;

public class TestDoor : MonoBehaviour
{
    [SerializeField] private float _openedDoorHeight = 4f;

    public void OpenDoor()
    {
        Vector3 pos = transform.localPosition;
        pos.y = _openedDoorHeight;
        transform.localPosition = pos;
    }
}
