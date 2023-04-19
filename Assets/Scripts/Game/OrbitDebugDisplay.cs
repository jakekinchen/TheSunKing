using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class OrbitDebugDisplay : MonoBehaviour {

    public bool useCollision = false;
    public int numSteps = 1000;
    public float timeStep = 0.1f;
    public bool usePhysicsTimeStep;

    public bool relativeToBody;
    public CelestialBody centralBody;
    public float width = 100;
    public bool useThickLines;

    private Dictionary<int, Color[]> lineColors = new Dictionary<int, Color[]>();

    void Start () {
        if (Application.isPlaying) {
            HideOrbits ();
        }
    }

    void Update () {

        if (!Application.isPlaying) {
            if (useCollision){
                DrawOrbitCollisions ();
            }else{
                DrawOrbits ();
            }
        }
    }

    void DrawOrbits () {
        CelestialBody[] bodies = FindObjectsOfType<CelestialBody> ();
        var virtualBodies = new VirtualBody[bodies.Length];
        var drawPoints = new Vector3[bodies.Length][];
        int referenceFrameIndex = 0;
        Vector3 referenceBodyInitialPosition = Vector3.zero;

        // Initialize virtual bodies (don't want to move the actual bodies)
        for (int i = 0; i < virtualBodies.Length; i++) {
            virtualBodies[i] = new VirtualBody (bodies[i]);
            drawPoints[i] = new Vector3[numSteps];

            if (bodies[i] == centralBody && relativeToBody) {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = virtualBodies[i].position;
            }
        }

        // Simulate
        for (int step = 0; step < numSteps; step++) {
            Vector3 referenceBodyPosition = (relativeToBody) ? virtualBodies[referenceFrameIndex].position : Vector3.zero;
            // Update velocities
            for (int i = 0; i < virtualBodies.Length; i++) {
                virtualBodies[i].velocity += CalculateAcceleration (i, virtualBodies) * timeStep;
            }
            // Update positions
            for (int i = 0; i < virtualBodies.Length; i++) {
                Vector3 newPos = virtualBodies[i].position + virtualBodies[i].velocity * timeStep;
                virtualBodies[i].position = newPos;
                if (relativeToBody) {
                    var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                    newPos -= referenceFrameOffset;
                }
                if (relativeToBody && i == referenceFrameIndex) {
                    newPos = referenceBodyInitialPosition;
                }

                drawPoints[i][step] = newPos;
            }
        }

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++) {
            var pathColour = bodies[bodyIndex].gameObject.GetComponentInChildren<MeshRenderer> ().sharedMaterial.color; //

            if (useThickLines) {
                var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer> ();
                lineRenderer.enabled = true;
                lineRenderer.positionCount = drawPoints[bodyIndex].Length;
                lineRenderer.SetPositions (drawPoints[bodyIndex]);
                lineRenderer.startColor = pathColour;
                lineRenderer.endColor = pathColour;
                lineRenderer.widthMultiplier = width;
            } else {
                for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++) {
                    Debug.DrawLine (drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], pathColour);
                }

                // Hide renderer
                var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer> ();
                if (lineRenderer) {
                    lineRenderer.enabled = false;
                }
            }

        }
    }

    Vector3 CalculateAcceleration (int i, VirtualBody[] virtualBodies) {
        Vector3 acceleration = Vector3.zero;
        for (int j = 0; j < virtualBodies.Length; j++) {
            if (i == j) {
                continue;
            }
            Vector3 forceDir = (virtualBodies[j].position - virtualBodies[i].position).normalized;
            float sqrDst = (virtualBodies[j].position - virtualBodies[i].position).sqrMagnitude;
            acceleration += forceDir * Universe.gravitationalConstant * virtualBodies[j].mass / sqrDst;
        }
        return acceleration;
    }

    void HideOrbits () {
        CelestialBody[] bodies = FindObjectsOfType<CelestialBody> ();

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < bodies.Length; bodyIndex++) {
            var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer> ();
            lineRenderer.positionCount = 0;
        }
    }

    void OnValidate () {
        if (usePhysicsTimeStep) {
            timeStep = Universe.physicsTimeStep;
        }
    }

    class VirtualBody {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public VirtualBody (CelestialBody body) {
            position = body.transform.position;
            velocity = body.initialVelocity;
            mass = body.mass;
        }
    }


