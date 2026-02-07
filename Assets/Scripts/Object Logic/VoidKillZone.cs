using Audio;
using UnityEngine;

public class VoidKillZone : StageLogicalObject
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        VoidKill();
    }

    private void VoidKill()
    {
        SFXManager.PlaySound(SFX.PLAYER_DIED);
        CharacterDamaged();
    }
}