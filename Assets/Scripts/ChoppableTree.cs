using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableTree : MonoBehaviour, IDetectable
{
    [SerializeField] private float durationToSpawnLogs;
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private Transform woodSpawnPositionsParent;

    private bool detectedChoppableTree;
    private bool playerInRange;
    private bool shownTreeHP;
    private GameObject player;

    private DetectPlayer detectPlayer;
    private TreeHealthUI treeHealthUI;
    private void Start()
    {
        detectPlayer = GetComponentInParent<DetectPlayer>();
        treeHealthUI = GetComponent<TreeHealthUI>();

        if (detectPlayer != null)
        {
            detectPlayer.OnPlayerRangeChanged += HandlePlayerRangeChanged;
        }
    }
    private void Update()
    {
        if (detectedChoppableTree || playerInRange)
        {
            if(detectedChoppableTree && playerInRange)
            {
                if(!shownTreeHP)
                {
                    treeHealthUI.SetVisibleObject(true);
                    shownTreeHP = true;
                }
            }
            else
            {
                if(shownTreeHP)
                {
                    treeHealthUI.SetVisibleObject(false);
                    shownTreeHP = false;
                }
            }
        }
    }
    private void OnDestroy()
    {
        if (detectPlayer != null)
        {
            detectPlayer.OnPlayerRangeChanged -= HandlePlayerRangeChanged;
        }
    }
    private void HandlePlayerRangeChanged(bool _isInRange)
    {
        playerInRange = _isInRange;
    }

    public void DetectRaycast(bool _detect)
    {
        detectedChoppableTree = _detect;
    }

    public void OnTriggered()
    {
        if(detectedChoppableTree && playerInRange)
        {
            GetComponent<TreeHealth>()?.TakeDamage(1);
        }
    }

    public void ApplyPhysics()
    {
        gameObject.AddComponent<Rigidbody>();
        StartCoroutine(SpawnLogs(durationToSpawnLogs));
    }

    private IEnumerator SpawnLogs(float _duration)
    {
        yield return new WaitForSeconds(_duration);

        for (int i = 0; i < woodSpawnPositionsParent.childCount; i++)
        {
            Vector3 position = woodSpawnPositionsParent.GetChild(i).position;
            Quaternion rotation = woodSpawnPositionsParent.GetChild(i).rotation;
            Instantiate(woodPrefab, position, rotation);
        }

        Destroy(gameObject);
    }

}
