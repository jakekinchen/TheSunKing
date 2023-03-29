using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBodySimulation : MonoBehaviour {
    CelestialBody[] bodies;
    static NBodySimulation instance;
    public bool pauseSimulation = false; // new boolean variable
    public bool useRungeKutta = false; // new boolean variable


    void Awake () {

        bodies = FindObjectsOfType<CelestialBody> ();
        Time.fixedDeltaTime = Universe.physicsTimeStep;
        Debug.Log ("Setting fixedDeltaTime to: " + Universe.physicsTimeStep);
    }

    void FixedUpdate () {
         if (!pauseSimulation) { // only update positions and velocities if pauseSimulation is false
         if (!useRungeKutta){
        for (int i = 0; i < bodies.Length; i++) {
            Vector3 acceleration = CalculateAcceleration (bodies[i].Position, bodies[i]);
            bodies[i].UpdateVelocity (acceleration, Universe.physicsTimeStep);
            //bodies[i].UpdateVelocity (bodies, Universe.physicsTimeStep);
        }

        for (int i = 0; i < bodies.Length; i++) {
            bodies[i].UpdatePosition (Universe.physicsTimeStep);
        }

         } else {
             // implement fourth order Runge-Kutta method
              for (int i = 0; i < bodies.Length; i++) {
                    Vector3 currentPosition = bodies[i].Position;
                    Vector3 currentVelocity = bodies[i].velocity;
                    Vector3 k1v = CalculateAcceleration(currentPosition, bodies[i]) * Universe.physicsTimeStep;
                    Vector3 k1p = currentVelocity * Universe.physicsTimeStep;

                    Vector3 k2v = CalculateAcceleration(currentPosition + k1p * 0.5f, bodies[i]) * Universe.physicsTimeStep;
                    Vector3 k2p = (currentVelocity + k1v * 0.5f) * Universe.physicsTimeStep;

                    Vector3 k3v = CalculateAcceleration(currentPosition + k2p * 0.5f, bodies[i]) * Universe.physicsTimeStep;
                    Vector3 k3p = (currentVelocity + k2v * 0.5f) * Universe.physicsTimeStep;

                    Vector3 k4v = CalculateAcceleration(currentPosition + k3p, bodies[i]) * Universe.physicsTimeStep;
                    Vector3 k4p = (currentVelocity + k3v) * Universe.physicsTimeStep;

                    // Update velocity and position
                    Vector3 newVelocity = currentVelocity + (k1v + 2 * k2v + 2 * k3v + k4v) / 6f;
                    Vector3 newPosition = currentPosition + (k1p + 2 * k2p + 2 * k3p + k4p) / 6f;

                    bodies[i].velocity = newVelocity;
                    bodies[i].Rigidbody.MovePosition(newPosition);

         }

      }
    }

    }
    
    public static Vector3 CalculateAcceleration (Vector3 point, CelestialBody ignoreBody = null) {
        Vector3 acceleration = Vector3.zero;
        foreach (var body in Instance.bodies) {
            if (body != ignoreBody) {
                float sqrDst = (body.Position - point).sqrMagnitude;
                Vector3 forceDir = (body.Position - point).normalized;
                acceleration += forceDir * Universe.gravitationalConstant * body.mass / sqrDst;
            }
        }

        return acceleration;
    }

    public static CelestialBody[] Bodies {
        get {
            return Instance.bodies;
        }
    }

    static NBodySimulation Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<NBodySimulation> ();
            }
            return instance;
        }
    }
}