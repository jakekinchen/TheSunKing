using UnityEngine;
using UnityEditor;

public class RuntimePositionMarkerEditor : EditorWindow
{
    GameObject objectToSpawn;

    [MenuItem("Tools/Runtime Position Marker")]
    public static void ShowWindow()
    {
        GetWindow<RuntimePositionMarkerEditor>("Runtime Position Marker");
    }

    private void OnGUI()
    {
        GUILayout.Label("Object to Spawn", EditorStyles.boldLabel);
        objectToSpawn = (GameObject)EditorGUILayout.ObjectField(objectToSpawn, typeof(GameObject), false);

        GUILayout.Space(10);
//make the button q also save the position
        if (GUILayout.Button("Save Current Position") || Event.current.keyCode == KeyCode.Q)
        {
            if (Application.isPlaying)
            {
                RuntimePositionMarker marker = FindObjectOfType<RuntimePositionMarker>();
                if (marker != null && marker.planet != null)
                {
                    marker.SaveCurrentPosition();
                    Debug.Log("Position and rotation saved");
                }
                else
                {
                    Debug.LogError("RuntimePositionMarker component or planet object not found.");
                }
            }
            else
            {
                Debug.LogError("Save position is only available during runtime.");
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Place Object at Saved Position"))
        {
            if (!Application.isPlaying)
            {
                
                SpawnObjectAtSavedPosition();
            }
            else
            {
                Debug.LogError("Placing object is only available outside runtime.");
            }
        }
    }

   void SpawnObjectAtSavedPosition()
{
    RuntimePositionMarker marker = FindObjectOfType<RuntimePositionMarker>();

    if (marker != null && marker.planet != null && objectToSpawn != null)
    {
        // Instantiate the object at the saved world position and rotation
        GameObject spawnedObject = Instantiate(objectToSpawn, RuntimePositionMarker.savedWorldPosition, RuntimePositionMarker.savedWorldRotation);

        // Parent the object to the planet
        spawnedObject.transform.SetParent(marker.planet.transform);

        // Calculate the local position and rotation of the spawned object
        Vector3 localPosition = marker.planet.transform.rotation * Quaternion.Inverse(marker.planet.transform.rotation) * (spawnedObject.transform.position - marker.planet.transform.position);
        Quaternion localRotation = Quaternion.Inverse(marker.planet.transform.rotation) * spawnedObject.transform.rotation;

        // Set the local position and rotation of the spawned object
        spawnedObject.transform.localPosition = localPosition;
        spawnedObject.transform.localRotation = localRotation;
    }
    else
    {
        Debug.LogError("RuntimePositionMarker component, planet object, or object to spawn not found.");
    }
}



}
