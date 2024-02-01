using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Outlineable : MonoBehaviour, IOutlineable
{
    private Outline outline;
    private void Start()
    {
        outline = GetComponent<Outline>();
        outline.OutlineMode = GameManager.instance.options.OutlineMode;
        outline.OutlineColor = GameManager.instance.options.OutlineColor;
        outline.enabled = false;
    }
    public void SetOutline(bool outlineEnabled)
    {
        outline.enabled = outlineEnabled;
    }
}
