using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof (Rigidbody))]
public class CelestialBody : GravityObject {


    public enum BodyType { Planet, Moon, Sun }
    public BodyType bodyType;
    public float radius;
    public float surfaceGravity;
    public Vector3 initialVelocity;
    public string bodyName = "Unnamed";
    Transform meshHolder;

    public Vector3 velocity { get; set; }
    public float mass { get; set; }

    Rigidbody rb;

    public float oceanRadius = 200f;
    public float atmosphereRadius = 250f;
    public float gravityStrength = 1f; // Strength of the gravity force pulling the player towards the celestial body
    public PlayerController player;
    public Transform playerTransform;
    public Rigidbody playerRigidbody; // Reference to the player's Rigidbody component
    

    private void FixedUpdate()
    {
        // Apply gravity force when inside the atmosphere
        if (IsPlayerInsideAtmosphere())
        {
            Vector3 gravityDirection = (transform.position - playerTransform.position).normalized;
            //playerRigidbody.AddForce(gravityDirection * gravityStrength, ForceMode.Acceleration);
        }
    }

    public void PlayerEnteredOcean()
    {
        float playerDistance = Vector3.Distance(playerTransform.position, transform.position);
        if (playerDistance < oceanRadius-1f)
        {
            Debug.Log("Player entered ocean");
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            playerTransform.position = transform.position + direction * oceanRadius;
        }
    }

    public bool IsPlayerInsideAtmosphere()
    {
        float playerDistance = Vector3.Distance(playerTransform.position, transform.position);
        return playerDistance <= atmosphereRadius;
    }

    public void PlayerEnteredAtmosphere()
    {
        float playerDistance = Vector3.Distance(playerTransform.position, transform.position);
        if (playerDistance < oceanRadius-1f)
        {
            Debug.Log("Player entered ocean");
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            playerTransform.position = transform.position + direction * oceanRadius;
        }
    }

    public void PlayerExitedAtmosphere()
    {
        float playerDistance = Vector3.Distance(playerTransform.position, transform.position);
        if (playerDistance > atmosphereRadius)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            playerTransform.position = transform.position + direction * atmosphereRadius;
        }
    }

    void Awake () {
        if (playerTransform && playerRigidbody == null)
        {
            player = FindObjectOfType<PlayerController>();
            playerRigidbody = player.GetComponent<Rigidbody>();
            playerTransform = player.transform;
        }
        rb = GetComponent<Rigidbody> ();
        velocity = initialVelocity;
        RecalculateMass ();
    }

    public void UpdateVelocity (CelestialBody[] allBodies, float timeStep) {
        foreach (var otherBody in allBodies) {
            if (otherBody != this) {
                float sqrDst = (otherBody.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.rb.position - rb.position).normalized;

                Vector3 acceleration = forceDir * Universe.gravitationalConstant * otherBody.mass / sqrDst;
                velocity += acceleration * timeStep;
            }
        }
    }

    public void UpdateVelocity (Vector3 acceleration, float timeStep) {
        velocity += acceleration * timeStep;
    }

    public void UpdatePosition (float timeStep) {
        rb.MovePosition (rb.position + velocity * timeStep);

    }

    void OnValidate () {
        RecalculateMass ();
        if (GetComponentInChildren<CelestialBodyGenerator> ()) {
            GetComponentInChildren<CelestialBodyGenerator> ().transform.localScale = Vector3.one * radius;
        }
        gameObject.name = bodyName;
    }

    public void RecalculateMass () {
        mass = surfaceGravity * radius * radius / Universe.gravitationalConstant;
        Rigidbody.mass = mass;
    }

    public Rigidbody Rigidbody {
        get {
            if (!rb) {
                rb = GetComponent<Rigidbody> ();
            }
            return rb;
        }
    }

    public Vector3 Position {
        get {
            return rb.position;
        }
    }

    public Vector3 AngularVelocity(Vector3 spawnPoint)
{
    Vector3 shipToPlanet = spawnPoint - rb.position;
    float distance = shipToPlanet.magnitude;
    Vector3 localUp = shipToPlanet.normalized;
    Vector3 localEast = Vector3.Cross(localUp, Vector3.up).normalized;

    float circumference = 2 * Mathf.PI * distance;
    float orbitPeriod = circumference / initialVelocity.magnitude;

    return (2 * Mathf.PI / orbitPeriod) * localEast;
}

}