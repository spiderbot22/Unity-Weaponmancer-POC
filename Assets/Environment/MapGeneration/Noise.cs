using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float noiseScale, int octaves, float lacunarity, float persistance)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        //prevent division by 0
        if (noiseScale <= 0)
        {
            noiseScale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = x * frequency / noiseScale;
                    float sampleY = y * frequency / noiseScale;

                    float perlinVal = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // "* 2 - 1" changes range from [0,1] to [-1,1]
                    noiseHeight += perlinVal * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                //keep track of min and max height values for normalization
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;

            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                /*
                 * Normalizes map by returning the relative position of the current height between 0 and 1.
                 * Ex: height = minNoiseHeight return 0
                 *     height = maxNoiseHeight return 1
                 *     height = (minNoiseHeight + maxNoiseHeight)/2 return 0.5
                 */
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }

        }

    return noiseMap;

    }

}
