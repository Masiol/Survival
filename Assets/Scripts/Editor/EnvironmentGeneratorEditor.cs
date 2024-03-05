#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnvironmentGeneratorController))]
public class EnvironmentGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnvironmentGeneratorController generator = (EnvironmentGeneratorController)target;
        if (GUILayout.Button("Generate Environment"))
        {
            generator.GenerateEnvironment();
        }
    }
}
#endif