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
        if (outline == null)
            outline = GetComponent<Outline>();

        if (outline != null) // Additional check in case the Outline component is missing
            outline.enabled = outlineEnabled;
        else
            Debug.LogWarning("Outline component missing on " + gameObject.name);
    }
}
