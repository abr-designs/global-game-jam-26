using System;
using GameInput;
using Samples.CharacterController3D.Scripts;
using UnityEngine;

namespace Prototype.Alex.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class LineAttachJoint : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rb;
        [SerializeField]
        private Collider playerCollider;

        [SerializeField, Range(0f, 1f)]
        private float dotThreshold = 0.5f;

        [SerializeField]
        private CharacterController3D characterController3D;

        private ConfigurableJoint m_joint;

        private bool m_waitToReattach;

        [SerializeField]
        private Transform pointATransform;
        [SerializeField]
        private Transform pointBTransform;

        private void Start()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_waitToReattach)
                return;

            if (other.gameObject != rb.gameObject)
                return;

            TryAttach();
        }

        private void Update()
        {
            if(m_waitToReattach && Input.GetKeyDown(KeyCode.Space))
                Detach();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!m_waitToReattach)
                return;
            
            Detach();
            m_waitToReattach = false;
        }

        private void TryAttach()
        {
            Vector3 closest = ClosestPointOnLine(pointATransform.position, pointBTransform.position, rb.position);

            //Check if collider waist is above the attach point
            if (playerCollider.bounds.center.y < closest.y)
                return;
            
            Vector3 worldDir = (pointATransform.position - pointBTransform.position).normalized;

            var dot = Math.Abs(Vector3.Dot(rb.linearVelocity.normalized, worldDir));
            if (dot < dotThreshold)
                return;
            
            m_waitToReattach = true;
            AttachToLine(pointATransform.position, pointBTransform.position);
        }


        private void AttachToLine(Vector3 pointA, Vector3 pointB)
        {
            Detach();

            characterController3D.isGrinding = true;
            Vector3 worldDir = (pointB - pointA).normalized;
            Vector3 closest = ClosestPointOnLine(pointA, pointB, rb.position);

            var dif = rb.position - closest;

            m_joint = rb.gameObject.AddComponent<ConfigurableJoint>();
            m_joint.connectedBody = null; // world

            m_joint.autoConfigureConnectedAnchor = false;
            m_joint.anchor = Vector3.zero;

            var desiredAnchor = closest + dif;
            desiredAnchor.y = Mathf.Max(desiredAnchor.y, closest.y + playerCollider.bounds.extents.y);
            
            m_joint.connectedAnchor = desiredAnchor;

            // Axis setup (THIS is the key)
            m_joint.axis = rb.transform.InverseTransformDirection(worldDir);
            m_joint.secondaryAxis = Vector3.up;

            // Allow movement only along axis
            m_joint.xMotion = ConfigurableJointMotion.Free;
            m_joint.yMotion = ConfigurableJointMotion.Locked;
            m_joint.zMotion = ConfigurableJointMotion.Locked;

            // No rotation
            m_joint.angularXMotion = ConfigurableJointMotion.Locked;
            m_joint.angularYMotion = ConfigurableJointMotion.Locked;
            m_joint.angularZMotion = ConfigurableJointMotion.Locked;

            m_joint.projectionMode = JointProjectionMode.PositionAndRotation;
        }

        public void Detach()
        {
            if (m_joint)
            {
                Destroy(m_joint);
                characterController3D.isGrinding = false;
            }
        }

        private static Vector3 ClosestPointOnLine(Vector3 a, Vector3 b, Vector3 p)
        {
            Vector3 ab = b - a;
            float t = Vector3.Dot(p - a, ab) / ab.sqrMagnitude;
            t = Mathf.Clamp01(t);
            return a + ab * t;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pointATransform.position, pointBTransform.position);
            
            Gizmos.color = Color.white;
            
            Gizmos.DrawWireSphere(pointATransform.position, 0.4f);
            Gizmos.DrawWireSphere(pointBTransform.position, 0.4f);
        }
    }
}