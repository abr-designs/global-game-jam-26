using Levels;
using UnityEngine;

public class StageController : LevelDataDefinition
{
    [SerializeField] private Transform m_stageSpawnPoint;
    public Transform StageSpawnPoint => m_stageSpawnPoint;
    public void SetStageSpawnPoint(Transform spawnPoint) { m_stageSpawnPoint = spawnPoint; }

    [SerializeField] private StageExitTrigger m_stageExitTrigger;
    public StageExitTrigger StageExitTrigger => m_stageExitTrigger;
    public void SetStageExitTrigger(StageExitTrigger exitTrigger) { m_stageExitTrigger = exitTrigger; }

    //public event Action RestartStage; // was not used
}