using UnityEngine;
using Utilities.WaitForAnimations.Base;

namespace GGJ.Utility
{
    public class WaitForMoveRotation : WaitForAnimationBase<Transform, Vector3>
    {
        private ANIM_DIR m_animDir = ANIM_DIR.START_TO_END;
        //============================================================================================================//

        public void Animate()
        {
            DoAnimation(0.5f, m_animDir);

            m_animDir = m_animDir == ANIM_DIR.START_TO_END ? ANIM_DIR.END_TO_START : ANIM_DIR.START_TO_END;
        }
        
        public override Coroutine DoAnimation(float time, ANIM_DIR animDir)
        {
            return StartCoroutine(DoAnimationCoroutine(time, animDir));
        }

        protected override Vector3 Lerp(Vector3 start, Vector3 end, float t)
        {
            return Vector3.Lerp(start, end, t);
        }

        protected override void SetValue(AnimationData data, Vector3 value)
        {
            data.transform.localRotation = Quaternion.Euler(value);
        }
    }
}