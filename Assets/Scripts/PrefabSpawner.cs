using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject terrainObject;
    public GameObject prefab;
    public int numberOfInstances;
    public float distanceFromSurface;

    private Mesh terrainMesh;
    private Vector3[] vertices;
    private Transform celestialBodyTransform;

    void Start()
    {
        MeshFilter terrainMeshFilter = terrainObject.GetComponent<MeshFilter>();
        if (terrainMeshFilter != null)
        {
            terrainMesh = terrainMeshFilter.sharedMesh;
            vertices = terrainMesh.vertices;
            celestialBodyTransform = terrainObject.transform.parent;

            GeneratePrefabs(); // Change the method name here
        }
        else
        {
            Debug.LogError("Terrain object does not have a MeshFilter component.");
        }
    }

    public void GeneratePrefabs() // Change the method name here
    {
        for (int i = 0; i < numberOfInstances; i++)
        {
            int randomIndex = Random.Range(0, vertices.Length);
            Vector3 spawnPosition = celestialBodyTransform.TransformPoint(vertices[randomIndex]) + celestialBodyTransform.TransformDirection(vertices[randomIndex]) * distanceFromSurface;
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, celestialBodyTransform.TransformDirection(vertices[randomIndex]));

            Instantiate(prefab, spawnPosition, spawnRotation, celestialBodyTransform);
        }
    }
}
