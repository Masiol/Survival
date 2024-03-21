using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TreeType
{
    Stump,
    Tree
}
public class Tree : MonoBehaviour, IDetectable
{
    [HideInInspector] public bool detectedChoppableTree;
    [HideInInspector] public bool playerInRange;
    
    [SerializeField] private TreeType treeType;

    private TreeManager treeManager;
    private DetectPlayer detectPlayer;
    private TreeHealthUI treeHealthUI;
    private bool shownTreeHP;

    private void Start()
    {
        treeManager = GetComponentInParent<TreeManager>(); 
        detectPlayer = GetComponentInParent<DetectPlayer>();
        treeHealthUI = GetComponent<TreeHealthUI>();

        if (detectPlayer != null)
        {
            detectPlayer.OnPlayerRangeChanged += HandlePlayerRangeChanged;
        }
    }
    private void Update()
    {
        if (treeType == TreeType.Tree)
        {
            if (detectedChoppableTree || playerInRange)
            {
                if (detectedChoppableTree && playerInRange)
                {
                    if (!shownTreeHP)
                    {
                        treeHealthUI.SetVisibleObject(true);
                        shownTreeHP = true;
                    }
                }
                else
                {
                    if (shownTreeHP)
                    {
                        treeHealthUI.SetVisibleObject(false);
                        shownTreeHP = false;
                    }
                }
            }
        }
        else if (treeType == TreeType.Stump) 
        {
            if (treeManager.CanChopStump)
            {
                if (detectedChoppableTree || playerInRange)
                {
                    if (detectedChoppableTree && playerInRange)
                    {
                        if (!shownTreeHP)
                        {
                            treeHealthUI.SetVisibleObject(true);
                            shownTreeHP = true;
                        }
                    }
                    else
                    {
                        if (shownTreeHP)
                        {
                            treeHealthUI.SetVisibleObject(false);
                            shownTreeHP = false;
                        }
                    }
                }
            }
        }
    }
    private void OnDestroy()
    {
        if (detectPlayer != null)
        {
            detectPlayer.OnPlayerRangeChanged -= HandlePlayerRangeChanged;
        }
    }
    private void HandlePlayerRangeChanged(bool _isInRange)
    {
        playerInRange = _isInRange;
    }

    public void DetectRaycast(bool _detect)
    {
        detectedChoppableTree = _detect;
    }
    public TreeType GetTreeType()
    {
        return treeType;
    }

    public void OnTriggered()
    {
        if (treeType == TreeType.Tree)
        {
            if (detectedChoppableTree && playerInRange)
            {
                GetComponent<TreeHealth>()?.TakeDamage(1);
            }
        }
        else if (treeType == TreeType.Stump)
        {
            if (treeManager.CanChopStump)
            {
                GetComponent<TreeHealth>()?.TakeDamage(1);
            }
        }
    }
    public void ApplyPhysics()
    {
        gameObject.AddComponent<Rigidbody>();
        StartCoroutine(treeManager.SpawnItems());
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
