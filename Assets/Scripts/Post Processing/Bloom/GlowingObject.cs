using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GlowingObject : MonoBehaviour
{
    public float emissionIntensity = 1.5f;
    public bool useAsLightSource = true;
    public float lightRange = 5f;
    public float lightIntensity = 1f;

    private Renderer objectRenderer;
    private MaterialPropertyBlock propertyBlock;
    private Light emissionLight;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
        CreateOrUpdateLight();
    }

    public void UpdateGlow()
    {
        CreateOrUpdateLight();

        objectRenderer.GetPropertyBlock(propertyBlock);
        Color baseEmissionColor = objectRenderer.sharedMaterial.GetColor("_EmissionColor");
        Color finalEmissionColor = baseEmissionColor * Mathf.LinearToGammaSpace(emissionIntensity);

        propertyBlock.SetColor("_EmissionColor", finalEmissionColor);
        objectRenderer.SetPropertyBlock(propertyBlock);

        if (useAsLightSource)
        {
            emissionLight.color = baseEmissionColor;
            emissionLight.intensity = lightIntensity;
        }
    }

    private void CreateOrUpdateLight()
    {
        if (useAsLightSource && emissionLight == null)
        {
            GameObject lightObject = new GameObject("EmissionLight");
            lightObject.transform.SetParent(transform);
            lightObject.transform.localPosition = Vector3.zero;
            emissionLight = lightObject.AddComponent<Light>();
            emissionLight.range = lightRange;
            emissionLight.intensity = lightIntensity;
        }
        else if (!useAsLightSource && emissionLight != null)
        {
            DestroyImmediate(emissionLight.gameObject);
        }
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            UpdateGlow();
        }
    }
}
