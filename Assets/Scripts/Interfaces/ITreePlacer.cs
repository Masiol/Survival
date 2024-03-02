using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITreePlacer
{
    void PlaceTrees(List<Vector3> positions, GameObject treePrefab, LayerMask terrainLayer, GameObject _treeParent, float rayDistace);
}
