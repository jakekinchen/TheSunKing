using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetUp : MonoBehaviour {
    public enum StartCondition { InShip, OnBody }

    public StartCondition startCondition;
    public CelestialBody startBody;

    public GameObject spawnPoint;

    
    public string planetName = "Humble Abode";
    public float delayInSeconds = 1f;

    void Start () {
        Ship ship = FindObjectOfType<Ship> ();
        PlayerController player = FindObjectOfType<PlayerController> ();

        if (startCondition == StartCondition.InShip) {
            ship.PilotShip ();
            ship.flightControls.ForcePlayerInInteractionZone ();
        } else if (startCondition == StartCondition.OnBody) {
            if (startBody) {
                StartCoroutine(AssignTerrainTagAndSpawnCrystal(player, ship));
            }
        }
    }

    private IEnumerator AssignTerrainTagAndSpawnCrystal(PlayerController player, Ship ship) {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayInSeconds);

        // Find the Humble Abode GameObject
        GameObject planet = GameObject.Find(planetName);

        if (planet != null) {
            // Find the Body Generator object in the children of the Humble Abode
            Transform bodyGeneratorTransform = planet.transform.Find("Body Generator");

            if (bodyGeneratorTransform != null) {
                // Find the terrain mesh object in the children of the Body Generator
                Transform terrainMeshTransform = bodyGeneratorTransform.Find("Terrain Mesh");

                // If the terrain mesh object is found, assign the 'Terrain' tag
                if (terrainMeshTransform != null) {
                    GameObject terrainMeshObject = terrainMeshTransform.gameObject;
                    terrainMeshObject.tag = "Terrain";

                    // Spawn the player and crystal on the terrain mesh
                    SpawnPlayerAndCrystal(player, ship);
                }
                else {
                    Debug.LogWarning("Terrain Mesh not found as a child of Body Generator.");
                }
            }
            else {
                Debug.LogWarning("Body Generator not found as a child of " + planetName + ".");
            }
        }
        else {
            Debug.LogWarning(planetName + " not found in the scene.");
        }
    }

   private void SpawnPlayerAndCrystal(PlayerController player, Ship ship)
{
    Vector3 pointAbovePlanet = startBody.transform.position + Vector3.right * startBody.radius * 1.1f;
    player.transform.position = pointAbovePlanet;
    player.transform.position = spawnPoint.transform.position;
    // Rotate player by 180 degrees
    player.transform.rotation = Quaternion.Euler(0, 180, 0);
    // Align player with the planet
    player.transform.rotation = Quaternion.FromToRotation(Vector3.up, pointAbovePlanet - startBody.transform.position);




    // Set up player and ship
    player.SetVelocity(startBody.initialVelocity);
    ship.transform.position = pointAbovePlanet + Vector3.right * 20;
    ship.SetVelocity(startBody.initialVelocity);
    ship.ToggleHatch();
}



}
