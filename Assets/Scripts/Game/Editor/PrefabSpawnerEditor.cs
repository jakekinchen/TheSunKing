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

        if (GUILayout.Button("Add Prefab"))
        {
            prefabSpawner.prefabsSettings.Add(new PrefabSettings());
        }

        if (GUILayout.Button("Remove Last Prefab") && prefabSpawner.prefabsSettings.Count > 0)
        {
            prefabSpawner.prefabsSettings.RemoveAt(prefabSpawner.prefabsSettings.Count - 1);
        }
    }
}
