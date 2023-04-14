using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    public float lacunarity;
    public float persistance;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, lacunarity, persistance, offset);

        MapDisplay display = FindObjectOfType<MapDisplay>(); //ref to MapDisplay.cs
        display.DrawNoiseMap(noiseMap); //create texture plane with a noise map
    }

}
