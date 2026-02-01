using Audio;
using GGJ.Player;
using UnityEngine;

public class SpikesTile : StageLogicalObject
{
    private bool _playerInSpikes;
    private bool _playerDashing;

    private void OnEnable()
    {
        CharacterDashAbility.Dashing += SetPlayerDashing;
    }

    private void OnDisable()
    {
        CharacterDashAbility.Dashing -= SetPlayerDashing;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        _playerInSpikes = true;

        if (_playerDashing)
            return;

        PlayerSuccumbsToSpikes();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        _playerInSpikes = false;
    }

    public void SetPlayerDashing(bool enabled)
    {
        _playerDashing = enabled;

        if (_playerDashing)
            return;

        if (_playerInSpikes)
            PlayerSuccumbsToSpikes();
    }    

    private void PlayerSuccumbsToSpikes()
    {
        Debug.LogWarning("PlayerSuccumbsToSpikes");
        SFXManager.PlaySound(SFX.PLAYER_DIED);
        base.OnCharacterDamaged();
    }
}