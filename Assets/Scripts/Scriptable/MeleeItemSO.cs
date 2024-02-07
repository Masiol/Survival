using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeItemSO", menuName = "ScriptableObject/MeleeItemSO")]
public class MeleeItemSO : HandItemSO
{
    [SerializeField] private float Damage = 0;
    public float damage => Damage;
}
