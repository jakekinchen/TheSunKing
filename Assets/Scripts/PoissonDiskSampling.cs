using System.Collections.Generic;
using UnityEngine;


public static class PoissonDiskSampling
{
    public static List<Vector3> GeneratePoints(float radius, Vector2 sampleRegionSize, float minHeight, float maxHeight, int numSamplesBeforeRejection = 30)
    {
        float cellSize = radius / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
        List<Vector3> points = new List<Vector3>();
        List<Vector2> spawnPoints = new List<Vector2>();

        spawnPoints.Add(sampleRegionSize / 2);
        while (spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCentre = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector3 candidate = new Vector3(spawnCentre.x + dir.x * Random.Range(radius, 2 * radius), Random.Range(minHeight, maxHeight), spawnCentre.y + dir.y * Random.Range(radius, 2 * radius));
                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(new Vector2(candidate.x, candidate.z));
                    grid[(int)(candidate.x / cellSize), (int)(candidate.z / cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }
            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        return points;
    }

    static bool IsValid(Vector3 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector3> points, int[,] grid)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.z >= 0 && candidate.z < sampleRegionSize.y)
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.z / cellSize);
            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
        {
        int pointIndex = grid[x, y] - 1;
        if (pointIndex != -1)
        {
        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
        if (sqrDst < radius * radius)
        {
        return false;
        }
        }
        }
        }
        return true;
        }
        return false;
    }
}