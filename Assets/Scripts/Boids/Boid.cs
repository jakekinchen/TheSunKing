using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

    public bool useOptimizedAvoidanceDetection = false;

    public GameObject planetObject;
    public Transform planet;
    

    BoidSettings settings;

    // State
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 forward;
    Vector3 velocity;

    // To update:
    Vector3 acceleration;
    [HideInInspector]
    public Vector3 avgFlockHeading;
    [HideInInspector]
    public Vector3 avgAvoidanceHeading;
    [HideInInspector]
    public Vector3 centreOfFlockmates;
    [HideInInspector]
    public int numPerceivedFlockmates;

    // Cached
    Material material;
    Transform cachedTransform;
    Transform target;

    void Awake () {
        planetObject = GameObject.Find("Body Simulation/Humble Abode");
        planet = planetObject.transform;
        material = transform.GetComponentInChildren<MeshRenderer> ().material;
        cachedTransform = transform;
    }

    public void SetPlanet(Transform planetTransform)
{
    planet = planetTransform;
}
Vector3 OptimizedAvoidanceRays()
{
    Vector3[] rayDirections = BoidHelper.directions;

    LayerMask bodyLayerMask = LayerMask.GetMask("Body");

    for (int i = 0; i < rayDirections.Length; i++)
    {
        Vector3 dir = cachedTransform.TransformDirection(rayDirections[i]);
        Ray ray = new Ray(position, dir);

         // Add the following lines for debug visualization
        Debug.DrawRay(ray.origin, ray.direction * settings.collisionAvoidDst, Color.red, 0.1f);

        if (!Physics.SphereCast(ray, settings.boundsRadius, settings.collisionAvoidDst, bodyLayerMask))
        {
            return dir;
        }
    }

    return forward;
}

    public void Initialize (BoidSettings settings, Transform target) {
        this.target = target;
        this.settings = settings;

        position = cachedTransform.position;
        forward = cachedTransform.forward;

        float startSpeed = (settings.minSpeed + settings.maxSpeed) / 2;
        velocity = transform.forward * startSpeed;
    }

    public void SetColour (Color col) {
        if (material != null) {
            material.color = col;
        }
    }

    public void UpdateBoid () {

        
        Vector3 acceleration = Vector3.zero;

         Vector3 gravityDirection = (planet.position - position).normalized;      
         acceleration += gravityDirection * settings.gravityStrength;
         Debug.DrawRay(position, gravityDirection, Color.red, 100f);


        if (target != null) {
            Vector3 offsetToTarget = (target.position - position);
            acceleration = SteerTowards (offsetToTarget) * settings.targetWeight;
        }

        if (numPerceivedFlockmates != 0) {
            centreOfFlockmates /= numPerceivedFlockmates;

            Vector3 offsetToFlockmatesCentre = (centreOfFlockmates - position);

            var alignmentForce = SteerTowards (avgFlockHeading) * settings.alignWeight;
            var cohesionForce = SteerTowards (offsetToFlockmatesCentre) * settings.cohesionWeight;
            var seperationForce = SteerTowards (avgAvoidanceHeading) * settings.seperateWeight;

            acceleration += alignmentForce;
            acceleration += cohesionForce;
            acceleration += seperationForce;
        }

        if (IsHeadingForCollision())
    {
        Vector3 collisionAvoidDir;
        if (useOptimizedAvoidanceDetection)
        {
            collisionAvoidDir = OptimizedAvoidanceRays();
        }
        else
        {
            collisionAvoidDir = ObstacleRays();
        }
        Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * settings.avoidCollisionWeight;
        acceleration += collisionAvoidForce;
    }

        if (planet != null)
        {
            Vector3 toCenter = (planet.position - position).normalized;
            Vector3 surfaceVelocity = Vector3.ProjectOnPlane(velocity, toCenter);
            velocity = surfaceVelocity * (1 - settings.surfaceAdherence) + velocity * settings.surfaceAdherence;

            // Calculate the surface separation force
            Vector3 surfaceSeparationForce = Vector3.zero;
            if (numPerceivedFlockmates != 0)
            {
                Vector3 avgSurfaceAvoidanceHeading = avgAvoidanceHeading - Vector3.Dot(avgAvoidanceHeading, toCenter) * toCenter;
                surfaceSeparationForce = SteerTowards(avgSurfaceAvoidanceHeading) * settings.surfaceSeparationWeight;
            }
            acceleration += surfaceSeparationForce;
        }

        velocity += acceleration * Time.deltaTime;
        float speed = velocity.magnitude;
        Vector3 dir = velocity / speed;
        speed = Mathf.Clamp (speed, settings.minSpeed, settings.maxSpeed);
        velocity = dir * speed;

        cachedTransform.position += velocity * Time.deltaTime;
        Vector3 localUp = (position - planet.position).normalized;
Quaternion targetRotation = Quaternion.LookRotation(dir, localUp);
cachedTransform.rotation = Quaternion.Slerp(cachedTransform.rotation, targetRotation, settings.rotationSpeed * Time.deltaTime);

        position = cachedTransform.position;
        forward = dir;
    }

    bool IsHeadingForCollision () {
        RaycastHit hit;
        if (Physics.SphereCast (position, settings.boundsRadius, forward, out hit, settings.collisionAvoidDst, settings.obstacleMask)) {
           
            return true;
        } else { }
        return false;
    }

    Vector3 ObstacleRays () {
        Vector3[] rayDirections = BoidHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++) {
            Vector3 dir = cachedTransform.TransformDirection (rayDirections[i]);
            Ray ray = new Ray (position, dir);
            if (!Physics.SphereCast (ray, settings.boundsRadius, settings.collisionAvoidDst, settings.obstacleMask)) {
                return dir;
            }
        }

        return forward;
    }

    Vector3 SteerTowards (Vector3 vector) {
        Vector3 v = vector.normalized * settings.maxSpeed - velocity;
        return Vector3.ClampMagnitude (v, settings.maxSteerForce);
    }

}