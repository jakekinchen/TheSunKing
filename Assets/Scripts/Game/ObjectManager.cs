using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;
    public GameObject terrainObject;
    public GameObject planet;
    public int terrainLayer;

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
        if (Physics.Raycast(obj.transform.position, direction, out hit, Mathf.Infinity, 1 << terrainLayer))
        {
            obj.transform.position = hit.point;
        }
        else
        {
            Debug.DrawLine(obj.transform.position, obj.transform.position + direction * 1000, Color.red, 10);
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
        if (Physics.Raycast(obj.transform.position, direction, out hit, Mathf.Infinity, 1 << terrainLayer))
        {
            obj.transform.up = hit.normal;
        }
        else
        {
            Debug.DrawLine(obj.transform.position, obj.transform.position + direction * 1000, Color.red, 10);
            Debug.LogError("AlignObjectToSurface: Raycast did not hit.");
        }
    }
}
