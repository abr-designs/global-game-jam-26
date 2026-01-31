using System;
using GameInput;
using Samples.CharacterController3D.Scripts;
using UnityEngine;
using Utilities.Debugging;

public class Grapple : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    private ConfigurableJoint m_joint;

    [SerializeField]
    private CharacterController3D characterController3D;

    [SerializeField] private float interactionDistance;
    private float m_interactionDistanceSqr;

    private bool m_mousePressed;

    //Unity Functions
    //================================================================================================================//

    private void OnEnable()
    {
        GameInputDelegator.OnLeftClick += OnLeftClick;
    }

    private void Start()
    {
        m_interactionDistanceSqr = interactionDistance * interactionDistance;
    }

    private void OnDisable()
    {
        GameInputDelegator.OnLeftClick -= OnLeftClick;
    }

    //================================================================================================================//

    private void TryAttach()
    {
        var pos = rb.position;
        var vector = Vector3.ProjectOnPlane(transform.position - pos, Vector3.up);

        //If we're too far, ignore
        if (vector.sqrMagnitude > m_interactionDistanceSqr)
            return;

        if (m_joint != null)
            return;
        
        Attach(transform.position);
        characterController3D.isGrappled = true;
    }


    private void Attach(Vector3 anchorPoint)
    {
        m_joint = rb.gameObject.AddComponent<ConfigurableJoint>();
        m_joint.autoConfigureConnectedAnchor = false;
        m_joint.connectedAnchor = anchorPoint;
        m_joint.connectedBody = null; // world

        // Anchor at center of mass
        m_joint.anchor = Vector3.zero;

        // Lock linear motion
        m_joint.xMotion = ConfigurableJointMotion.Limited;
        m_joint.yMotion = ConfigurableJointMotion.Limited;
        m_joint.zMotion = ConfigurableJointMotion.Limited;

        float distance = Vector3.Distance(rb.position, anchorPoint);

        SoftJointLimit limit = m_joint.linearLimit;
        limit.limit = distance;
        m_joint.linearLimit = limit;

        // Allow free rotation (swing!)
        m_joint.angularXMotion = ConfigurableJointMotion.Free;
        m_joint.angularYMotion = ConfigurableJointMotion.Free;
        m_joint.angularZMotion = ConfigurableJointMotion.Free;

        m_joint.enableCollision = false;

        // Stability tweaks
        m_joint.projectionMode = JointProjectionMode.PositionAndRotation;
        m_joint.projectionDistance = 0.1f;

        m_joint.linearLimitSpring = new SoftJointLimitSpring()
        {
            spring = 12f,
            damper = 1f
        };
        

        //----------------------------------------------------------//

        m_joint.massScale = 1f;
        m_joint.connectedMassScale = 1f;

        JointDrive drive = new JointDrive
        {
            positionSpring = 0f,
            positionDamper = 0f,
            maximumForce = Mathf.Infinity
        };

        m_joint.xDrive = drive;
        m_joint.yDrive = drive;
        m_joint.zDrive = drive;
    }

    private void Detach()
    {
        if (m_joint)
        {
            Destroy(m_joint);
            characterController3D.isGrappled = false;
        }
    }
    //================================================================================================================//

    
    private void OnLeftClick(bool pressed)
    {
        if (!m_mousePressed && pressed)
        {
            TryAttach();
        }
        else if(m_mousePressed && !pressed)
        {
            Detach();
        }

        m_mousePressed = pressed;
    }

    private void OnDrawGizmos()
    {
        Draw.Circle(transform.position, Vector3.up, Color.yellow, interactionDistance);

        if (!Application.isPlaying)
            return;

        if (m_joint == null)
            return;
        
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, rb.position);
    }
}