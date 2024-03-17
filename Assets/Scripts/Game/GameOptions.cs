using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptions : MonoBehaviour
{
    [Header("Gravity")] 
    [Tooltip("Gravity Strength = 9.81")]
    public float gravityStrength = 9.81f;

    [Header("Mouse")]
    [Range(0.01f, 100)]
    [SerializeField] private float userMouseSensitivity;
    public float MouseSensitivity => userMouseSensitivity;

    [Header("Interaction")]
    [Range(1f, 5f)]
    [SerializeField] private float interactionDistance;   
    public float InteractionDistance => interactionDistance;

    [Range(1f, 35f)]
    [SerializeField] private float buildDistance;
    public float BuildDistance => buildDistance;

    [Header("Outlines")]
    [SerializeField] private Outline.Mode outlineMode = Outline.Mode.OutlineAll;
    public Outline.Mode OutlineMode => outlineMode;

    [SerializeField] private Color32 outlineColor = new Color(255, 255, 0, 255);
    public Color32 OutlineColor => outlineColor;
}
