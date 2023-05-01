using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PrefabSpawner))]
public class PrefabSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);
        PrefabSpawner prefabSpawner = (PrefabSpawner)target;

        GUILayout.Label("Individual Prefab Settings Actions:");
        for (int i = 0; i < prefabSpawner.prefabsSettings.Count; i++)
        {
            GUILayout.Label($"Prefab {i + 1}: " + (prefabSpawner.prefabsSettings[i].prefab == null ? "(Prefab is null)" : prefabSpawner.prefabsSettings[i].prefab.name));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate"))
            {
                prefabSpawner.GeneratePrefabs(prefabSpawner.prefabsSettings[i]);
            }

            if (GUILayout.Button("Delete"))
            {
                prefabSpawner.DeletePrefabs(prefabSpawner.prefabsSettings[i]);
            }
            GUILayout.EndHorizontal();

            prefabSpawner.prefabsSettings[i].prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefabSpawner.prefabsSettings[i].prefab, typeof(GameObject), false);
            prefabSpawner.prefabsSettings[i].numberOfInstances = EditorGUILayout.IntField("Number of Instances", prefabSpawner.prefabsSettings[i].numberOfInstances);
            prefabSpawner.prefabsSettings[i].distanceFromSurface = EditorGUILayout.FloatField("Distance from Surface", prefabSpawner.prefabsSettings[i].distanceFromSurface);
            prefabSpawner.prefabsSettings[i].scaleFactor = EditorGUILayout.FloatField("Scale Factor", prefabSpawner.prefabsSettings[i].scaleFactor);

            prefabSpawner.prefabsSettings[i].spawnOnLand = EditorGUILayout.Toggle("Spawn on Land", prefabSpawner.prefabsSettings[i].spawnOnLand);
            prefabSpawner.prefabsSettings[i].spawnOnOcean = EditorGUILayout.Toggle("Spawn on Ocean", prefabSpawner.prefabsSettings[i].spawnOnOcean);
            prefabSpawner.prefabsSettings[i].useCustomHeightRange = EditorGUILayout.Toggle("Use Custom Height Range", prefabSpawner.prefabsSettings[i].useCustomHeightRange);

            EditorGUI.BeginDisabledGroup(!prefabSpawner.prefabsSettings[i].useCustomHeightRange);
            prefabSpawner.prefabsSettings[i].minHeight = EditorGUILayout.FloatField("Min Height", prefabSpawner.prefabsSettings[i].minHeight);
            prefabSpawner.prefabsSettings[i].maxHeight = EditorGUILayout.FloatField("Max Height", prefabSpawner.prefabsSettings[i].maxHeight);
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);
        }
    }
}
