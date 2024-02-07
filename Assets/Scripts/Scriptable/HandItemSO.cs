using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HandItemSO", menuName = "ScriptableObject/HandItemSO")]
public class HandItemSO : ItemSO
{
    [SerializeField] private Vector3 StartRotation = Vector3.zero;
    public Vector3 startRotation => StartRotation;
}
