using Levels;
using UI;
using UnityEngine;
using Utilities;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Transform m_stageContainer;
    [SerializeField] private Transform m_playerCharacter;
    [SerializeField] private StageController m_currentStage;

    [Header("UI")]
    [SerializeField] private InGameMenuUI m_inGameMenu;

    private void OnEnable()
    {
        m_inGameMenu.RestartStage += RestartStage;
    }
    private void OnDisable()
    {
        m_inGameMenu.RestartStage += RestartStage;
    }

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

        StartGame();
    }

    private void StartGame()
    {
        // empty stage container
        DestroyAllChildren(m_stageContainer);

        // load level data
        LevelLoader.LoadFirstLevel();
        LevelDataDefinition firstStageData = ((StageController)LevelLoader.CurrentLevelDataDefinition);
        
        LoadStage(firstStageData);
    }

    private void LoadStage(LevelDataDefinition stageData)
    {
        GameObject newStageObject = Instantiate(stageData.gameObject, m_stageContainer);
        m_currentStage = newStageObject.GetComponent<StageController>();
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
            m_currentStage.StageSpawnPoint.position,
            m_currentStage.StageSpawnPoint.transform.rotation);

        stage.StageExitTrigger.PlayerReachedExit += EndStage;

        ScreenFader.FadeIn(1f, null);
    }

    private void RestartStage()
    {
        m_playerCharacter.transform.SetPositionAndRotation(
            m_currentStage.StageSpawnPoint.position,
            m_currentStage.StageSpawnPoint.transform.rotation);
    }

    private void EndStage()
    {
        m_currentStage.StageExitTrigger.PlayerReachedExit -= EndStage;

        ScreenFader.FadeOut(1f, () =>
        {
            // destroy previous stage
            Destroy(m_currentStage.gameObject);

            // advance to next stage
            LevelLoader.LoadNextLevel();
            LevelDataDefinition nextStageData = ((StageController)LevelLoader.CurrentLevelDataDefinition);
            LoadStage(nextStageData);
        });
    }

    // ---------- HELPERS ---------- //
    private void DestroyAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(parent.GetChild(i).gameObject);
        }
    }
}
