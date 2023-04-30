using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PrefabSpawner))]
public class PrefabSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PrefabSpawner prefabSpawner = (PrefabSpawner)target;

        if (GUILayout.Button("Initialize Folders"))
        {
            prefabSpawner.Initialize();
        }

        for (int i = 0; i < prefabSpawner.prefabsSettings.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(prefabSpawner.prefabsSettings[i].prefab.name);
            if (GUILayout.Button("Generate"))
            {
                prefabSpawner.GeneratePrefabs(prefabSpawner.prefabsSettings[i]);
            }

                        if (GUILayout.Button("Delete"))
            {
                prefabSpawner.DeletePrefabs(prefabSpawner.prefabsSettings[i]);
            }
            GUILayout.EndHorizontal();
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

