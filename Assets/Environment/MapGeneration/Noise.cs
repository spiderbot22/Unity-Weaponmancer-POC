using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public enum NormalizeMode {Local, Global}; //local min/max, estimate global min/max (so chunks are seamless)

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float noiseScale, int octaves, float lacunarity, float persistance, Vector2 offset, NormalizeMode normalizeMode)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed); //psuedo random number generator
        Vector2[] octaveOffsets = new Vector2[octaves]; //so each octave is seeded differently

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < octaves; i++)
        {
            float xOffset = prng.Next(-100000, 100000) + offset.x; //generate random number in -100k->100k range to prevent perlin function from tripping out
            float yOffset = prng.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(xOffset, yOffset);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        //prevent division by 0
        if (noiseScale <= 0)
        {
            noiseScale = 0.0001f;
        }

        float maxLocalNoiseHeight = float.MinValue;
        float minLocalNoiseHeight = float.MaxValue;

        //find center of screen for noise scale
        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {

                    //higher the frequency, the more spread out the sample points and height values will change more rapidly
                    float sampleX = (x - halfWidth + octaveOffsets[i].x) / noiseScale * frequency;
                    float sampleY = (y - halfHeight + octaveOffsets[i].y) / noiseScale * frequency;

                    float perlinVal = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // "* 2 - 1" changes range from [0,1] to [-1,1]
                    noiseHeight += perlinVal * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                //keep track of min and max height values for normalization
                if (noiseHeight > maxLocalNoiseHeight)
                {
                    maxLocalNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minLocalNoiseHeight)
                {
                    minLocalNoiseHeight = noiseHeight;
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
                 * Ex: height = minNoiseHeight, return 0
                 *     height = maxNoiseHeight, return 1
                 *     height = (minNoiseHeight + maxNoiseHeight)/2, return 0.5
                 */
                if (normalizeMode == NormalizeMode.Local)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                }
                else
                {
                    float normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight * 2f);
                    noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                }
                
            }

        }

    return noiseMap;

    }

}
