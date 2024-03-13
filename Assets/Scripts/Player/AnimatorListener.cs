using UnityEngine;
using System;

public class AnimatorListener : MonoBehaviour
{
    public static event Action<HandItemSO> OnAttackEvent;

    public HandItemSO _handItem;

    public HandItemSO handItem
    {
        get { return _handItem; }
        set { _handItem = value; }
    }
    public void AttackEvent()
    {
        Debug.Log("test");
        if (OnAttackEvent != null)
        {
            OnAttackEvent?.Invoke(_handItem);
        }
    }
}