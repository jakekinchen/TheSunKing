using UnityEngine;
using UnityEngine.UI;

public class EnergyDisplay : MonoBehaviour
{
    public PlayerController playerController;
    public Slider energySlider;

    private void Update()
    {
        UpdateEnergyDisplay();
    }

    private void UpdateEnergyDisplay()
    {
        float currentEnergy = playerController.energy;
        float maxEnergy = playerController.maxEnergy;

        energySlider.value = currentEnergy / maxEnergy;
    }
}
