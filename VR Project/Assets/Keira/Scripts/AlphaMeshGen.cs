using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AlphaMeshGen : MonoBehaviour
{

    [Header("General")]
    [Tooltip("Map Size/Tile Count (Max 128 currently). Needs to match .png texture")]
    [Range(1, 128)]
    public int mapSize = 64;

    [Tooltip("Height multiplier for the mesh")]
    [Range(1, 100)]
    public int mapHeight = 10;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MeshCreation()
    {

    }

    float[,] ReadPNG()
    {
        float[,] heightMap = new float[mapSize, mapSize];

        byte[] bytes;

        bytes = File.ReadAllBytes(Application.dataPath + "/../Map.png");   


        return heightMap;
    }

}
