using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [SerializeField] private bool canChopStump = false;

    [SerializeField] private float durationToSpawnLogs;
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private GameObject stickPrefab; // Dodano prefab patyka
    [SerializeField] private Transform woodSpawnPositionsParent;
    [SerializeField] private Transform stickSpawnPositionsParent;

    [SerializeField] private Tree spawnableItemTree;

    public bool CanChopStump
    {
        get { return canChopStump; }
        set { canChopStump = value; }
    }
    public IEnumerator SpawnItems()
    {
        yield return new WaitForSeconds(durationToSpawnLogs);

        for (int i = 0; i < woodSpawnPositionsParent.childCount; i++)
        {
            Vector3 position = woodSpawnPositionsParent.GetChild(i).position;
            Quaternion rotation = woodSpawnPositionsParent.GetChild(i).rotation;

            Instantiate(woodPrefab, position, rotation);
        }

        for (int i = 0; i < stickSpawnPositionsParent.childCount; i++)
        {
            Vector3 position = stickSpawnPositionsParent.GetChild(i).position;
            Quaternion rotation = stickSpawnPositionsParent.GetChild(i).rotation;

            Instantiate(stickPrefab, position, rotation);
        }

        spawnableItemTree.DestroyObject();
    }
}
