using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabSettings
{
    public GameObject prefab;
    public int numberOfInstances;
    public float distanceFromSurface;
    public float scaleFactor = 1.0f;
    public float minHeight;
    public float maxHeight;
    [HideInInspector]
    public GameObject folder; // Store the folder for each prefab
}

public class PrefabSpawner : MonoBehaviour
{
    public GameObject terrainObject;
    public List<PrefabSettings> prefabsSettings;

    private Mesh terrainMesh;
    private Vector3[] vertices;
    private Transform celestialBodyTransform;

    public void Initialize()
    {
        GameObject parentFolder = GameObject.Find("Prefab Clones");
        if (parentFolder == null)
        {
            parentFolder = new GameObject("Prefab Clones");
        }

        foreach (var prefabSettings in prefabsSettings)
        {
            prefabSettings.folder = new GameObject(prefabSettings.prefab.name + " Clones");
            prefabSettings.folder.transform.parent = parentFolder.transform;
        }

        if (terrainObject == null) return;

        MeshFilter terrainMeshFilter = terrainObject.GetComponent<MeshFilter>();
        if (terrainMeshFilter != null)
        {
            terrainMesh = terrainMeshFilter.sharedMesh;
            vertices = terrainMesh.vertices;
            celestialBodyTransform = terrainObject.transform.parent;
        }
        else
        {
            Debug.LogError("Terrain object does not have a MeshFilter component.");
        }
    }

    public void GeneratePrefabs(PrefabSettings settings)
    {
        if (terrainObject == null) return;

        for (int i = 0; i < settings.numberOfInstances; i++)
        {
            int randomIndex = Random.Range(0, vertices.Length);
            Vector3 spawnPosition = celestialBodyTransform.TransformPoint(vertices[randomIndex]) + celestialBodyTransform.TransformDirection(vertices[randomIndex]) * settings.distanceFromSurface;
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, celestialBodyTransform.TransformDirection(vertices[randomIndex]));

            float terrainHeight = terrainMesh.bounds.extents.y * vertices[randomIndex].y;

            if (terrainHeight >= settings.minHeight && terrainHeight <= settings.maxHeight)
            {
                GameObject spawnedPrefab = Instantiate(settings.prefab, spawnPosition, spawnRotation, settings.folder.transform);
                spawnedPrefab.transform.localScale = Vector3.one * settings.scaleFactor;
            }
        }
    }

    public void DeletePrefabs(PrefabSettings settings)
    {
        if (settings.folder != null)
        {
            DestroyImmediate(settings.folder);
        }
    }
}
