using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeGeneratorController : MonoBehaviour
{
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private int numberOfTrees = 100;
    [SerializeField] private float areaWidth = 100f;
    [SerializeField] private float areaLength = 100f;
    [SerializeField] private Vector3 centerPosition = Vector3.zero;
    [SerializeField] private float minRayDistance = 10f;
    [SerializeField] private List<Collider> exclusionZones;

    private ITreePlacer treePlacer;
    private TreePositionGenerator positionGenerator = new TreePositionGenerator();
    private GameObject treesParent;


    public void GenerateTrees()
    {
        InitializeTreesParent();
        ClearPreviousTrees();

        if (treePlacer == null)
        {
            treePlacer = new TreePlacer();
        }

        var positions = positionGenerator.GeneratePositions(numberOfTrees, areaWidth, areaLength, centerPosition, exclusionZones);
        treePlacer.PlaceTrees(positions, treePrefab, terrainLayer, treesParent, minRayDistance);
    }

    private void InitializeTreesParent()
    {
        treesParent = GameObject.Find("Trees") ?? new GameObject("Trees");
    }

    private void ClearPreviousTrees()
    {
        if (treesParent != null)
        {
            foreach (Transform child in treesParent.transform.Cast<Transform>().ToArray())
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(centerPosition, new Vector3(areaWidth, 0, areaLength));
    }
}
