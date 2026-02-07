using System;
using Audio;
using Levels;
using Samples.CharacterController3D.Scripts;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using Object = UnityEngine.Object;
using GGJ.Player;
using Unity.Cinemachine;

public class StageManager : MonoBehaviour
{
    public static event Action OnPlayerReset;
    [SerializeField] private Transform m_stageContainer;
    [SerializeField] private Transform m_playerCharacter;
    [SerializeField] private StageController m_currentStage;

    [Header("UI")]
    [SerializeField] private InGameMenuUI m_inGameMenu;

    [Header("Runtime")]
    private bool stageIsRestarting;

    // private
    private int _creditsSceneIndex = 2;

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

    //private void Awake()
    //{
    //    ValidateLevels();
    //}

    private void Start()
    {
        if(m_playerCharacter == null)
        {
            Debug.LogError($"StageManager: No PlayerCharacter found");
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
        m_currentStage = ((StageController)LevelLoader.CurrentLevelDataDefinition);

        LoadStage();
    }

    private void LoadStage()
    {
        StageLogicalObject.OnCharacterDamaged += LoseStage;
        StartStage();
    }

    private void StartStage()
    {
        m_playerCharacter.GetComponent<Rigidbody>().position = m_currentStage.StageSpawnPoint.position;
        m_playerCharacter.GetComponent<Character3DBalancer>()?.ForceFaceDirection(m_currentStage.StageSpawnPoint.transform.forward.normalized);

        // Have camera look behind player
        FindFirstObjectByType<CharacterCameraLook>()?.Recenter();

        PlayerMaskManager.EquipMask(GGJ.Player.Enums.MASK_TYPE.NONE);

        m_currentStage.StageExitTrigger.PlayerReachedExit += EndStage;

        ScreenFader.FadeIn(1f, null);

        stageIsRestarting = false;
    }

    private void EndStage()
    {
        ScreenFader.FadeOut(1f, () =>
        {
            //SFXManager.PlaySound(SFX.PICKUP_OBJECT);

            // if this is last stage then else change scenes
            if (LevelLoader.OnLastLevel())
            {
                // change to credits scene
                SceneManager.LoadScene(_creditsSceneIndex);
                return; // todo - somewhere fade in is happening before the change to the credits scene
            }

            BreakdownCurrentStage();

            // advance to next stage
            LevelLoader.LoadNextLevel();
            m_currentStage = ((StageController)LevelLoader.CurrentLevelDataDefinition);
            m_currentStage.transform.SetParent(m_stageContainer, false);
            LoadStage();
        });
    }

    private void BreakdownCurrentStage()
    {
        m_currentStage.StageExitTrigger.PlayerReachedExit -= EndStage;

        // destroy existing stage
        Destroy(m_currentStage.gameObject);

        StageLogicalObject.OnCharacterDamaged -= LoseStage;
    }

    // ---------- HELPERS ---------- //
    private void DestroyAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(parent.GetChild(i).gameObject);
        }
    }

    //private void ValidateLevels()
    //{
    //    LevelLoader levelLoader = GetComponent<LevelLoader>();

    //    bool allLevelsValid = true;
    //    foreach(LevelDataDefinition levelData in levelLoader.Levels)
    //    {
    //        StageController stageController = (StageController)levelData;
            
    //        if (stageController.StageSpawnPoint == null)
    //        {
    //            Debug.LogError($"StageManger -> ValidateLevels: Level [{stageController.levelName}] is not valid. No StageSpawnPoint found.");
    //            allLevelsValid = false;
    //        }

    //        if (stageController.StageExitTrigger == null)
    //        {
    //            Debug.LogError($"StageManger -> ValidateLevels: Level [{stageController.levelName}] is not valid. No StageExitTrigger found.");
    //            allLevelsValid = false;
    //        }
    //    }

    //    if(!allLevelsValid)
    //    {
    //        Debug.LogError("StageManger -> ValidateLevels: Not all levels valid.");
    //            return;
    //    }
    //}

    // ---------- EVENTS ACTIONS ---------- //
    private void RestartStage() // TODO - restart actually needs to restart instead of just respawn
    {
        // exit early if the stage is already restarting
        if (stageIsRestarting)
            return;

        stageIsRestarting = true;

        ScreenFader.FadeOut(1f, () =>
        {
            //SFXManager.PlaySound(SFX.PICKUP_OBJECT);

            BreakdownCurrentStage();
            LevelLoader.Restart();
            m_currentStage = ((StageController)LevelLoader.CurrentLevelDataDefinition);
            m_currentStage.transform.SetParent(m_stageContainer, false);
            OnPlayerReset?.Invoke();
            LoadStage();

            ScreenFader.FadeIn(null);
            
            stageIsRestarting = false;
        });
    }

    private void LoseStage()
    {
        RestartStage();
    }

}
