using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnvironmentElement
{
    public GameObject prefab;
    public int count;
    public float minHeightTerrain;
    public float maxHeightTerrain;
    public float yOffset;
    public float spacing;

    public bool randomYOffset;
    public bool randomRotation = false;
    public bool enableRotateToTerrain;

    public RandomYPos randomY;
    public RandomScale randomScale;

    public List<Collider> exclusionZones;

    public float colliderCheckRadius;
    public string objectTag;
    public List<string> unacceptedTags = new List<string>();
}

[System.Serializable]
public class RandomYPos
{
    public float minY;
    public float maxY;
}
[System.Serializable]
public class RandomScale
{
    public float minScale;
    public float maxScale;
}

[System.Serializable]
public class EnvironmentConfig
{
    public EnvironmentElement[] elements;
}
public class EnvironmentGeneratorController : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float areaWidth = 100f;
    [SerializeField] private float areaLength = 100f;
    [SerializeField] private Vector3 centerPosition = Vector3.zero;
    [SerializeField] private EnvironmentConfig environmentConfig;

    private EnvironmentPositionGenerator positionGenerator = new EnvironmentPositionGenerator();
    private EnvironmentPlacer placer = new EnvironmentPlacer();
    private GameObject environmentParent;

    public void GenerateEnvironment()
    {
        if (environmentParent != null)
        {
            DestroyImmediate(environmentParent);
        }
        environmentParent = new GameObject("GeneratedEnvironment");

        List<Vector3> occupiedPositions = new List<Vector3>();

        foreach (var element in environmentConfig.elements)
        {
            var positions = positionGenerator.GeneratePositions(element, areaWidth, areaLength, centerPosition);
            GameObject elementParent = new GameObject(element.prefab.name + "Container");
            elementParent.transform.parent = environmentParent.transform;

            foreach (var position in positions)
            {
                placer.PlaceElement(position, element, terrainLayer, elementParent);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(centerPosition + Vector3.up * 0.5f, new Vector3(areaWidth, 1, areaLength));

        if (environmentConfig != null && environmentConfig.elements != null)
        {
            foreach (var element in environmentConfig.elements)
            {
                if (element.exclusionZones != null)
                {
                    Gizmos.color = new Color(1, 0, 0, 0.5f);
                    foreach (var zone in element.exclusionZones)
                    {
                        Gizmos.DrawCube(zone.bounds.center, zone.bounds.size);
                    }
                }
            }
        }
    }
}
