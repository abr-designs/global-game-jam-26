using System;
using UnityEngine;

public class StageLogicalObject : MonoBehaviour
{
    public event Action CharacterDamaged;

    protected void OnCharacterDamaged()
    {
        CharacterDamaged?.Invoke();
    }
}