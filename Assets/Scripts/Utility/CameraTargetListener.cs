using System;
using Unity.Cinemachine;
using UnityEngine;

namespace GGJ.Utility
{
    public class CameraTargetListener : MonoBehaviour
    {
        [SerializeField] 
        private CinemachineCamera cinemachineCamera;

        private void OnEnable()
        {
            StageManager.OnNewCameraTarget += OnNewCameraTarget;
        }

        private void OnDisable()
        {
            StageManager.OnNewCameraTarget -= OnNewCameraTarget;
        }

        private void OnNewCameraTarget(Transform target)
        {
            cinemachineCamera.Follow = target;
        }
    }
}