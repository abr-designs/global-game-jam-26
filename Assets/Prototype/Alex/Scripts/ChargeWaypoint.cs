using System;
using System.Collections;
using Samples.CharacterController3D.Scripts;
using UnityEngine;
using Utilities.Debugging;

namespace Prototype.Alex.Scripts
{
    public class ChargeWaypoint : MonoBehaviour
    {
        private static ChargeMiniGame s_chargeMiniGame;
        private static Transform s_playerTransform;

        [SerializeField, Min(0f)]
        private float useRadius;
        private bool m_activated;

        [SerializeField, Min(0f)]
        private float postGameWaitTime = 1f;


        private void Start()
        {
            s_chargeMiniGame ??= FindFirstObjectByType<ChargeMiniGame>();
            s_playerTransform ??= FindFirstObjectByType<CharacterController3D>().transform;
        }

        private void Update()
        {
            if (m_activated)
                return;
            
            //FIXME If there are a bunch of points this won't scale well
            if (Vector3.Distance(transform.position, s_playerTransform.position) > useRadius)
                return;

            m_activated = true;

            StartCoroutine(MiniGameStartCoroutine());
        }

        private IEnumerator MiniGameStartCoroutine()
        {
            bool gameDone = false;
            s_chargeMiniGame.StartMiniGame(_ =>
            {
                gameDone = true;
            });

            yield return new WaitUntil(() => gameDone);

            if(postGameWaitTime > 0)
                yield return new WaitForSecondsRealtime(1f);
            
            s_chargeMiniGame.HideGame();
        }
        
        
        
        //================================================================================================================//

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var position = transform.position;
            
            Draw.Circle(position, Vector3.up, Color.yellow, useRadius);
        }
#endif
    }
}