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
}

public class PrefabSpawner : MonoBehaviour
{
    public GameObject terrainObject;
    public List<PrefabSettings> prefabsSettings;

    private Mesh terrainMesh;
    private Vector3[] vertices;
    private Transform celestialBodyTransform;

    private void OnEnable()
    {
        GeneratePrefabs();
    }

    public void GeneratePrefabs()
    {
        if (terrainObject == null) return;

        MeshFilter terrainMeshFilter = terrainObject.GetComponent<MeshFilter>();
        if (terrainMeshFilter != null)
        {
            terrainMesh = terrainMeshFilter.sharedMesh;
            vertices = terrainMesh.vertices;
            celestialBodyTransform = terrainObject.transform.parent;

            foreach (var prefabSettings in prefabsSettings)
            {
                SpawnPrefabs(prefabSettings);
            }
        }
        else
        {
            Debug.LogError("Terrain object does not have a MeshFilter component.");
        }
    }

    private void SpawnPrefabs(PrefabSettings settings)
    {
        for (int i = 0; i < settings.numberOfInstances; i++)
        {
            int randomIndex = Random.Range(0, vertices.Length);
            Vector3 spawnPosition = celestialBodyTransform.TransformPoint(vertices[randomIndex]) + celestialBodyTransform.TransformDirection(vertices[randomIndex]) * settings.distanceFromSurface;
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, celestialBodyTransform.TransformDirection(vertices[randomIndex]));

            float terrainHeight = terrainMesh.bounds.extents.y * vertices[randomIndex].y;

            if (terrainHeight >= settings.minHeight && terrainHeight <= settings.maxHeight)
            {
                GameObject spawnedPrefab = Instantiate(settings.prefab, spawnPosition, spawnRotation, celestialBodyTransform);
                spawnedPrefab.transform.localScale = Vector3.one * settings.scaleFactor;
            }
        }
    }
}
