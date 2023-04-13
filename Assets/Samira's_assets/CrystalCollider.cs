using UnityEngine;
using UnityEngine.Events;

public class CrystalCollider : MonoBehaviour
{
    [System.Serializable]
    public class CrystalCollisionEvent : UnityEvent<bool> { }

    public CrystalCollisionEvent onCrystalCollision;

    private bool hasCrystal = false;

    private void Start()
    {
        if (onCrystalCollision == null)
        {
            onCrystalCollision = new CrystalCollisionEvent();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasCrystal)
        {
            Debug.Log("Crystal collision");
            hasCrystal = true;
            gameObject.SetActive(false);
            onCrystalCollision.Invoke(hasCrystal);
        }
        
    }
}
