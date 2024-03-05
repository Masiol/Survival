using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvironmentPositionGenerator
{
    public List<Vector3> GeneratePositions(EnvironmentElement element, float areaWidth, float areaLength, Vector3 centerPosition)
    {
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < element.count; i++)
        {
            int attempt = 0;
            bool positionFound = false;

            while (!positionFound && attempt < 100) // Ograniczenie prób do unikniêcia nieskoñczonej pêtli.
            {
                attempt++;
                Vector3 potentialPosition = new Vector3(
                    Random.Range(-areaWidth / 2, areaWidth / 2) + centerPosition.x,
                    100, // Start z wysokoœci 100 do raycast w dó³
                    Random.Range(-areaLength / 2, areaLength / 2) + centerPosition.z
                );

                if (Physics.Raycast(potentialPosition, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                {
                    Vector3 adjustedPosition = hit.point; // Pozycja dostosowana do wysokoœci terenu

                    // Sprawdzenie minimalnego odstêpu od innych obiektów
                    if (hit.point.y >= element.minHeightTerrain && hit.point.y <= element.maxHeightTerrain && positions.All(pos => Vector3.Distance(new Vector3(pos.x, 0, pos.z), new Vector3(adjustedPosition.x, 0, adjustedPosition.z)) >= element.spacing))
                    {
                        if (element.exclusionZones.Count != 0)
                        {
                            if (!IsPositionInExclusionZone(adjustedPosition, element.exclusionZones))
                            {
                                positions.Add(adjustedPosition);
                                positionFound = true;
                            }
                        }
                        else
                        {
                            positions.Add(adjustedPosition);
                            positionFound = true;
                        }

                    }
                }
            }
        }
        return positions;
    }

    private bool IsPositionInExclusionZone(Vector3 position, List<Collider> exclusionZones)
    {
        foreach (var zone in exclusionZones)
        {
            if (zone.bounds.Contains(new Vector3(position.x, zone.bounds.center.y, position.z))) // Uwzglêdnij tylko X i Z dla sprawdzenia
            {
                return true;
            }
        }
        return false;
    }
}