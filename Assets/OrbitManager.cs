using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Planet
{
    public class OrbitManager : MonoBehaviour
    {
        public static OrbitManager Instance;

        private List<GameObject> _objects;

        public GameObject rootObject;

        public Vector3 startPos;
        public Vector3 basePos;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            _objects = new List<GameObject>();
            
            CollectObjects(rootObject); // Gathers all linked orbiting objects

            RecalculatePositions(rootObject, Vector2.positiveInfinity); // Figures out where they should be

            InvokeRepeating(nameof(ProcessMovement), 0, 1f); //TODO change to modify the speed
        }

        private void CollectObjects(GameObject obj)
        {
            _objects.Add(obj);

            if (obj.GetComponent<MovementObj>().satellites.Length == 0) return;
            
            foreach (var satellite in obj.GetComponent<MovementObj>().satellites)
            {
                CollectObjects(satellite);
                Debug.unityLogger.LogException(new UnityException("Added Object"));
            }
        }

        private void ProcessMovement()
        {
            foreach (var satellite in _objects)
            {
                Debug.unityLogger.Log("Processing Movement");
                var mObj = satellite.GetComponent<MovementObj>();
                
                mObj.currentRotDegree = ((0.01f / mObj.orbitalRadius) * 360f) + mObj.currentRotDegree;

                if (mObj.currentRotDegree >= 360f)
                {
                    mObj.currentRotDegree -= 360f;
                }
            }

            RecalculatePositions(rootObject, Vector2.positiveInfinity);
        }

        private void RecalculatePositions(GameObject obj, Vector3 referencePos)
        {
            MovementObj mObj = obj.GetComponent<MovementObj>();
            if (referencePos == Vector3.positiveInfinity ||
                mObj.orbitalRadius == 0) // must be neg infinity if specifying the root node
            {
                obj.transform.position = basePos + startPos; // Set pos to origin
                foreach (var satellite in mObj.satellites)
                {
                    RecalculatePositions(satellite, basePos + startPos); // Recalculate based on 
                }
            }
            else
            {
                // For a circle with origin (j, k), radius r, degree t:
                // x(t) = r cos(t) + j
                // y(t) = r sin(t) + k

                var radius = mObj.orbitalRadius;
                var degree = mObj.currentRotDegree * Math.PI / 180; // Expects Radians

                var x = (float) (radius * Math.Cos(degree) + referencePos.x);
                var y = referencePos.y;
                var z = (float) (radius * Math.Sin(degree) + referencePos.z);
                //Debug.unityLogger.Log("x: " + x + " z: " + y);

                obj.transform.position = new Vector3(x, y, z);

                foreach (var satellite in mObj.satellites)
                {
                    RecalculatePositions(satellite, new Vector2(x, y)); // Recalculate based on 
                }
            }
        }
    }
}