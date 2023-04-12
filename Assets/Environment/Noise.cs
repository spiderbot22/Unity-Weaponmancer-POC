using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float noiseScale)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        //prevent division by 0
        if (noiseScale <= 0)
        {
            noiseScale = 0.0001f;
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float sampleX = x / noiseScale;
                float sampleY = y / noiseScale;

                float perlinVal = Mathf.PerlinNoise(sampleX, sampleY);
                noiseMap[x, y] = perlinVal;
            }
        }

        return noiseMap;

    }

}
