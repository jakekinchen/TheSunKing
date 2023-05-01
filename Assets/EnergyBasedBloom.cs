using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EnergyBasedBloom : MonoBehaviour
{
    public PostProcessProfile postProcessingProfile;
    public PlayerController playerController;
    public float maxEnergy = 100f;
    public float minBloomIntensity = 0f;
    public float maxBloomIntensity = 3f;
    private float currentEnergy;

    private Bloom bloom;

    private void Start()
    {
        //playerController = GetComponent<PlayerController>();
        bloom = postProcessingProfile.GetSetting<Bloom>();
    }

    private void Update()
    {
        currentEnergy = playerController.energy;
        UpdateBloomIntensity();
    }

    private void UpdateBloomIntensity()
    {
        float energyPercentage = currentEnergy / maxEnergy;
        float bloomIntensity = Mathf.Lerp(minBloomIntensity, maxBloomIntensity, energyPercentage);
        bloom.intensity.value = bloomIntensity;
    }
}
