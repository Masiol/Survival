using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [Header("Build Objects")]
    [SerializeField] private List<GameObject> floorObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> wallObjects = new List<GameObject>();

    [Header("Build Settings")]
    [SerializeField] private SelectedBuildType currentBuildType;
    [SerializeField] private LayerMask connectorLayer;

    [Header("Ghost Settings")]
    [SerializeField] private Material ghostMaterialValid;
    [SerializeField] private Material ghostMaterialInvalid;
    [SerializeField] private Material ghostMaterialAfterBuild;
    [SerializeField] private float connectorOverlapRadius = 1;
    [SerializeField] private float maxGroundAngle = 45f;

    [Header("Internal State")]
    [SerializeField] private bool isBuilding = false;
    [SerializeField] private int currentBuildingIndex;
    private GameObject ghostBuildGameobject;
    private bool isGhostInValidPosition = false;
    private Transform ModelParent = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            isBuilding = !isBuilding;

        if (isBuilding)
        {
            GhostBuild();

            if (Input.GetMouseButtonDown(0))
                PlaceBuild();
        }
        else if (ghostBuildGameobject)
        {
            Destroy(ghostBuildGameobject);
            ghostBuildGameobject = null;
        }
    }

    private void GhostBuild()
    {
        GameObject currentBuild = GetCurrentBuild();
        CreateGhostPrefab(currentBuild);

        MoveGhostPrefabToRaycast();
        CheckBuildValidity();
    }

    private void CreateGhostPrefab(GameObject _currentBuild)
    {
        if (ghostBuildGameobject == null)
        {
            ghostBuildGameobject = Instantiate(_currentBuild);

            ModelParent = ghostBuildGameobject.transform.GetChild(0);

            GhostifyModel(ModelParent, ghostMaterialValid);
            GhostifyModel(ghostBuildGameobject.transform);
        }
    }

    private void MoveGhostPrefabToRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, GameManager.instance.options.BuildDistance))
        {
            ghostBuildGameobject.transform.position = hit.point;
        }
    }

    private void CheckBuildValidity()
    {
        Collider[] colliders = Physics.OverlapSphere(ghostBuildGameobject.transform.position, connectorOverlapRadius, connectorLayer);
        if (colliders.Length > 0)
        {
            GhostConnectBuild(colliders);
        }
        else
        {
            GhostSeperateBuild();
        }
    }

    private void GhostConnectBuild(Collider[] _colliders)
    {
        Connector bestConnector = null;

        foreach (Collider collider in _colliders)
        {
            Connector connector = collider.GetComponent<Connector>();

            if (connector.canConnectTo)
            {
                bestConnector = connector;
                break;
            }
        }

        if (bestConnector == null || currentBuildType == SelectedBuildType.Floor && bestConnector.isConnectedToFloor || currentBuildType == SelectedBuildType.Wall && bestConnector.isConnectedToWall)
        {
            GhostifyModel(ModelParent, ghostMaterialInvalid);
            isGhostInValidPosition = false;
            return;
        }

        SnapGhostPrefabToConnector(bestConnector);
    }

    private void SnapGhostPrefabToConnector(Connector _connector)
    {
        Transform ghostConnector = FindSnapConnector(_connector.transform, ghostBuildGameobject.transform.GetChild(1));
        ghostBuildGameobject.transform.position = _connector.transform.position - (ghostConnector.position - ghostBuildGameobject.transform.position);

        if (currentBuildType == SelectedBuildType.Wall)
        {
            Quaternion newRotation = ghostBuildGameobject.transform.rotation;
            newRotation.eulerAngles = new Vector3(newRotation.eulerAngles.x, _connector.transform.rotation.eulerAngles.y, newRotation.eulerAngles.z);
            ghostBuildGameobject.transform.rotation = newRotation;
        }

        GhostifyModel(ModelParent, ghostMaterialValid);
        isGhostInValidPosition = true;
    }

    private void GhostSeperateBuild()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (currentBuildType == SelectedBuildType.Wall)
            {
                GhostifyModel(ModelParent, ghostMaterialInvalid);
                isGhostInValidPosition = false;
                return;
            }

            if (hit.collider.transform.root.CompareTag("Buildables"))
            {
                GhostifyModel(ModelParent, ghostMaterialInvalid);
                isGhostInValidPosition = false;
                return;
            }

            if (Vector3.Angle(hit.normal, Vector3.up) < maxGroundAngle)
            {
                GhostifyModel(ModelParent, ghostMaterialValid);
                isGhostInValidPosition = true;
            }
            else
            {
                GhostifyModel(ModelParent, ghostMaterialInvalid);
                isGhostInValidPosition = false;
            }
        }
    }

    private Transform FindSnapConnector(Transform _snapConnector, Transform _ghostConnectorParent)
    {
        ConnectorPosition OppositeConnectorTag = getOppositePosition(_snapConnector.GetComponent<Connector>());

        foreach (Connector connector in _ghostConnectorParent.GetComponentsInChildren<Connector>())
        {
            if (connector.connectorPosition == OppositeConnectorTag)
                return connector.transform;
        }

        return null;
    }

    private ConnectorPosition getOppositePosition(Connector _connector)
    {
        ConnectorPosition position = _connector.connectorPosition;

        if (currentBuildType == SelectedBuildType.Wall && _connector.connectorParentType == SelectedBuildType.Floor)
            return ConnectorPosition.Bottom;

        if (currentBuildType == SelectedBuildType.Floor && _connector.connectorParentType == SelectedBuildType.Wall && _connector.connectorPosition == ConnectorPosition.Top)
        {
            if (_connector.transform.root.rotation.y == 0)
            {
                return GetConnectorClosestToPlayer(true);
            }
            else
            {
                return GetConnectorClosestToPlayer(false);
            }
        }

        switch (position)
        {
            case ConnectorPosition.Left:
                return ConnectorPosition.Right;
            case ConnectorPosition.Right:
                return ConnectorPosition.Left;
            case ConnectorPosition.Bottom:
                return ConnectorPosition.Top;
            case ConnectorPosition.Top:
                return ConnectorPosition.Bottom;
            default:
                return ConnectorPosition.Bottom;
        }
    }

    private ConnectorPosition GetConnectorClosestToPlayer(bool _topBottom)
    {
        Transform cameraTransform = Camera.main.transform;

        if (_topBottom)
            return cameraTransform.position.z >= ghostBuildGameobject.transform.position.z ? ConnectorPosition.Bottom : ConnectorPosition.Top;
        else
            return cameraTransform.position.x >= ghostBuildGameobject.transform.position.x ? ConnectorPosition.Left : ConnectorPosition.Right;
    }

    private void GhostifyModel(Transform _modelParent, Material _ghostMaterial = null)
    {
        if (_ghostMaterial != null)
        {
            foreach (MeshRenderer meshRenderer in _modelParent.GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material = _ghostMaterial;
            }
        }
        else
        {
            foreach (Collider modelColliders in _modelParent.GetComponentsInChildren<Collider>())
            {
                modelColliders.enabled = false;
            }
        }
    }

    private GameObject GetCurrentBuild()
    {
        switch (currentBuildType)
        {
            case SelectedBuildType.Floor:
                return floorObjects[currentBuildingIndex];
            case SelectedBuildType.Wall:
                return wallObjects[currentBuildingIndex];
        }

        return null;
    }

    private void PlaceBuild()
    {
        if (ghostBuildGameobject != null & isGhostInValidPosition)
        {
            GameObject newBuild = Instantiate(GetCurrentBuild(), ghostBuildGameobject.transform.position, ghostBuildGameobject.transform.rotation);
            if (newBuild != null)
            {
                foreach (MeshRenderer meshRenderer in newBuild.GetComponentsInChildren<MeshRenderer>())
                {
                    meshRenderer.material = ghostMaterialAfterBuild;
                }

                Transform modelTransform = newBuild.transform.Find("Model");
                if (modelTransform != null)
                {
                    Collider modelCollider = modelTransform.GetComponent<Collider>();
                    if (modelCollider != null)
                    {
                        modelCollider.isTrigger = true;
                    }
                }
            }
            Destroy(ghostBuildGameobject);
            ghostBuildGameobject = null;

            newBuild.GetComponentInChildren<BuildableElement>().SetupUI();

            isBuilding = false;

            foreach (Connector connector in newBuild.GetComponentsInChildren<Connector>())
            {
                connector.UpdateConnectors(true);
            }
        }
    }
}

[System.Serializable]
public enum SelectedBuildType
{
    Floor,
    Wall,
}