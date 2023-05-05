using UnityEngine;

public class CheckMeshCollider : MonoBehaviour
{
    void Start()
    {
        // Check if there's a MeshCollider component attached to the GameObject
        if (GetComponent<MeshCollider>() != null)
        {
            Debug.Log("MeshCollider component found on the GameObject.");
        }
        else
        {
            Debug.Log("MeshCollider component NOT found on the GameObject.");
        }
    }
}