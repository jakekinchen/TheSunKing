using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabSettings
{
    public GameObject prefab;
    public bool usePoissonDiskSampling;
    public float poissonDiskRadius = 5.0f;
    public int poissonDiskMaxSamples = 30;
    public int numberOfInstances;
    public float distanceFromSurface;
    public float scaleFactor = 1.0f;
    public bool spawnOnLand;
    public bool spawnOnOcean;
    public bool useCustomHeightRange;
    public float minHeight;
    public float maxHeight;
    [HideInInspector]
    public GameObject parentFolder;
}

public class PrefabSpawner : MonoBehaviour
{
    public GameObject terrainObject;
    public CelestialBodyGenerator celestialBodyGenerator;
    public List<PrefabSettings> prefabsSettings;

    private Mesh terrainMesh;
    private Vector3[] vertices;
    private Transform celestialBodyTransform;


    private void Awake()
    {
        if (terrainObject == null || celestialBodyGenerator == null) return;
        MeshCollider terrainMeshCollider = terrainObject.GetComponent<MeshCollider>();
        PoissonDiskSampling sampling = gameObject.AddComponent<PoissonDiskSampling>();
        sampling.Initialize(terrainMeshCollider); // Fix this line
    }

    public void GeneratePrefabs(PrefabSettings prefabSettings)
    {
        
        if (terrainObject == null || celestialBodyGenerator == null) return;

        MeshCollider terrainMeshCollider = terrainObject.GetComponent<MeshCollider>();
        if (terrainMeshCollider != null)
        {
            terrainMesh = terrainMeshCollider.sharedMesh;
            vertices = terrainMesh.vertices;
            celestialBodyTransform = terrainObject.transform.parent;

            if (prefabSettings.parentFolder == null)
            {   
                float oceanRadius = celestialBodyGenerator.GetOceanRadius();
                bool ocean = prefabSettings.spawnOnOcean && oceanRadius > 0;
                bool land = prefabSettings.spawnOnLand;
                bool both = ocean && land;
                bool customHeightRange = prefabSettings.useCustomHeightRange;
                string folderTag = (!(ocean || land) ? "" : both ? "Ocean+Land" : ocean ? "Ocean" : "Land") + (customHeightRange ? " (" + prefabSettings.minHeight + "-" + prefabSettings.maxHeight + ")" : "");
                string parentFolderName = prefabSettings.prefab.name.StartsWith("SM_") ? prefabSettings.prefab.name.Substring(3) : prefabSettings.prefab.name;
                parentFolderName += $" ({prefabSettings.numberOfInstances}) ({folderTag})";
                prefabSettings.parentFolder = new GameObject(parentFolderName);

                // Set the parent to the "Clones" folder or create it if it doesn't exist
                Transform clonesParent = celestialBodyTransform.Find("Clones");
                if (clonesParent == null)
                {
                    clonesParent = new GameObject("Clones").transform;
                    clonesParent.parent = celestialBodyTransform;
                }
                prefabSettings.parentFolder.transform.parent = clonesParent;
            }
            SpawnPrefabs(prefabSettings);
        }    
        else
       {
        Debug.LogError("Terrain object does not have a MeshCollider component.");
       }
    }

    private void SpawnPrefabs(PrefabSettings settings)
{
    float oceanRadius = celestialBodyGenerator.GetOceanRadius();
    
    int layerMask = LayerMask.GetMask("Body");
    
    List<Vector3> spawnPositions;
    
    if (settings.usePoissonDiskSampling)
    {
        spawnPositions = GeneratePoissonDiskSamplingPositions(settings);
        Debug.Log("Generated " + spawnPositions.Count + " spawn positions using Poisson Disk Sampling.");
        foreach (Vector3 spawnPosition in spawnPositions)
        {
            Debug.Log("Spawned " + settings.prefab.name + " at " + spawnPosition);
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, celestialBodyTransform.TransformDirection(spawnPosition - celestialBodyTransform.position));
            float terrainHeight = (spawnPosition - celestialBodyTransform.position).magnitude;
            InstantiatePrefab(settings, spawnPosition, spawnRotation, layerMask);
        }
    }
    else
    {
        for (int i = 0; i < settings.numberOfInstances; i++)
        {
            int randomIndex = Random.Range(0, vertices.Length);
            Vector3 spawnPosition = celestialBodyTransform.TransformPoint(vertices[randomIndex]) + celestialBodyTransform.TransformDirection(vertices[randomIndex]) * settings.distanceFromSurface;
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, celestialBodyTransform.TransformDirection(vertices[randomIndex]));
            InstantiatePrefab(settings, spawnPosition, spawnRotation, layerMask);
        }
    }
}

