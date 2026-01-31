using UnityEngine;

public class SpikesTile : StageLogicalObject
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        base.OnCharacterDamaged();
    }
}