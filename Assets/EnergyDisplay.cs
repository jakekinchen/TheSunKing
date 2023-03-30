using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyDisplay : MonoBehaviour
{
    public PlayerController playerController;
    public TextMeshProUGUI energyText;

    void Update()
    {
        if (playerController != null && energyText != null)
        {
            energyText.text = "Energy: " + playerController.energy.ToString("F2");
        }
    }
}
