using System.Collections.Generic;
using UnityEngine;

public class TreePositionGenerator
{
    private bool IsPositionInExclusionZone(Vector3 _position, List<Collider> _exclusiveZones)
    {
        foreach (var zone in _exclusiveZones)
        {
            if(zone.bounds.Contains(_position))
            {
                return true;
            }
        }
        return false;
    }
    public List<Vector3> GeneratePositions(int numberOfTrees, float areaWidth, float areaLength, Vector3 centerPosition, List<Collider> exclusionZones)
    {
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < numberOfTrees; i++)
        {
            Vector3 position = Vector3.zero;
            bool validPositionFound = false;
            while (!validPositionFound)
            {
                float posX = Random.Range(-areaWidth / 2, areaWidth / 2) + centerPosition.x;
                float posZ = Random.Range(-areaLength / 2, areaLength / 2) + centerPosition.z;
                position = new Vector3(posX, 100, posZ);

                if (!IsPositionInExclusionZone(position, exclusionZones))
                {
                    validPositionFound = true;
                }
            }
            positions.Add(position);
        }
        return positions;
    }
}