using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))] //require a mesh filter in the same object of this script so a null mesh isn't used
public class MeshGeneration : MonoBehaviour
{

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        //Generate total vertices
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        //Generate grid of vertices
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize ; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.1f, z * 0.1f) * 2f; //generate varied heights with perlin noise algorythm
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        //Generate total triangles
        triangles = new int[xSize * zSize * 6];
        int tris = 0;
        int vert = 0;

        //Generate grid of triangles  
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                /*
                 * Add tris to each array element to shift to the next set of triangles per loop
                 * Add vert to each equation to shift to the next set of vertices
                 */

                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++; //so script skips generating triangles from the end of one row to the beginning of another
        }

    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals(); //fix lighting
    }

}