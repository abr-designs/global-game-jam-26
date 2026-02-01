using UnityEngine;
using Utilities.WaitForAnimations;
using Utilities.WaitForAnimations.Base;

namespace GGJ.Utility
{
    public class WaitForMoveSimple : WaitForMoveAnimations
    {
        private ANIM_DIR m_animDir = ANIM_DIR.START_TO_END;
        //============================================================================================================//

        public void Animate()
        {
            DoAnimation(0.5f, m_animDir);

            m_animDir = m_animDir == ANIM_DIR.START_TO_END ? ANIM_DIR.END_TO_START : ANIM_DIR.START_TO_END;
        }
        
        protected override void SetValue(AnimationData data, Vector3 value)
        {
            data.transform.localPosition = value;
        }

    }
}