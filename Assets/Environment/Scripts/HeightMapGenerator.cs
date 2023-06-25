using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HeightMapGenerator
{
    public static HeightMap GenerateHeightMap(int width, int height, HeightMapSettings settings)
    {

    }
}

public struct HeightMap
{
    public readonly float[,] values;
    public readonly float minValue;
    public readonly float maxValue;

    public HeightMap(float[,] heightMap, float minValue, float maxValue)
    {
        this.values = heightMap;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

}
