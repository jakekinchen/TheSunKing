using UnityEditor;
using UnityEngine;

public class PrefabSpawnerWindow : EditorWindow
{
    private PrefabSpawner prefabSpawner;
    private Vector2 scrollPosition;

    [MenuItem("Window/Prefab Spawner")]
    public static void ShowWindow()
    {
        GetWindow<PrefabSpawnerWindow>("Prefab Spawner");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        prefabSpawner = (PrefabSpawner)EditorGUILayout.ObjectField("Prefab Spawner", prefabSpawner, typeof(PrefabSpawner), true);

        if (prefabSpawner == null)
        {
            EditorGUILayout.HelpBox("Please assign a Prefab Spawner.", MessageType.Warning);
            return;
        }
    GUILayout.Space(10);
        EditorGUI.BeginChangeCheck();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        GUILayout.Space(10);
        GUILayout.Label("Individual Prefab Settings Actions:");
        for (int i = 0; i < prefabSpawner.prefabsSettings.Count; i++)
        {
            GUILayout.Label($"Prefab {i + 1}: " + (prefabSpawner.prefabsSettings[i].prefab == null ? "(Prefab is null)" : prefabSpawner.prefabsSettings[i].prefab.name));


            prefabSpawner.prefabsSettings[i].prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefabSpawner.prefabsSettings[i].prefab, typeof(GameObject), false);
            prefabSpawner.prefabsSettings[i].numberOfInstances = EditorGUILayout.IntField("Number of Instances", prefabSpawner.prefabsSettings[i].numberOfInstances);
            prefabSpawner.prefabsSettings[i].distanceFromSurface = EditorGUILayout.FloatField("Distance from Surface", prefabSpawner.prefabsSettings[i].distanceFromSurface);
            prefabSpawner.prefabsSettings[i].scaleFactor = EditorGUILayout.FloatField("Scale Factor", prefabSpawner.prefabsSettings[i].scaleFactor);

            prefabSpawner.prefabsSettings[i].usePoissonDiskSampling = EditorGUILayout.Toggle("Use Poisson Disk Sampling", prefabSpawner.prefabsSettings[i].usePoissonDiskSampling);
            if (prefabSpawner.prefabsSettings[i].usePoissonDiskSampling)
            {
                prefabSpawner.prefabsSettings[i].poissonDiskRadius = EditorGUILayout.FloatField("Radius", prefabSpawner.prefabsSettings[i].poissonDiskRadius);
                prefabSpawner.prefabsSettings[i].poissonDiskMaxSamples = EditorGUILayout.IntField("Rejection Samples", prefabSpawner.prefabsSettings[i].poissonDiskMaxSamples);
            }

            prefabSpawner.prefabsSettings[i].spawnOnLand = EditorGUILayout.Toggle("Spawn on Land", prefabSpawner.prefabsSettings[i].spawnOnLand);
            if (EditorGUI.EndChangeCheck())
            {
                HandleToggleChange(i, "land");
            }

            EditorGUI.BeginChangeCheck();
            prefabSpawner.prefabsSettings[i].spawnOnOcean = EditorGUILayout.Toggle("Spawn on Ocean", prefabSpawner.prefabsSettings[i].spawnOnOcean);
            if (EditorGUI.EndChangeCheck())
            {
                HandleToggleChange(i, "ocean");
            }

            EditorGUI.BeginChangeCheck();
            prefabSpawner.prefabsSettings[i].useCustomHeightRange = EditorGUILayout.Toggle("Use Custom Height Range", prefabSpawner.prefabsSettings[i].useCustomHeightRange);
            if (EditorGUI.EndChangeCheck())
            {
                HandleToggleChange(i, "custom");
            }
            if (prefabSpawner.prefabsSettings[i].useCustomHeightRange)
            {
            prefabSpawner.prefabsSettings[i].minHeight = EditorGUILayout.FloatField("Min Height", prefabSpawner.prefabsSettings[i].minHeight);
            prefabSpawner.prefabsSettings[i].maxHeight = EditorGUILayout.FloatField("Max Height", prefabSpawner.prefabsSettings[i].maxHeight);
            }

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
            GUILayout.Space(20);
        }
        EditorGUILayout.EndScrollView();

        if(GUILayout.Button("Add Prefab Settings"))
        {
            prefabSpawner.prefabsSettings.Add(new PrefabSettings());
        }
      

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(prefabSpawner);
            if (!EditorApplication.isPlaying)
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(prefabSpawner.gameObject.scene);
            }
        }
    }

    private void HandleToggleChange(int index, string option)
{
    if (option == "land")
    {
        if (!prefabSpawner.prefabsSettings[index].spawnOnLand && !prefabSpawner.prefabsSettings[index].spawnOnOcean && !prefabSpawner.prefabsSettings[index].useCustomHeightRange)
        {
            prefabSpawner.prefabsSettings[index].spawnOnLand = true;
        }
    }
    else if (option == "ocean")
    {
        if (!prefabSpawner.prefabsSettings[index].spawnOnLand && !prefabSpawner.prefabsSettings[index].spawnOnOcean && !prefabSpawner.prefabsSettings[index].useCustomHeightRange)
        {
            prefabSpawner.prefabsSettings[index].spawnOnOcean = true;
        }
    }
    else if (option == "custom")
    {
        if (!prefabSpawner.prefabsSettings[index].spawnOnLand && !prefabSpawner.prefabsSettings[index].spawnOnOcean && !prefabSpawner.prefabsSettings[index].useCustomHeightRange)
        {
            prefabSpawner.prefabsSettings[index].useCustomHeightRange = true;
        }
    }
}
}
