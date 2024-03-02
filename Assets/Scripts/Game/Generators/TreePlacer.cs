using System.Collections.Generic;
using UnityEngine;

public class TreePlacer : ITreePlacer
{
    public void PlaceTrees(List<Vector3> _positions, GameObject _treePrefab, LayerMask _terrainLayer, GameObject _treesParent, float _minRayDistance)
    {
        foreach (Vector3 startPosition in _positions)
        {
            if (Physics.Raycast(startPosition, Vector3.down, out RaycastHit hit, Mathf.Infinity, _terrainLayer))
            {
                if (hit.point.y < _minRayDistance) 
                {
                    Vector3 newVector = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
                    GameObject tree = Object.Instantiate(_treePrefab, newVector, Quaternion.identity, _treesParent.transform);
                    // tree.transform.up = hit.normal;
                }
            }
        }
    }
}
