using UnityEngine;

public class MoonlightController : MonoBehaviour
{
    public GameObject Earth;
    public GameObject Sun;

    private Transform _earthTransform;
    private Transform _sunTransform;
    private Transform _moonlightTransform;

    // Start is called before the first frame update
    void Start()
    {
        _earthTransform = Earth.transform;
        _sunTransform = Sun.transform;
        _moonlightTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoonlightPosition();
        UpdateMoonlightDirection();
    }

    private void UpdateMoonlightPosition()
    {
        Vector3 earthToSunDirection = _earthTransform.position - _sunTransform.position;
        Vector3 moonlightPosition = _earthTransform.position + earthToSunDirection.normalized * 20; // 20 is the distance from Earth to the moonlight
        _moonlightTransform.position = moonlightPosition;
    }

    private void UpdateMoonlightDirection()
    {
        Vector3 moonlightToEarthDirection = _earthTransform.position - _moonlightTransform.position;
        _moonlightTransform.forward = moonlightToEarthDirection.normalized;
    }
}
