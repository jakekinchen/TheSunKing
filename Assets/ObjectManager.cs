using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;
    public LayerMask planetLayer;
    public GameObject planet;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlanet(GameObject newPlanet)
{
    planet = newPlanet;
}


    public void StickObjectToSurface(GameObject obj)
{
    if (obj == null)
    {
        Debug.LogError("StickObjectToSurface: obj is null.");
        return;
    }

    Vector3 direction = (planet.transform.position - obj.transform.position).normalized;
    RaycastHit hit;
    if (Physics.Raycast(obj.transform.position, direction, out hit, Mathf.Infinity, planetLayer))
    {
        obj.transform.position = hit.point;
    }
    else
    {
        Debug.LogError("StickObjectToSurface: Raycast did not hit.");
    }
}

public void AlignObjectToSurface(GameObject obj)
{
    if (obj == null)
    {
        Debug.LogError("AlignObjectToSurface: obj is null.");
        return;
    }

    Vector3 direction = (planet.transform.position - obj.transform.position).normalized;
    RaycastHit hit;
    if (Physics.Raycast(obj.transform.position, direction, out hit, Mathf.Infinity, planetLayer))
    {
        obj.transform.up = hit.normal;
    }
    else
    {
        Debug.LogError("AlignObjectToSurface: Raycast did not hit.");
    }
}

}
