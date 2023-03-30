using UnityEngine;

public class RuntimePositionMarker : MonoBehaviour
{
    public GameObject planet;
    public static Vector3 savedWorldPosition;
    public static Quaternion savedWorldRotation;

    public void SaveCurrentPosition()
    {
        savedWorldPosition = transform.position;
        savedWorldRotation = transform.rotation;
    }
}
