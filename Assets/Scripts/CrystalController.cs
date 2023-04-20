using UnityEngine;

public class CrystalController : MonoBehaviour
{
    public float levitateHeight = 0.5f;
    public float levitateSpeed = 0.5f;
    public float rotationSpeed = 10f;

    public CelestialBody celestialBody;
    public Rigidbody rb;

    private void Start()
{
    celestialBody = GameObject.FindGameObjectWithTag("Humble Abode").GetComponent<CelestialBody>();
    rb = GetComponent<Rigidbody>();
    rb.useGravity = false;
    rb.isKinematic = false;
}

    private void FixedUpdate()
    {
        ApplyGravity();
        Levitate();
        Rotate();
    }

    private void ApplyGravity()
    {
        Vector3 forceDir = (celestialBody.Position - rb.position).normalized;
        float sqrDst = (celestialBody.Position - rb.position).sqrMagnitude;
        Vector3 acceleration = forceDir * Universe.gravitationalConstant * celestialBody.mass / sqrDst;
        rb.AddForce(acceleration, ForceMode.Acceleration);
    }

    private void Levitate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            float targetHeight = hit.point.y + levitateHeight;
            float difference = targetHeight - transform.position.y;
            rb.velocity = new Vector3(rb.velocity.x, difference * levitateSpeed, rb.velocity.z);
        }
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}
