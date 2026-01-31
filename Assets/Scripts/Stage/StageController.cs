using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] private StageSpawnPoint m_stageSpawnPoint;
    public StageSpawnPoint StageSpawnPoint => m_stageSpawnPoint;

    [SerializeField] private StageExitTrigger m_stageExitTrigger;
    public StageExitTrigger StageExitTrigger => m_stageExitTrigger;
}
