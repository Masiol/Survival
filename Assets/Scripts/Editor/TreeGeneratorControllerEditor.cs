using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TreeGeneratorController))]
public class TreePlacerOnMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TreeGeneratorController script = (TreeGeneratorController)target;

        if (GUILayout.Button("Generate Trees"))
        {
            script.GenerateTrees();
        }
    }
}
