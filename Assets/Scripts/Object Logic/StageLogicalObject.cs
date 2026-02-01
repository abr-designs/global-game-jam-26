using System;
using UnityEngine;

public class StageLogicalObject : MonoBehaviour
{
    public static event Action OnCharacterDamaged;

    public static void CharacterDamaged()
    {
        OnCharacterDamaged?.Invoke();
    }
}