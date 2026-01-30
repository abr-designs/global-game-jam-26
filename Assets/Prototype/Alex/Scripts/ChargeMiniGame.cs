using System;
using System.Diagnostics;
using Audio;
using GameInput;
using NaughtyAttributes;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
        
        public readonly struct MiniGameResults
        {
            public bool InSafeArea { get; }
            public bool InBonusArea { get; }

            public MiniGameResults(bool inSafeArea, bool inBonusArea)
            {
                InSafeArea = inSafeArea;
                InBonusArea = inBonusArea;
            }
            
        }
        //================================================================================================================//
        
        [Header("UI")]
        [SerializeField]
        private ChargeBarUI chargeBarUI;
        [SerializeField]
        private Canvas chargeUICanvas;
        
        [Header("Mini Game Behaviours")]
        [SerializeField]
        private float chargeSpeed;
        [SerializeField]
        private AnimationCurve chargeCurve;

        [SerializeField, Range(0f, 1f)]
        private float timeScaleDuring = 0.001f;

        [SerializeField, Range(0f, 1f)]
        private float failPunishImpact;
        
        [Header("Results Debug")]
        [SerializeField, ReadOnly]
        private bool inSafeArea;
        [SerializeField, ReadOnly]
        private bool inBonusArea;

        [SerializeField, ReadOnly]
        private DEBUG_STATE m_chargingState;

        private float ChargeValue => chargeCurve.Evaluate(m_value);
        private float m_value;
        private Action<MiniGameResults> m_onGameCompletedCallback;

        private bool m_isKeyPressed;

        //Unity Functions
        //================================================================================================================//

        private void OnEnable()
        {
            GameInputDelegator.OnJumpPressed += OnSpacePressed;
        }

        private void Start()
        {
            HideGame();
        }

        private void Update()
        {
            if (!chargeUICanvas.enabled)
                return;

            switch (m_chargingState)
            {
                case DEBUG_STATE.IDLE:
                    if (m_isKeyPressed)
                        m_chargingState = DEBUG_STATE.CHARGING;
                    return;
                case DEBUG_STATE.CHARGING:
                    m_value += Time.unscaledDeltaTime * chargeSpeed;
                    chargeBarUI.SetChargeValue(ChargeValue);

                    if (!m_isKeyPressed || m_value >= 1f)
                    {
                        chargeBarUI.SetChargeValue(ChargeValue);
                        inSafeArea = chargeBarUI.GetChargeSuccessState(ChargeValue, out inBonusArea);
                        m_chargingState = DEBUG_STATE.DONE;
                        
                        //TODO This will need to be smoothed out
                        if(!inSafeArea)
                            chargeBarUI.SetSafeAreaSize(chargeBarUI.SafeAreaSize - failPunishImpact);

                        m_onGameCompletedCallback?.Invoke(new MiniGameResults(inSafeArea, inBonusArea));
                    }
                    
                    return;
                case DEBUG_STATE.DONE:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        private void OnDisable()
        {
            GameInputDelegator.OnJumpPressed -= OnSpacePressed;
        }

        //Charge Mini Game Functions
        //================================================================================================================//

        [Button]
        [Conditional("UNITY_EDITOR")]
        private void DebugStartGame()
        {
            StartMiniGame(results =>
            {
                Debug.Log($"Game Completed. In Safe Area: {results.InSafeArea} | In Bonus Area: {results.InBonusArea}");
            });
        }
        
        public void StartMiniGame(Action<MiniGameResults> onGameCompleted)
        {
            m_onGameCompletedCallback = onGameCompleted;
            Time.timeScale = timeScaleDuring;
            chargeUICanvas.enabled = true;
            m_chargingState = DEBUG_STATE.IDLE;

            SFXManager.PlaySound(SFX.UI_BUTTON_CLICK);
        }

        public void HideGame()
        {
            chargeUICanvas.enabled = false;
            ResetValues();
            Time.timeScale = 1f;
        }

        private void ResetValues()
        {
            m_value = 0f;
            inSafeArea = false;
            inBonusArea = false;
            chargeBarUI.SetChargeValue(m_value);
        }

        //Callbacks
        //================================================================================================================//

        private void OnSpacePressed(bool pressed)
        {
            m_isKeyPressed = pressed;
        }
        
    }
}