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

        PlaySuccumsToSpikes();
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
            PlaySuccumsToSpikes();
    }    

    private void PlaySuccumsToSpikes()
    {
        Debug.LogWarning("PlaySuccumsToSpikes");
        SFXManager.PlaySound(SFX.PLAYER_DIED);
        CharacterDamaged();
    }
}