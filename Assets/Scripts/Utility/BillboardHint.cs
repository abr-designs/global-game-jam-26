using Samples.CharacterController3D.Scripts;
using UnityEngine;

public class BillboardHint : MonoBehaviour
{    
    // Offset in camera orientation (z into the screen)
    [SerializeField]
    private Vector3 offset = Vector3.zero;


    private static CharacterController3D s_characterController3D;
    private Transform m_cameraTransform;
    private Vector3 m_moveVelocity;
    private Vector3 m_rotateVelocity;
    void Start() {
        if(s_characterController3D == null)
            s_characterController3D = FindFirstObjectByType<CharacterController3D>(FindObjectsInactive.Exclude);
        m_cameraTransform = FindFirstObjectByType<Camera>(FindObjectsInactive.Exclude).transform;
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        SetPosition();
    }
    private void SetPosition()
    {   
        Vector3 worldOffset = m_cameraTransform.TransformDirection(offset);
        var targetPosition = s_characterController3D.transform.position + worldOffset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_moveVelocity, 0.2f);
        transform.forward = Vector3.SmoothDamp(transform.forward, m_cameraTransform.forward, ref m_rotateVelocity, 0.2f);
    }

}
