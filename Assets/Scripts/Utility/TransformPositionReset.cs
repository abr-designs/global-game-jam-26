using UnityEngine;

namespace GGJ.Utility
{
    public class TransformPositionReset : MonoBehaviour
    {
        [SerializeField]
        private Transform targetTransform;

        [SerializeField]
        private KeyCode resetKey = KeyCode.R; 

        private Vector3 m_startWorldPosition;
        private Quaternion m_startWorldRotation;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            targetTransform.position = targetTransform.position;
            targetTransform.rotation = targetTransform.rotation;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!Input.GetKeyDown(resetKey))
                return;

            ResetTransform();
        }

        private void ResetTransform()
        {
            targetTransform.position = targetTransform.position;
            targetTransform.rotation = targetTransform.rotation;
        }
    }
}
