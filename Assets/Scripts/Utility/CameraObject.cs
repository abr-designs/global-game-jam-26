using UnityEngine;
using Utilities.Debugging;

namespace GGJ.Utility
{
    public class CameraObject : MonoBehaviour
    {
        public Vector3 Direction => transform.TransformVector(hideWhenLocalDirection);
        
        [SerializeField]
        private Renderer renderer;
        [SerializeField]
        private Vector3 hideWhenLocalDirection;

        private bool m_isVisible = true;

        private void OnEnable()
        {
            CameraObjectHider.Register(this);
        }

        private void OnDisable()
        {
            CameraObjectHider.DeRegister(this);
        }

        public void SetVisible(bool state)
        {
            //Don't try and set the same value
            if (state == m_isVisible)
                return;
            
            renderer.enabled = state;
            m_isVisible = state;
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Draw.Arrow(renderer.bounds.center, Direction, Color.blue);
        }

#endif
    }
}