void DrawOrbitCollisions() {
    CelestialBody[] bodies = FindObjectsOfType<CelestialBody>();
    var virtualBodies = new VirtualBody[bodies.Length];
    var drawPoints = new Vector3[bodies.Length][];
    int referenceFrameIndex = 0;
    Vector3 referenceBodyInitialPosition = Vector3.zero;

    // Initialize virtual bodies (don't want to move the actual bodies)
    for (int i = 0; i < virtualBodies.Length; i++) {
        virtualBodies[i] = new VirtualBody(bodies[i]);
        drawPoints[i] = new Vector3[numSteps];

        if (bodies[i] == centralBody && relativeToBody) {
            referenceFrameIndex = i;
            referenceBodyInitialPosition = virtualBodies[i].position;
        }
    }

    // Simulate
    for (int step = 0; step < numSteps; step++) {
        Vector3 referenceBodyPosition = (relativeToBody) ? virtualBodies[referenceFrameIndex].position : Vector3.zero;
        // Update velocities
        for (int i = 0; i < virtualBodies.Length; i++) {
            virtualBodies[i].velocity += CalculateAcceleration(i, virtualBodies) * timeStep;
        }
        // Update positions
        for (int i = 0; i < virtualBodies.Length; i++) {
            Vector3 newPos = virtualBodies[i].position + virtualBodies[i].velocity * timeStep;
            virtualBodies[i].position = newPos;
            if (relativeToBody) {
                var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                newPos -= referenceFrameOffset;
            }
            if (relativeToBody && i == referenceFrameIndex) {
                newPos = referenceBodyInitialPosition;
            }

            drawPoints[i][step] = newPos;
        }
    }

    // Calculate line segment colors based on proximity
    for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++) {
        lineColors[bodyIndex] = new Color[numSteps - 1];
        for (int step = 0; step < numSteps - 1; step++) {
            Color segmentColor = Color.green;
            for (int otherIndex = 0; otherIndex < virtualBodies.Length; otherIndex++) {
                if (bodyIndex == otherIndex) continue;

                float distance = Vector3.Distance(drawPoints[bodyIndex][step], drawPoints[otherIndex][step]);
                if (distance < 2 * virtualBodies[bodyIndex].mass) { // mass can be used as a proxy for the radius
                    segmentColor = Color.red;
                    break;
                }
            }
            lineColors[bodyIndex][step] = segmentColor;
        }
    }

        // Draw paths
    for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++) {
        var initialColor = bodies[bodyIndex].gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.color;

        for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++) {
            // Check for collisions
            bool collisionDetected = false;
            for (int j = 0; j < virtualBodies.Length; j++) {
                if (bodyIndex == j) continue;

                float distance = (drawPoints[bodyIndex][i] - drawPoints[j][i]).magnitude;
                float combinedRadius = .25f * (Mathf.Pow(virtualBodies[bodyIndex].mass, 1 / 3f) + Mathf.Pow(virtualBodies[j].mass, 1 / 3f));

                if ((distance <= combinedRadius)&& (i != 0)&& (j != 0)) {
                    collisionDetected = true;
                    Debug.Log($"Collision detected between {bodies[bodyIndex].name} and {bodies[j].name} at step {i}");
                    break;
                }
            }

            Color currentLineColor = collisionDetected ? Color.red : initialColor;
            Debug.DrawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], currentLineColor);
        }

        // Hide renderer
        var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
        if (lineRenderer) {
            lineRenderer.enabled = false;
        }
    }

    }
}