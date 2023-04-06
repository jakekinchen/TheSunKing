using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetUp : MonoBehaviour {
	public enum StartCondition { InShip, OnBody }

	public StartCondition startCondition;
	public CelestialBody startBody;

	public GameObject spawnPoint;

	public GameObject crystal;

	void Start () {
		Ship ship = FindObjectOfType<Ship> ();
		PlayerController player = FindObjectOfType<PlayerController> ();

		if (startCondition == StartCondition.InShip) {
			ship.PilotShip ();
			ship.flightControls.ForcePlayerInInteractionZone ();
		} else if (startCondition == StartCondition.OnBody) {
			if (startBody) {
				Vector3 pointAbovePlanet = startBody.transform.position + Vector3.right * startBody.radius * 1.1f;
				player.transform.position = pointAbovePlanet;
				player.transform.position = spawnPoint.transform.position;
				//rotate player by 180 degrees
				player.transform.rotation = Quaternion.Euler (0, 180, 0);
				//align player with planet
				//player.transform.rotation = Quaternion.FromToRotation (Vector3.up, pointAbovePlanet - startBody.transform.position);
				//transform crystal to a random position near the player on the surface of the planet
				
				float distanceFromPlayer = 0.01f;
				Vector3 randomDirection = Random.onUnitSphere;
				Vector3 crystalPosition = player.transform.position + randomDirection * distanceFromPlayer;
				crystalPosition = startBody.transform.position + (crystalPosition - startBody.transform.position).normalized * startBody.radius;
				crystal.transform.position = crystalPosition;
				
				player.SetVelocity (startBody.initialVelocity);
				ship.transform.position = pointAbovePlanet + Vector3.right * 20;
				ship.SetVelocity (startBody.initialVelocity);
				ship.ToggleHatch ();
			}
		}
	}
}