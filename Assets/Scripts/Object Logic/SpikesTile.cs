using Audio;
using UnityEngine;

public class SpikesTile : StageLogicalObject
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        SFXManager.PlaySound(SFX.PLAYER_DIED);
        base.OnCharacterDamaged();
    }
}