using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PoissonDiskSampling : MonoBehaviour
{
    public float radius = 1;
    public Vector3 sampleRegionSize = new Vector3(10, 5, 7.5f);
    public int numSamplesBeforeRejection = 30;

    public bool is2D = true;

    public float cellSize;
    private Vector3[,,] grid;

public void Initialize(MeshCollider terrainMeshCollider)
{
    float minHeight = 0;
    float maxHeight = 1;

    Bounds terrainBounds = terrainMeshCollider.sharedMesh.bounds;
    Vector3 terrainCenter = terrainMeshCollider.transform.TransformPoint(terrainBounds.center);

    (List<Vector3> points, Vector3 _) = GeneratePoints(radius, sampleRegionSize, minHeight, maxHeight, numSamplesBeforeRejection, terrainCenter, terrainMeshCollider);
}


private float GetTerrainHeight(Vector3 point, MeshCollider terrainMeshCollider)
{
    Mesh terrainMesh = terrainMeshCollider.sharedMesh;
    Vector3[] vertices = terrainMesh.vertices;
    int[] triangles = terrainMesh.triangles;

    for (int i = 0; i < triangles.Length; i += 3)
    {
        Vector3 a = terrainMeshCollider.transform.TransformPoint(vertices[triangles[i]]);
        Vector3 b = terrainMeshCollider.transform.TransformPoint(vertices[triangles[i + 1]]);
        Vector3 c = terrainMeshCollider.transform.TransformPoint(vertices[triangles[i + 2]]);

        Plane plane = new Plane(a, b, c);
        float distance;

        Ray ray = new Ray(new Vector3(point.x, 1000f, point.z), Vector3.down);
        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance).y;
        }
    }

    return 0f;
}
    public (List<Vector3> points, Vector3 sampleRegionSize) GeneratePoints(float radius, Vector3 sampleRegionSize, float minHeight, float maxHeight, int numSamplesBeforeRejection, Vector3 terrainCenter, MeshCollider terrainMeshCollider)
    {
        Debug.Log("Generating points with radius: " + radius + " and sampleRegionSize: " + sampleRegionSize + " and minHeight: " + minHeight + " and maxHeight: " + maxHeight + " and numSamplesBeforeRejection: " + numSamplesBeforeRejection);
        cellSize = radius / Mathf.Sqrt(is2D ? 2 : 3); 
        grid = new Vector3[Mathf.CeilToInt(sampleRegionSize.x / cellSize), is2D ? 1 : Mathf.CeilToInt(sampleRegionSize.y / cellSize), Mathf.CeilToInt(sampleRegionSize.z / cellSize)];
        List<Vector3> points = new List<Vector3>();
        List<Vector3> spawnPoints = new List<Vector3>();

        spawnPoints.Add(terrainCenter);
        while (spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector3 spawnCentre = spawnPoints[spawnIndex];
            bool accepted = false;

            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                Vector3 dir = Random.insideUnitSphere;
                Vector3 candidate = spawnCentre + dir.normalized * Random.Range(radius, 2 * radius);
                candidate.y = Mathf.Clamp(candidate.y, minHeight, maxHeight);
                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid, is2D))
                {
                    float terrainHeight = GetTerrainHeight(candidate, terrainMeshCollider);
                    candidate.y += terrainHeight;

                    Vector3 localCandidate = transform.InverseTransformPoint(candidate);
                    points.Add(localCandidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize), (int)(candidate.z / cellSize)] = candidate;
                    accepted = true;
                    break;
                }
            }
            if (!accepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        return (points, sampleRegionSize);
    }

    static bool IsValid(Vector3 candidate, Vector3 sampleRegionSize, float cellSize, float radius, List<Vector3> points, Vector3[,,] grid, bool is2D)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y && candidate.z >= 0 && candidate.z < sampleRegionSize.z)
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);
            int cellZ = (int)(candidate.z / cellSize);
            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);
            int searchStartZ = Mathf.Max(0, cellZ - 2);
            int searchEndZ = Mathf.Min(cellZ + 2, grid.GetLength(2) - 1);
            for (int z = searchStartZ; z <= searchEndZ; z++)
            {
                for (int y = is2D ? 0 : searchStartY; y <= (is2D ? 0 : searchEndY); y++)
                {
                    for (int x = searchStartX; x <= searchEndX; x++)
                    {
                        Vector3 point = grid[x, y, z];
                        if (point != Vector3.zero)
                        {
                            float sqrDst = (candidate - point).sqrMagnitude;
                            if (sqrDst < radius * radius)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        Debug.Log("Candidate is not valid with candidate: " + candidate + " and sampleRegionSize: " + sampleRegionSize + " and cellSize: " + cellSize + " and radius: " + radius + " and points: " + points + " and grid: " + grid);
        // Draw a wire sphere around the candidate to visualize the failure
        DrawDebugSphere(candidate, radius * 2, Color.red, 5f); // Increase sphere radius
        Debug.Log("Debug sphere drawn at: " + candidate);

        return false;
    }

     private static void DrawDebugSphere(Vector3 center, float radius, Color color, float duration)
{
    float step = 10.0f;

    for (float phi = 0.0f; phi < 180.0f; phi += step)
    {
        for (float theta = 0.0f; theta < 360.0f; theta += step)
        {
            Vector3 p1 = center + GetSphericalCoordinate(radius, phi, theta);
            Vector3 p2 = center + GetSphericalCoordinate(radius, phi + step, theta);
            Vector3 p3 = center + GetSphericalCoordinate(radius, phi, theta + step);
            Vector3 p4 = center + GetSphericalCoordinate(radius, phi + step, theta + step);

           Debug.DrawLine(p1, p2, color, duration * 5); // Increase duration
            Debug.DrawLine(p1, p3, color, duration * 5); // Increase duration
            Debug.DrawLine(p2, p4, color, duration * 5); // Increase duration
            Debug.DrawLine(p3, p4, color, duration * 5); // Increase duration
            Debug.Log("Debug sphere drawn at: " + center);

        }
    }
}

private static Vector3 GetSphericalCoordinate(float radius, float phi, float theta)
{
    float phiInRadians = Mathf.Deg2Rad * phi;
    float thetaInRadians = Mathf.Deg2Rad * theta;

    float x = radius * Mathf.Sin(phiInRadians) * Mathf.Cos(thetaInRadians);
    float y = radius * Mathf.Cos(phiInRadians);
    float z = radius * Mathf.Sin(phiInRadians) * Mathf.Sin(thetaInRadians);

 return new Vector3(x, y, z);
}
}
