using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SolarSystemSpawner))]
public class SolarSystemSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SolarSystemSpawner spawner = (SolarSystemSpawner)target;

        if (GUILayout.Button("Generate Humble Abode Terrain in Edit Mode"))
        {
            spawner.editModeGeneration = true;
            spawner.Spawn(0);
            spawner.editModeGeneration = false;
        }
    }
}
