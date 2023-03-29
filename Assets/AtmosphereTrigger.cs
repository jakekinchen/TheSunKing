using UnityEngine;

public class AtmosphereTrigger : MonoBehaviour {
    public bool isPlayerInside { get; private set; }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            isPlayerInside = false;
        }
    }
}
