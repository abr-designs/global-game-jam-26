using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private StageController m_currentStage;
    [SerializeField] private Transform m_playerCharacter;

    private void Start()
    {
        if(m_playerCharacter == null)
        {
            Debug.LogError($"StageManager: No PlayerCharacter found");
            return;
        }

        if (m_currentStage == null)
            m_currentStage = FindFirstObjectByType<StageController>();

        if (m_currentStage == null)
        {
            Debug.LogError($"StageManager: No StageController found");
            return;
        }

        StartStage(m_currentStage);
    }

    private void StartStage(StageController stage)
    {
        m_currentStage = stage;

        if (m_currentStage.StageSpawnPoint == null)
        {
            Debug.LogError($"StageManager: Stage [{m_currentStage.name}] has no defined StageSpawnPoint");
            return;
        }

        if (m_currentStage.StageExitTrigger == null)
        {
            Debug.LogError($"StageManager: Stage [{m_currentStage.name}] has no defined StageExitTrigger");
            return;
        }

        m_playerCharacter.transform.SetPositionAndRotation(
            m_currentStage.StageSpawnPoint.transform.position,
            m_currentStage.StageSpawnPoint.transform.rotation);

        stage.StageExitTrigger.PlayerReachedExit += EndStage;
    }

    private void EndStage()
    {
        m_currentStage.StageExitTrigger.PlayerReachedExit -= EndStage;

        // restart
        StartStage(m_currentStage);
    }
}
