using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{

    public Renderer textureRender;

    public void DrawNoiseMap(float[,] noiseMap)
    {
        //get width and height of given noise map
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colorMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //give each vert a color ranging from black to white
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        //apply color map to noise map
        texture.SetPixels(colorMap);
        texture.Apply();

        //generate texture
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(width, 1, height); //set size of plane to size of map

    }
       
}
