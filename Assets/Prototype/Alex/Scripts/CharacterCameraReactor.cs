using NaughtyAttributes;
using Samples.CharacterController3D.Scripts;
using Unity.Cinemachine;
using UnityEngine;

namespace Prototype.Alex.Scripts
{
    
    public class CharacterCameraReactor : MonoBehaviour
    {

        
        [SerializeField]
        private float maxSpeed;
        [SerializeField]
        private AnimationCurve transitionCurve;
        
        [SerializeField]
        private CharacterMovement3DDataScriptableObject moveData;
        
        [Header("Camera Components")]
        [SerializeField]
        private CinemachineCamera cinemachineCamera;
        [SerializeField]
        private CinemachineOrbitalFollow cinemachineOrbitalFollow;
        [SerializeField]
        private CinemachineRotationComposer cinemachineRotationComposer;

        [Header("Camera Motion Values")]
        [SerializeField]
        private float lensFovMin = 60;
        [SerializeField]
        private float lensFovMax = 80;

        [SerializeField]
        private Vector3 orbitalTargetOffsetMin = new Vector3(0,0,0);
        [SerializeField]
        private Vector3 orbitalTargetOffsetMax = new Vector3(0,1,0);

        [SerializeField]
        private float orbitalRadiusMin = 2.2f;
        [SerializeField]
        private float orbitalRadiusMax = 2f;
        
        [SerializeField]
        private Vector3 composerTargetOffsetMin = new Vector3(0,0.21f,0);
        [SerializeField]
        private Vector3 composerTargetOffsetMax = new Vector3(0, 0.5f, 0);

        private CharacterController3D m_characterController3D;
        [SerializeField, ReadOnly]
        private float m_lastT;
        
        //================================================================================================================//

        private void Start()
        {
            m_characterController3D = FindFirstObjectByType<CharacterController3D>(FindObjectsInactive.Exclude);
        }

        private void LateUpdate()
        {
            var t = transitionCurve.Evaluate(m_characterController3D.Speed / maxSpeed);

            TryUpdateCameraValues(t);
        }

        private void TryUpdateCameraValues(float t)
        {
            //if (!Mathf.Approximately(t, m_lastT))
            //    return;

            m_lastT = t;
            
            cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(lensFovMin, lensFovMax, t);
            cinemachineOrbitalFollow.TargetOffset = Vector3.Lerp(orbitalTargetOffsetMin, orbitalTargetOffsetMax, t);
            cinemachineOrbitalFollow.Radius = Mathf.Lerp(orbitalRadiusMin, orbitalRadiusMax, t);
            cinemachineRotationComposer.TargetOffset = Vector3.Lerp(composerTargetOffsetMin, composerTargetOffsetMax, t);
        }


#if UNITY_EDITOR

        [Button]
        private void AssignMinValues()
        {
            lensFovMin = cinemachineCamera.Lens.FieldOfView;
            orbitalTargetOffsetMin = cinemachineOrbitalFollow.TargetOffset;
            orbitalRadiusMin = cinemachineOrbitalFollow.Radius;
            composerTargetOffsetMax = cinemachineRotationComposer.TargetOffset;
            UnityEditor.EditorUtility.SetDirty(this);
        }
        [Button]
        private void AssignMaxValues()
        {
            lensFovMax = cinemachineCamera.Lens.FieldOfView;
            orbitalTargetOffsetMax = cinemachineOrbitalFollow.TargetOffset;
            orbitalRadiusMax = cinemachineOrbitalFollow.Radius;
            composerTargetOffsetMax = cinemachineRotationComposer.TargetOffset;
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
#endif
    }
}