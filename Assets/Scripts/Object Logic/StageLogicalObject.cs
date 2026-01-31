using System;
using UnityEngine;

public class StageLogicalObject : MonoBehaviour
{
    public static event Action CharacterDamaged;

    protected void OnCharacterDamaged()
    {
        CharacterDamaged?.Invoke();
    }
}