using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[Serializable]
public class SpawnArea
{
    public Vector2 size = new Vector2(10, 10);
    public Vector3 center = Vector3.zero;
    public int amountToSpawn = 10;
    public GameObject animalPrefab;
    public Color colorArea;
}

[Serializable]
public class AnimalSpawnConfig
{
    public SpawnArea[] spawnAreas; 
}

public class AnimalSpawner : MonoBehaviour
{
    public AnimalSpawnConfig spawnConfig;

    private void Start()
    {
        foreach (var spawnArea in spawnConfig.spawnAreas)
        {
            SpawnAnimals(spawnArea);
        }
    }

    private void SpawnAnimals(SpawnArea spawnArea)
    {
        for (int i = 0; i < spawnArea.amountToSpawn; i++)
        {
            Vector2 randomPosition2D = new Vector2(UnityEngine.Random.Range(-spawnArea.size.x / 2, spawnArea.size.x / 2), UnityEngine.Random.Range(-spawnArea.size.y / 2, spawnArea.size.y / 2));
            Vector3 randomPosition = spawnArea.center + new Vector3(randomPosition2D.x, 0, randomPosition2D.y);

           
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, 10.0f, NavMesh.AllAreas))
            {
               
                Vector3 spawnPosition = hit.position;
                Quaternion randomRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
                Instantiate(spawnArea.animalPrefab, spawnPosition, randomRotation);
            }
            else
            {
              
                Debug.LogWarning("Nie mo¿na znaleŸæ pozycji na NavMesh w pobli¿u: " + randomPosition);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (spawnConfig != null && spawnConfig.spawnAreas != null)
        {
            foreach (var spawnArea in spawnConfig.spawnAreas)
            {
                Gizmos.color = spawnArea.colorArea;
                Gizmos.DrawWireCube(spawnArea.center, new Vector3(spawnArea.size.x, 1, spawnArea.size.y));
            }
        }
    }
}
