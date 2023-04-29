using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabSpawner))]
public class PrefabSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PrefabSpawner spawner = (PrefabSpawner)target;

        if (GUILayout.Button("Spawn Prefabs"))
        {
            spawner.SpawnPrefabs();
        }
    }
}
