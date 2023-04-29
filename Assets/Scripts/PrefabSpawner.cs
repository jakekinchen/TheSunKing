using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int numberOfPrefabs;
    public float spawnRadius;
    public float heightOffset;
    public float rotationRandomness;

    [Range(0, 1)]
    public float scaleMin;
    [Range(0, 1)]
    public float scaleMax;

    public LayerMask terrainLayer;

    public void SpawnPrefabs()
    {
        if (prefab == null) return;

        for (int i = 0; i < numberOfPrefabs; i++)
        {
            Vector3 randomPointOnSphere = Random.onUnitSphere * spawnRadius;
            Vector3 spawnPosition = transform.position + randomPointOnSphere;

            RaycastHit hit;
            if (Physics.Raycast(spawnPosition + Vector3.up * 500f, Vector3.down, out hit, 1000f, terrainLayer))
            {
                spawnPosition = hit.point + Vector3.up * heightOffset;

                Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                Quaternion randomRotation = Quaternion.Euler(
                    Random.Range(-rotationRandomness, rotationRandomness),
                    Random.Range(-rotationRandomness, rotationRandomness),
                    Random.Range(-rotationRandomness, rotationRandomness)
                );

                GameObject newPrefab = Instantiate(prefab, spawnPosition, surfaceRotation * randomRotation);
                newPrefab.transform.parent = transform;

                float randomScale = Random.Range(scaleMin, scaleMax);
                newPrefab.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            }
        }
    }
}
