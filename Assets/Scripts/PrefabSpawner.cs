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

    public int maxTotalAttempts = 10000;

    private void Awake()
    {
        //undoStack = new Stack<(PrefabSettings, GameObject)>(undoHistorySize);
    }

    public void GeneratePrefabs(PrefabSettings prefabSettings)
    {
        
        if (terrainObject == null || celestialBodyGenerator == null) return;

        MeshFilter terrainMeshFilter = terrainObject.GetComponent<MeshFilter>();
        if (terrainMeshFilter != null)
        {
            terrainMesh = terrainMeshFilter.sharedMesh;
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
        Debug.LogError("Terrain object does not have a MeshFilter component.");
       }
    }

    private void SpawnPrefabs(PrefabSettings settings)
    {
        float oceanRadius = celestialBodyGenerator.GetOceanRadius();

        int layerMask = LayerMask.GetMask("Body");
        int totalAttempts = 0; 
        int maxAttempts = 10;
        
         List<Vector3> spawnPositions;
        
        if (settings.usePoissonDiskSampling)
        {
            spawnPositions = GeneratePoissonDiskSamplingPositions(settings);
        }
        else
        {
        for (int i = 0; i < settings.numberOfInstances; i++)
        {
            int randomIndex = Random.Range(0, vertices.Length);
            Vector3 spawnPosition = celestialBodyTransform.TransformPoint(vertices[randomIndex]) + celestialBodyTransform.TransformDirection(vertices[randomIndex]) * settings.distanceFromSurface;
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, celestialBodyTransform.TransformDirection(vertices[randomIndex]));

            Vector3 worldSpaceVertex = celestialBodyTransform.TransformPoint(vertices[randomIndex]);
            float terrainHeight = (worldSpaceVertex - celestialBodyTransform.position).magnitude;

            bool spawnOnLand = settings.spawnOnLand && terrainHeight > oceanRadius && terrainHeight <= oceanRadius + 50f;
            bool spawnOnOcean = settings.spawnOnOcean && terrainHeight <= oceanRadius;
            bool spawnOnCustomRange = settings.useCustomHeightRange && terrainHeight >= settings.minHeight && terrainHeight <= settings.maxHeight;

            if (spawnOnLand || spawnOnOcean || spawnOnCustomRange)
            {
                int attempt = 0;
                while (attempt < maxAttempts && totalAttempts < maxTotalAttempts)
                {
                    if ( !Physics.CheckSphere(spawnPosition, settings.prefab.transform.localScale.x * settings.scaleFactor / 2f, layerMask)
                    
                      )
                    {
                        GameObject spawnedPrefab = Instantiate(settings.prefab, spawnPosition, spawnRotation, settings.parentFolder.transform);
                        spawnedPrefab.name = settings.prefab.name+"_"+i;
                        spawnedPrefab.transform.localScale = Vector3.one * settings.scaleFactor;
                        break;
                    }
                    else
                    {
                        randomIndex = Random.Range(0, vertices.Length);
                        spawnPosition = celestialBodyTransform.TransformPoint(vertices[randomIndex]) + celestialBodyTransform.TransformDirection(vertices[randomIndex]) * settings.distanceFromSurface;
                        spawnRotation = Quaternion.FromToRotation(Vector3.up, celestialBodyTransform.TransformDirection(vertices[randomIndex]));
                        attempt++;
                        totalAttempts++;
                    }
                }
                if (totalAttempts >= maxTotalAttempts)
                {
                    Debug.LogWarning("Could not spawn all prefabs after reaching the maximum total attempts.");
                    break; // Exit the loop if the fail-safe limit is reached
                }

            } 
        }
        }
    }

private List<Vector3> GeneratePoissonDiskSamplingPositions(PrefabSettings settings)
{
    Bounds bounds = terrainMesh.bounds;
    List<Vector3> samplePoints = PoissonDiskSampling.GeneratePoints(settings.poissonDiskRadius, new Vector2(bounds.size.x, bounds.size.z), settings.minHeight, settings.maxHeight, settings.poissonDiskMaxSamples);
    return samplePoints;
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