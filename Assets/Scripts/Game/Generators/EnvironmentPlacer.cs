using System.Collections.Generic;
using UnityEngine;

public class EnvironmentPlacer
{
    public void PlaceElement(Vector3 position, EnvironmentElement element, LayerMask terrainLayer, GameObject parent)
    {
        if (Physics.Raycast(position + Vector3.up * 100, Vector3.down, out RaycastHit hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.point.y >= element.minHeightTerrain && hit.point.y <= element.maxHeightTerrain)
            {
                Quaternion rotation = element.randomRotation ? Quaternion.Euler(0, Random.Range(0, 360), 0) : Quaternion.identity;

                float yOffset = element.yOffset;
                if (element.randomYOffset && element.randomY != null)
                {
                    yOffset += Random.Range(element.randomY.minY, element.randomY.maxY);
                }

                Vector3 newPos = new Vector3(hit.point.x, hit.point.y + yOffset, hit.point.z);
                var go = GameObject.Instantiate(element.prefab, newPos, rotation, parent.transform);
                float randomScale = Random.Range(element.randomScale.minScale, element.randomScale.maxScale);
                go.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

                if(element.enableRotateToTerrain)
                {
                    go.transform.up = hit.normal;
                }
                if(IsNearbyDisallowedTag(go.transform.position, element.colliderCheckRadius, element))
                {
                    go.AddComponent<ObjectDestroyer>();
                    go.GetComponent<ObjectDestroyer>().Destroy();
                }
            }
        }
    }
    private bool IsNearbyDisallowedTag(Vector3 position, float checkRadius, EnvironmentElement element)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, checkRadius);
        foreach (var hitCollider in hitColliders)
        {
            // Jeœli collider ma tag, który jest niedozwolony dla tego elementu
            if (element.unacceptedTags.Contains(hitCollider.gameObject.tag))
            {
                return true;
            }
        }
        return false;
    }
    
}