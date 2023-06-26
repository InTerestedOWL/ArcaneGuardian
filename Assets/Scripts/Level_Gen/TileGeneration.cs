using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class TileGeneration : MonoBehaviour
{
    [SerializeField] NoiseMapGeneration noiseMapGeneration;

    [SerializeField] private MeshRenderer tileRenderer;

    [SerializeField] private MeshFilter meshFilter;

    [SerializeField] private MeshCollider meshCollider;

    [SerializeField] private float mapScale;

    [SerializeField] private TerrainType[] terrainTypes;
    
    [SerializeField] private BuildingType[] buildingTypes;
    [SerializeField] private float maxRandom;

    [SerializeField] private float heightMultiplier;
   
    
    [SerializeField]
    private AnimationCurve heightCurve;

    [SerializeField]
    private Wave[] waves;
    void Start()
    {
        GenerateTile();
    }

    void GenerateTile() {
        // calculate tile depth and width based on the mesh vertices
        Vector3[] meshVertices = this.meshFilter.mesh.vertices;
        int tileDepth = (int)Mathf.Sqrt (meshVertices.Length);
        int tileWidth = tileDepth;
        // calculate the offsets based on the tile position
        float offsetX = -this.gameObject.transform.position.x;
        float offsetZ = -this.gameObject.transform.position.z;
        // generate a heightMap using noise
        float[,] heightMap = this.noiseMapGeneration.GenerateNoiseMap (tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, waves);
        // build a Texture2D from the height map
       // Texture2D tileTexture = BuildTexture (heightMap);
        BuildTerrain(heightMap);
        //this.tileRenderer.material.mainTexture = tileTexture;
        // update the tile mesh vertices according to the height map
        //UpdateMeshVertices (heightMap);
    }
    
    private void BuildTerrain(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);
        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                // transform the 2D map index is an Array index
                float height = heightMap[zIndex, xIndex];
                // choose a terrain type according to the height value
                BuildingType buildingType = ChooseBuildingType(height);
                if (buildingType != null)
                {
                    
                    Vector3 blockPosition = new Vector3(this.gameObject.transform.position.x + xIndex,
                        this.gameObject.transform.position.y + 0.3f,
                        this.gameObject.transform.position.z + zIndex);
                    Vector3 halfExtents =  new Vector3(5f,
                        0,
                         5f);
                    if (!Physics.CheckBox(blockPosition, halfExtents))
                    {
                        GameObject building = Instantiate(buildingType.asset, blockPosition, Quaternion.identity);
                        /*NavMeshObstacle obstRef = building.AddComponent<NavMeshObstacle>();
                        obstRef.carving = true;*/
                        building.transform.localScale = new Vector3(buildingType.scale, buildingType.scale, buildingType.scale);
                    }
                }
            }
        }
        LevelGeneration.BuildNavMesh();
    }

    private Texture2D BuildTexture(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);
        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                // transform the 2D map index is an Array index
                int colorIndex = zIndex * tileWidth + xIndex;
                float height = heightMap[zIndex, xIndex];
                // choose a terrain type according to the height value
                TerrainType terrainType = ChooseTerrainType(height);
                // assign the color according to the terrain type
                colorMap[colorIndex] = terrainType.color;
            }
        }

        // create a new texture and set its pixel colors
        Texture2D tileTexture = new Texture2D(tileWidth, tileDepth);
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels(colorMap);
        tileTexture.Apply();
        return tileTexture;
    }

    TerrainType ChooseTerrainType(float height)
    {
        // for each terrain type, check if the height is lower than the one for the terrain type
        foreach (TerrainType terrainType in terrainTypes)
        {
            // return the first terrain type whose height is higher than the generated one
            if ( height < terrainType.height)
            {
                
                return terrainType;
            }
        }

        return terrainTypes[terrainTypes.Length -1];
    }

    BuildingType ChooseBuildingType(float height)
    {
        foreach (BuildingType buildingType in buildingTypes)
        {
            double value = Math.Ceiling(Random.Range(-0.1f, maxRandom));
            // return the first terrain type whose height is higher than the generated one
            if ( height > buildingType.minHeight && height <= buildingType.maxHeight && buildingType.probability == value)
            {
                return buildingType;
            }
        }

        return null;
    }
    
    private void UpdateMeshVertices(float[,] heightMap) {
        int tileDepth = heightMap.GetLength (0);
        int tileWidth = heightMap.GetLength (1);
        Vector3[] meshVertices = this.meshFilter.mesh.vertices;
        // iterate through all the heightMap coordinates, updating the vertex index
        int vertexIndex = 0;
        for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
                float height = heightMap [zIndex, xIndex];
                Vector3 vertex = meshVertices [vertexIndex];
                // change the vertex Y coordinate, proportional to the height value. The height value is evaluated by the heightCurve function, in order to correct it.
                meshVertices[vertexIndex] = new Vector3(vertex.x, this.heightCurve.Evaluate(height) * this.heightMultiplier, vertex.z);
                vertexIndex++;
            }
        }
        // update the vertices in the mesh and update its properties
        this.meshFilter.mesh.vertices = meshVertices;
        this.meshFilter.mesh.RecalculateBounds ();
        this.meshFilter.mesh.RecalculateNormals ();
        // update the mesh collider
        this.meshCollider.sharedMesh = this.meshFilter.mesh;
    }
}

[System.Serializable]
public class TerrainType
{
    public string name;
    public float height;
    public Color color;
}

[System.Serializable]
public class BuildingType
{
    public string name;
    public float minHeight;
    public float maxHeight;
    public GameObject asset;
    public double probability;
    public float scale;
}