private void InstantiatePrefab(PrefabSettings settings, Vector3 spawnPosition, Quaternion spawnRotation, int layerMask)
{
    float rayHeight = 1000f; // Set a large enough value to ensure the ray starts above the terrain
    Vector3 rayOrigin = new Vector3(spawnPosition.x, rayHeight, spawnPosition.z);
    RaycastHit hit;

    if (Physics.Raycast(rayOrigin, Vector3.down, out hit, Mathf.Infinity, layerMask) && !Physics.CheckSphere(hit.point, settings.prefab.transform.localScale.x * settings.scaleFactor / 2f, layerMask, QueryTriggerInteraction.Ignore))
    {
        GameObject spawnedPrefab = Instantiate(settings.prefab, hit.point + hit.normal * settings.distanceFromSurface, spawnRotation, settings.parentFolder.transform);
        spawnedPrefab.name = settings.prefab.name + "_" + settings.parentFolder.transform.childCount;
        spawnedPrefab.transform.localScale = Vector3.one * settings.scaleFactor;
    }
    else {
        Debug.Log("Prefab " + settings.prefab.name + " could not be spawned at " + spawnPosition + " because it would overlap with another object or terrain was not detected.");
        Debug.DrawRay(rayOrigin, Vector3.down * 1000f, Color.red, 1000f);
    }
}

private List<Vector3> GeneratePoissonDiskSamplingPositions(PrefabSettings settings)
{
    Bounds bounds = terrainMesh.bounds;
    Vector3 sampleRegionSize = new Vector3(bounds.size.x, bounds.size.y, bounds.size.z);

    MeshCollider terrainMeshCollider = terrainObject.GetComponent<MeshCollider>();

    float adjustedRadius = settings.poissonDiskRadius * settings.scaleFactor;
    float adjustedCellSize = adjustedRadius / Mathf.Sqrt(3);

    // Add PoissonDiskSampling as a component
    PoissonDiskSampling poissonDiskSampling = gameObject.AddComponent<PoissonDiskSampling>();
    poissonDiskSampling.Initialize(terrainMeshCollider);
    poissonDiskSampling.radius = adjustedRadius;
    poissonDiskSampling.cellSize = adjustedCellSize;
    poissonDiskSampling.sampleRegionSize = sampleRegionSize;
    poissonDiskSampling.numSamplesBeforeRejection = settings.poissonDiskMaxSamples;

    float minHeight = settings.useCustomHeightRange ? settings.minHeight : 0;
    float maxHeight = settings.useCustomHeightRange ? settings.maxHeight : Mathf.Max(bounds.size.y, settings.distanceFromSurface);

    Bounds terrainBounds = terrainMesh.bounds;
    Vector3 terrainCenter = celestialBodyTransform.TransformPoint(terrainBounds.center);

    // Generate Poisson Disk Sampling positions
    (List<Vector3> points, Vector3 _) = poissonDiskSampling.GeneratePoints(
        settings.poissonDiskRadius, sampleRegionSize, minHeight, maxHeight, settings.poissonDiskMaxSamples, terrainCenter, terrainMeshCollider
    );
    // Remove PoissonDiskSampling component after generating points
    DestroyImmediate(poissonDiskSampling);

    return points;
}



    public void DeletePrefabs(PrefabSettings settings)
    {
        if (settings.parentFolder != null)
        {
            DestroyImmediate(settings.parentFolder);
            settings.parentFolder = null;
        }
    }

    
}