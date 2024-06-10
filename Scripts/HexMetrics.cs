using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    public static float outerRadius = 4f;
    public static float innerRadius = outerRadius * 0.866025404f;
    public static float width = 2f * outerRadius;
    public static float height = Mathf.Sqrt(3) * outerRadius;
    public const int chunkSizeX = 3, chunkSizeY = 2;



    public static Vector3[] corners = {
        new Vector3(0f, outerRadius, 0f),
        new Vector3(innerRadius,  0.5f * outerRadius, 0f),
        new Vector3(innerRadius, -0.5f * outerRadius, 0f),
        new Vector3(0f, -outerRadius, 0f),
        new Vector3(-innerRadius, -0.5f * outerRadius, 0f),
        new Vector3(-innerRadius, 0.5f * outerRadius, 0f),
    };

    public static Vector3 GetHexagonCenter()
    {
        Vector3 center = Vector3.zero;

        foreach (Vector3 corner in corners)
        {
            center += corner;
        }

        center /= corners.Length;

        return center;
    }
}
