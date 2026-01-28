using System;
using NaughtyAttributes;
using UnityEngine;

namespace Prototype.Alex.Scripts
{
    public class ChargeMiniGame : MonoBehaviour
    {
        private enum DEBUG_STATE
        {
            IDLE,
            CHARGING,
            DONE
        }
        [SerializeField]
        private ChargeBarUI chargeBarUI;
        
        [SerializeField]
        private float chargeSpeed;
        [SerializeField]
        private AnimationCurve chargeCurve;
        
        [SerializeField, ReadOnly]
        private bool failed;
        [SerializeField, ReadOnly]
        private bool bonus;

        [SerializeField]
        private KeyCode toggle = KeyCode.Space;

        private DEBUG_STATE state;
        private float m_value;
        
        
        private void Start()
        {
            
        }

        private void Update()
        {
            var keyDown = Input.GetKeyDown(toggle);

            switch (state)
            {
                case DEBUG_STATE.IDLE:
                    if (keyDown)
                        state = DEBUG_STATE.CHARGING;
                    return;
                case DEBUG_STATE.CHARGING:
                    m_value += Time.deltaTime * chargeSpeed;
                    chargeBarUI.SetChargeValue(m_value);

                    if (keyDown || m_value >= 1f)
                    {
                        chargeBarUI.SetChargeValue(m_value);
                        failed = chargeBarUI.GetChargeState(m_value, out bonus);
                        state = DEBUG_STATE.DONE;
                    }
                    
                    return;
                case DEBUG_STATE.DONE:

                    if (keyDown)
                    {
                        m_value = 0f;
                        failed = false;
                        bonus = false;
                        chargeBarUI.SetChargeValue(m_value);
                        state = DEBUG_STATE.IDLE;
                    }
                    
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
        
        
    }
}