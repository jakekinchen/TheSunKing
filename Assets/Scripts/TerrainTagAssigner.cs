using System.Collections;
using UnityEngine;

public class TerrainTagAssigner : MonoBehaviour
{
    public string planetName = "Humble Abode";
    public float delayInSeconds = 1f;

    private IEnumerator Start()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayInSeconds);

        // Find the Humble Abode GameObject
        GameObject planet = GameObject.Find(planetName);

        if (planet != null)
        {
            // Find the Body Generator object in the children of the Humble Abode
            Transform bodyGeneratorTransform = planet.transform.Find("Body Generator");

            if (bodyGeneratorTransform != null)
            {
                // Find the terrain mesh object in the children of the Body Generator
                Transform terrainMeshTransform = bodyGeneratorTransform.Find("Terrain Mesh");

                // If the terrain mesh object is found, assign the 'Terrain' tag
                if (terrainMeshTransform != null)
                {
                    GameObject terrainMeshObject = terrainMeshTransform.gameObject;
                    terrainMeshObject.tag = "Terrain";
                }
                else
                {
                    Debug.LogWarning("Terrain Mesh not found as a child of Body Generator.");
                }
            }
            else
            {
                Debug.LogWarning("Body Generator not found as a child of " + planetName + ".");
            }
        }
        else
        {
            Debug.LogWarning(planetName + " not found in the scene.");
        }
    }
}
