using Audio;
using Levels;
using Samples.CharacterController3D.Scripts;
using System.Collections.Generic;
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

    [Header("Runtime")]
    private List<StageLogicalObject> stageLogicalObjects = new List<StageLogicalObject>(); // may no long er be used
    private bool stageIsRestarting;

    private void OnEnable()
    {
        m_inGameMenu.RestartStage += RestartStage;
        m_inGameMenu.ExitStage += BreakdownCurrentStage;
    }
    private void OnDisable()
    {
        m_inGameMenu.RestartStage -= RestartStage;
        m_inGameMenu.ExitStage -= BreakdownCurrentStage;
    }

    private void Awake()
    {
        ValidateLevels();
    }

    private void Start()
    {
        if(m_playerCharacter == null)
        {
            Debug.LogError($"StageManager: No PlayerCharacter found");
            return;
        }

        //if (m_currentStage == null)
        //    m_currentStage = FindFirstObjectByType<StageController>();

        //if (m_currentStage == null)
        //{
        //    Debug.LogError($"StageManager: No StageController found");
        //    return;
        //}

        StartGame();
    }

    private void StartGame()
    {
        // empty stage container
        DestroyAllChildren(m_stageContainer);

        // load level data
        LevelLoader.LoadFirstLevel();
        StageController startingStageData = ((StageController)LevelLoader.CurrentLevelDataDefinition);
        
        LoadStage(startingStageData);
    }

    private void LoadStage(StageController stageController)
    {
        m_currentStage = stageController;

        LoadStageLogicalObjects();
        StartStage(m_currentStage);
    }

    private void LoadStageLogicalObjects()
    {
        stageLogicalObjects.Clear();

        StageLogicalObject.CharacterDamaged += LoseStage;
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

        //m_playerCharacter.transform.SetPositionAndRotation(
        //    m_currentStage.StageSpawnPoint.position,
        //    m_currentStage.StageSpawnPoint.transform.rotation);

        m_playerCharacter.transform.position = m_currentStage.StageSpawnPoint.position;

        m_playerCharacter.GetComponent<Character3DBalancer>()?.FaceDirection(m_currentStage.StageSpawnPoint.transform.forward.normalized);

        stage.StageExitTrigger.PlayerReachedExit += EndStage;

        //SFXManager.PlaySound(SFX.PICKUP_OBJECT);

        ScreenFader.FadeIn(1f, null);

        stageIsRestarting = false;
    }

    private void EndStage()
    {
        ScreenFader.FadeOut(1f, () =>
        {
            SFXManager.PlaySound(SFX.PICKUP_OBJECT);

            BreakdownCurrentStage();

            // advance to next stage
            LevelLoader.LoadNextLevel();
            StageController nextStageData = ((StageController)LevelLoader.CurrentLevelDataDefinition);
            LoadStage(nextStageData);
        });
    }

    private void BreakdownCurrentStage()
    {
        m_currentStage.StageExitTrigger.PlayerReachedExit -= EndStage;

        // destroy existing stage
        Destroy(m_currentStage.gameObject);

        StageLogicalObject.CharacterDamaged -= LoseStage;

        stageLogicalObjects.Clear();
    }

    // ---------- HELPERS ---------- //
    private void DestroyAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(parent.GetChild(i).gameObject);
        }
    }

    private void ValidateLevels()
    {
        LevelLoader levelLoader = GetComponent<LevelLoader>();

        bool allLevelsValid = true;
        foreach(LevelDataDefinition levelData in levelLoader.Levels)
        {
            StageController stageController = (StageController)levelData;
            
            if (stageController.StageSpawnPoint == null)
            {
                Debug.LogError($"StageManger -> ValidateLevels: Level [{stageController.levelName}] is not valid. No StageSpawnPoint found.");
                allLevelsValid = false;
            }

            if (stageController.StageExitTrigger == null)
            {
                Debug.LogError($"StageManger -> ValidateLevels: Level [{stageController.levelName}] is not valid. No StageExitTrigger found.");
                allLevelsValid = false;
            }
        }

        if(!allLevelsValid)
        {
            Debug.LogError("StageManger -> ValidateLevels: Not all levels valid.");
                return;
        }
    }

    // ---------- EVENTS ACTIONS ---------- //
    private void RestartStage() // TODO - restart actually needs to restart instead of just respawn
    {
        // exit early if the stage is already restarting
        if (stageIsRestarting)
            return;

        stageIsRestarting = true;

        ScreenFader.FadeOut(1f, () =>
        {
            SFXManager.PlaySound(SFX.PICKUP_OBJECT);

            m_playerCharacter.transform.position = m_currentStage.StageSpawnPoint.position;

            m_playerCharacter.GetComponent<Character3DBalancer>()?.FaceDirection(m_currentStage.StageSpawnPoint.transform.forward.normalized);

            ScreenFader.FadeIn(null);
            
            stageIsRestarting = false;
        });
    }

    private void LoseStage()
    {
            RestartStage();
    }

}
