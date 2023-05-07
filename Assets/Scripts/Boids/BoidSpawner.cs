using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour {

    public enum GizmoType { Never, SelectedOnly, Always }

    public Boid prefab;
    public float spawnRadius = 10;
    public int spawnCount = 10;
    public Color colour;
    public GizmoType showSpawnRegion;

    public bool spawnInOcean = false;
    public float oceanHeightMin = 0f;
    public float oceanHeightMax = 200f;
    public float landHeightMin = 200f;
    public float landHeightMax = 260f;

    void Awake () {
        for (int i = 0; i < spawnCount; i++) {
            Vector3 randomPos = Random.insideUnitSphere * spawnRadius;

            if (spawnInOcean) {
                randomPos.y = Mathf.Clamp(randomPos.y, oceanHeightMin, oceanHeightMax);
            }

            Vector3 pos = transform.position + randomPos;
            Boid boid = Instantiate(prefab, pos, Quaternion.identity, transform);

            Vector3 toPlanetCenter = (boid.planet.position - pos).normalized;
            Vector3 boidForward = Random.onUnitSphere;
            Vector3 boidUp = Vector3.Cross(boidForward, toPlanetCenter).normalized;
            boidForward = Vector3.Cross(toPlanetCenter, boidUp).normalized;
            boid.transform.rotation = Quaternion.LookRotation(boidForward, toPlanetCenter);


            boid.SetColour(colour);
            boid.SetPlanet(boid.planet.transform);
            if (spawnInOcean) {
                boid.SetHeight(oceanHeightMin, oceanHeightMax);
            }else{
                boid.SetHeight(landHeightMin, landHeightMax);
            }
            //Debug.Log("Planet: " + boid.planet);
        }
    }

    private void OnDrawGizmos () {
        if (showSpawnRegion == GizmoType.Always) {
            DrawGizmos ();
        }
    }

    void OnDrawGizmosSelected () {
        if (showSpawnRegion == GizmoType.SelectedOnly) {
            DrawGizmos ();
        }
    }

    void DrawGizmos () {

        Gizmos.color = new Color (colour.r, colour.g, colour.b, 0.3f);
        Gizmos.DrawSphere (transform.position, spawnRadius);
    }

}