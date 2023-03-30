using UnityEngine;

public class AtmosphereTrigger : MonoBehaviour {
    public bool isPlayerInside { get; private set; }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerInside = true;
            Debug.Log("Player entered atmosphere");
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerInside = false;
            Debug.Log("Player exited atmosphere");
        }
    }
}
