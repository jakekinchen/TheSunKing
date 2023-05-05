using UnityEngine;

public class EnergyLightController : MonoBehaviour
{
    [SerializeField] private Light pointLight;
    private const float maxEnergy = 100f;
    private const float maxRange = 10f;

    private void Awake()
    {
        if (pointLight == null)
        {
            pointLight = GetComponent<Light>();
        }
    }

    public void UpdateEnergyLevel(float energy)
    {
        float normalizedEnergy = Mathf.Clamp(energy, 0, maxEnergy) / maxEnergy;
        pointLight.range = normalizedEnergy * maxRange;
    }
}
