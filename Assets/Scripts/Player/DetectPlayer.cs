using System;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public event Action<bool> OnPlayerRangeChanged;
    private bool _isPlayerInRange;
    public bool isPlayerInRange
    {
        get => _isPlayerInRange;
        private set
        {
            if (_isPlayerInRange != value)
            {
                _isPlayerInRange = value;
                OnPlayerRangeChanged?.Invoke(_isPlayerInRange);
            }
        }
    }
    private GameObject player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>()?.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInRange = false;
        }
    }
}
