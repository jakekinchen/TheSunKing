using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PrefabSpawner))]
public class PrefabSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PrefabSpawner prefabSpawner = (PrefabSpawner)target;
        if (GUILayout.Button("Generate Prefabs"))
        {
            prefabSpawner.GeneratePrefabs();
        }
    }
}
