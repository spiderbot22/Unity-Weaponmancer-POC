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

    public bool autoUpdate;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale, octaves, lacunarity, persistance);

        MapDisplay display = FindObjectOfType<MapDisplay>(); //ref to MapDisplay.cs
        display.DrawNoiseMap(noiseMap); //create texture plane with a noise map
    }

}
