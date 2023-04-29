using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereTrigger : MonoBehaviour
{
    public CelestialBody celestialBody;
    private bool isPlayerInside;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPlayerInside)
        {
            isPlayerInside = true;
            celestialBody.PlayerEnteredAtmosphere();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPlayerInside)
        {
            isPlayerInside = false;
            celestialBody.PlayerExitedAtmosphere();
        }
    }
}
