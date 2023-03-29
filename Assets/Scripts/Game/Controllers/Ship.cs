using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Ship : GravityObject {

	public InputSettings inputSettings;
	public Transform hatch;
	public float hatchAngle;
	public Transform camViewPoint;
	public Transform pilotSeatPoint;
	public LayerMask groundedMask;
	public GameObject window;
	public GameObject targetPlanet;

	public bool NewGravity = true;

	public bool Teleport = true;

	public GameObject SpawnPoint;

	//make a list of all the planets in the scene and select to assign to targetPlanet
	public List<CelestialBody> planets = new List<CelestialBody>();


	[Header ("Handling")]
	public float thrustStrength = 20;
	public float rotSpeed = 5;
	public float rollSpeed = 30;
	public float rotSmoothSpeed = 10;

	[Header ("Interact")]
	public Interactable flightControls;

	Rigidbody rb;
	Quaternion targetRot;
	Quaternion smoothedRot;

	Vector3 thrusterInput;
	PlayerController pilot;
	bool shipIsPiloted;
	int numCollisionTouches;
	bool hatchOpen;

	KeyCode ascendKey = KeyCode.Space;
	KeyCode descendKey = KeyCode.LeftShift;
	KeyCode rollCounterKey = KeyCode.Q;
	KeyCode rollClockwiseKey = KeyCode.E;
	KeyCode forwardKey = KeyCode.W;
	KeyCode backwardKey = KeyCode.S;
	KeyCode leftKey = KeyCode.A;
	KeyCode rightKey = KeyCode.D;

	void Awake () {
		InitRigidbody ();
		targetRot = transform.rotation;
		smoothedRot = transform.rotation;
		inputSettings.Begin ();
		if (Teleport) {
			//teleport to spawn point
			FindPlanet ();
		}
		
	}

	void Update () {
		if (shipIsPiloted) {
			HandleMovement ();
		}

		// Animate hatch
		float hatchTargetAngle = (hatchOpen) ? hatchAngle : 0;
		hatch.localEulerAngles = Vector3.right * Mathf.LerpAngle (hatch.localEulerAngles.x, hatchTargetAngle, Time.deltaTime);

		HandleCheats ();
	}

	void HandleMovement () {
		// Thruster input
		int thrustInputX = GetInputAxis (leftKey, rightKey);
		int thrustInputY = GetInputAxis (descendKey, ascendKey);
		int thrustInputZ = GetInputAxis (backwardKey, forwardKey);
		thrusterInput = new Vector3 (thrustInputX, thrustInputY, thrustInputZ);

		// Rotation input
		float yawInput = Input.GetAxisRaw ("Mouse X") * rotSpeed * inputSettings.mouseSensitivity / 100f;
		float pitchInput = Input.GetAxisRaw ("Mouse Y") * rotSpeed * inputSettings.mouseSensitivity / 100f;
		float rollInput = GetInputAxis (rollCounterKey, rollClockwiseKey) * rollSpeed * Time.deltaTime;

		// Calculate rotation
		if (numCollisionTouches == 0) {
			var yaw = Quaternion.AngleAxis (yawInput, transform.up);
			var pitch = Quaternion.AngleAxis (-pitchInput, transform.right);
			var roll = Quaternion.AngleAxis (-rollInput, transform.forward);

			targetRot = yaw * pitch * roll * targetRot;

			smoothedRot = Quaternion.Slerp (transform.rotation, targetRot, Time.deltaTime * rotSmoothSpeed);
		} else {
			targetRot = transform.rotation;
			smoothedRot = transform.rotation;
		}
	}

	void FixedUpdate () {
		// Gravity
		if (NewGravity) {CelestialBody[] bodies = NBodySimulation.Bodies;
    Vector3 gravityOfNearestBody = Vector3.zero;
    float nearestSurfaceDst = float.MaxValue;

    // Gravity
    foreach (CelestialBody body in bodies) {
        float sqrDst = (body.Position - rb.position).sqrMagnitude;
        Vector3 forceDir = (body.Position - rb.position).normalized;
        Vector3 acceleration = forceDir * Universe.gravitationalConstant * body.mass / sqrDst;
        rb.AddForce(acceleration, ForceMode.Acceleration);

        float dstToSurface = Mathf.Sqrt(sqrDst) - body.radius;

        // Find body with strongest gravitational pull
        if (dstToSurface < nearestSurfaceDst) {
            nearestSurfaceDst = dstToSurface;
            gravityOfNearestBody = acceleration;
        }
    }

    // Rotate to align with gravity up
    Vector3 gravityUp = -gravityOfNearestBody.normalized;
    rb.rotation = Quaternion.FromToRotation(transform.up, gravityUp) * rb.rotation;

    // Move
    //MoveShip();
	} else {
		Vector3 gravity = NBodySimulation.CalculateAcceleration (rb.position);
		rb.AddForce (gravity, ForceMode.Acceleration);

		// Thrusters
		Vector3 thrustDir = transform.TransformVector (thrusterInput);
		rb.AddForce (thrustDir * thrustStrength, ForceMode.Acceleration);

		if (numCollisionTouches == 0) {
			rb.MoveRotation (smoothedRot);
		}
	}
	
	}

	void TeleportToBody (CelestialBody body) {
		Vector3 shipToPlanet = rb.position - body.Position;
		Vector3 angularVelocity = body.AngularVelocity(rb.position);
		Vector3 surfaceVelocity = Vector3.Cross(angularVelocity, shipToPlanet);
		rb.velocity = surfaceVelocity;

		rb.MovePosition (body.transform.position + (transform.position - body.transform.position).normalized * body.radius * 1.15f);
		SpawnPoint = FindSpawnPoint(body);
		if (SpawnPoint != null) {
			 rb.MovePosition(SpawnPoint.transform.position);
		}
	}

	int GetInputAxis (KeyCode negativeAxis, KeyCode positiveAxis) {
		int axis = 0;
		if (Input.GetKey (positiveAxis)) {
			axis++;
		}
		if (Input.GetKey (negativeAxis)) {
			axis--;
		}
		return axis;
	}

	void HandleCheats () {
		if (Universe.cheatsEnabled) {
			if (Input.GetKeyDown (KeyCode.Return) && IsPiloted && Time.timeScale != 0) {
				var shipHud = FindObjectOfType<ShipHUD> ();
				if (shipHud.LockedBody) {
					TeleportToBody (shipHud.LockedBody);
				}
			}
		}
	}

	void InitRigidbody () {
		rb = GetComponent<Rigidbody> ();
		rb.interpolation = RigidbodyInterpolation.Interpolate;
		rb.useGravity = false;
		rb.isKinematic = false;
		rb.centerOfMass = Vector3.zero;
		rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
	}

	public void ToggleHatch () {
		hatchOpen = !hatchOpen;
	}

	public void TogglePiloting () {
		if (shipIsPiloted) {
			StopPilotingShip ();
		} else {
			PilotShip ();
		}
	}

	public void PilotShip () {
		pilot = FindObjectOfType<PlayerController> ();
		shipIsPiloted = true;
		pilot.Camera.transform.parent = camViewPoint;
		pilot.Camera.transform.localPosition = Vector3.zero;
		pilot.Camera.transform.localRotation = Quaternion.identity;
		pilot.gameObject.SetActive (false);
		hatchOpen = false;
		window.SetActive (false);

	}

	void StopPilotingShip () {
		shipIsPiloted = false;
		pilot.transform.position = pilotSeatPoint.position;
		pilot.transform.rotation = pilotSeatPoint.rotation;
		pilot.Rigidbody.velocity = rb.velocity;
		pilot.gameObject.SetActive (true);
		window.SetActive (true);
		pilot.ExitFromSpaceship ();
	}

	void OnCollisionEnter (Collision other) {
		if (groundedMask == (groundedMask | (1 << other.gameObject.layer))) {
			numCollisionTouches++;
		}
	}

	void OnCollisionExit (Collision other) {
		if (groundedMask == (groundedMask | (1 << other.gameObject.layer))) {
			numCollisionTouches--;
		}
	}

	public void SetVelocity (Vector3 velocity) {
		rb.velocity = velocity;
	}

	public bool ShowHUD {
		get {
			return shipIsPiloted;
		}
	}
	public bool HatchOpen {
		get {
			return hatchOpen;
		}
	}

	public bool IsPiloted {
		get {
			return shipIsPiloted;
		}
	}

	public Rigidbody Rigidbody {
		get {
			return rb;
		}
	}


	public void FindPlanet () {
    // Find the game object named Cyclops
    CelestialBody planet = targetPlanet.GetComponent<CelestialBody>();
    if (planet) {
        // Get the CelestialBody component from the found GameObject
            TeleportToBody(planet);
        } else {
            Debug.LogError("Null CelestialBody component attached to Ship's target planet.");
        }
	}

	public GameObject FindSpawnPoint(CelestialBody body)
{
    // Find all objects with the ShipSpawn tag that are children of the body object
    GameObject[] spawnPoints = body.transform.GetComponentsInChildren<Transform>()
        .Where(x => x.CompareTag("ShipSpawn"))
        .Select(x => x.gameObject)
        .ToArray();

    if (spawnPoints.Length > 0)
    {
        // Return the first found object
        return spawnPoints[0];
    }
    else
    {
        Debug.LogError("No object with the ShipSpawn tag found.");
        return null;
    }
}



}