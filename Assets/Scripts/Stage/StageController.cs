using Levels;
using System;
using UnityEngine;

public class StageController : LevelDataDefinition
{
    [SerializeField] private Transform m_stageSpawnPoint;
    public Transform StageSpawnPoint => m_stageSpawnPoint;

    [SerializeField] private StageExitTrigger m_stageExitTrigger;
    public StageExitTrigger StageExitTrigger => m_stageExitTrigger;

    public Transform cameraLookatPoint;

    public event Action RestartStage;
}