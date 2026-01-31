using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Utility
{
    [RequireComponent(typeof(Camera))]
    public class CameraObjectHider : MonoBehaviour
    {
        private static List<CameraObject> s_cameraObjects;

        [SerializeField, Min(1)]
        private int checkFrequency = 1;

        [SerializeField, Range(-1f, 1f)]
        private float dotThreshold;

        private int m_checkCounter;

        public static void Register(CameraObject cameraObject)
        {
            s_cameraObjects ??= new List<CameraObject>();
            
            s_cameraObjects.Add(cameraObject);
        }
        public static void DeRegister(CameraObject cameraObject)
        {
            s_cameraObjects.Remove(cameraObject);
        }

        private void LateUpdate()
        {
            switch (checkFrequency)
            {
                case > 1 when m_checkCounter++ >= checkFrequency:
                    m_checkCounter = 0;
                    CheckAllObjects();
                    break;
                case 1:
                    CheckAllObjects();
                    break;
            }
        }

        private void CheckAllObjects()
        {
            var camForward = transform.forward.normalized;
            for (int i = s_cameraObjects.Count - 1; i >= 0; i--)
            {
                var otherDir = s_cameraObjects[i].Direction.normalized;
                var dot = Vector3.Dot(camForward, otherDir);

                
                s_cameraObjects[i].SetVisible(dot > dotThreshold);
            }
        }
    }
}