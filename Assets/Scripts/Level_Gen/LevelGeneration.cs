using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] NoiseMapGeneration noiseMapGeneration;
    [SerializeField] private int mapWidthInTiles, mapDepthInTiles;

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject waterPrefabStraight;
    [SerializeField] private GameObject waterPrefabStraight2;
    [SerializeField] private GameObject waterPrefabStraight3;

    [SerializeField] private GameObject waterPrefabRight;
    [SerializeField] private GameObject waterPrefabRight2;
    [SerializeField] private GameObject waterPrefabRight3;
    [SerializeField] private GameObject waterPrefabEnd;
    [SerializeField] private GameObject waterPrefabEnd2;
    [SerializeField] private GameObject moundPrefabEnd3;
    [SerializeField]
    private Wave[] waves;
    
    private GameObject[] waterPrefabs = new GameObject[8];


    [SerializeField] private GameObject burgPrefab;
    private TileType[,] tiles;

    void Start()
    {
        waterPrefabs[0] = waterPrefabStraight;
        waterPrefabs[1] = waterPrefabStraight2;
        waterPrefabs[2] = waterPrefabStraight3;
        waterPrefabs[3] = waterPrefabEnd;
        waterPrefabs[4] = waterPrefabEnd2;
        waterPrefabs[5] = waterPrefabRight;
        waterPrefabs[6] = waterPrefabRight2;
        waterPrefabs[7] = waterPrefabRight3;
        GenerateMap();
    }

    void GenerateMap()
    {
        tiles = new TileType[mapWidthInTiles, mapDepthInTiles];
        // get the tile dimensions from the tile Prefab
        Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        int tileWidth = (int)tileSize.x;
        int tileDepth = (int)tileSize.z;
        // for each Tile, instantiate a Tile in the correct position
        float offsetX = -this.gameObject.transform.position.x;
        float offsetZ = -this.gameObject.transform.position.z;
        float[,] noiseMap = this.noiseMapGeneration.GenerateNoiseMap(mapDepthInTiles, mapWidthInTiles, 1.0f, offsetX, offsetZ, waves);
        for (int xTileIndex = 0; xTileIndex < mapWidthInTiles; xTileIndex++)
        {
            for (int zTileIndex = 0; zTileIndex < mapDepthInTiles; zTileIndex++)
            {
                // calculate the tile position based on the X and Z indices
                Vector3 tilePosition = new Vector3(this.gameObject.transform.position.x + xTileIndex * tileWidth,
                    this.gameObject.transform.position.y,
                    this.gameObject.transform.position.z + zTileIndex * tileDepth);
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                TileType tileType = new TileType();
                tileType.type = "plane";
                tileType.orientation = "normal";
                tileType.tile = tile;
                tiles[xTileIndex, zTileIndex] = tileType;

                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                tile.transform.Rotate(0f, 90.0f, 0.0f, Space.Self);
                //BuildRiver(xTileIndex, zTileIndex, tilePosition);
                //Build the wall
                /*if (zTileIndex == 0 || zTileIndex == mapDepthInTiles - 1 || xTileIndex == 0 ||
                    xTileIndex == mapWidthInTiles - 1)
                {
                    BuildSurroundingWall(xTileIndex, zTileIndex, tileWidth, tileDepth);
                }*/
            }
        }
    }

    private void BuildSurroundingWall(int xTileIndex, int zTileIndex, int tileWidth, int tileDepth)
    {
        Vector3 blockPosition = new Vector3(this.gameObject.transform.position.x + xTileIndex * tileWidth,
            this.gameObject.transform.position.y,
            this.gameObject.transform.position.z + zTileIndex * tileDepth);
        GameObject burg = Instantiate(burgPrefab, blockPosition, Quaternion.identity) as GameObject;
    }

    private void BuildRiver(int xTileIndex, int zTileIndex, Vector3 tilePosition)
    {
        if (xTileIndex == 0)
                {
                    int value = (int)Math.Ceiling(Random.Range(-0.1f, 100f));
                    if (value <= 4)
                    {
                        GameObject tile =
                            Instantiate(waterPrefabs[value], tilePosition, Quaternion.identity) as GameObject;
                        tile.transform.Rotate(0f, 90.0f, 0.0f, Space.Self);
                        TileType tileType = new TileType();
                        tileType.type = "water";
                        tileType.orientation = "normal";
                        tileType.tile = tile;
                        tiles[xTileIndex, zTileIndex] = tileType;
                    }
                    else
                    {
                        GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                        tile.transform.Rotate(0f, 90.0f, 0.0f, Space.Self);
                        TileType tileType = new TileType();
                        tileType.type = "plane";
                        tileType.orientation = "normal";
                        tileType.tile = tile;
                        tiles[xTileIndex, zTileIndex] = tileType;
                    }
                }
                else if (tiles[xTileIndex - 1, zTileIndex].type.Equals("water"))
                {
                    int value = (int)Math.Ceiling(Random.Range(-0.1f, 32f));
                    if (value == 4 || value == 5 || (value / 4) == 4 || (value / 4) == 5) value = 1;
                    if (value >= 8)
                    {
                        GameObject tile =
                            Instantiate(waterPrefabs[(value / 4) - 1], tilePosition, Quaternion.identity) as GameObject;
                        tile.transform.Rotate(0f, 90.0f, 0.0f, Space.Self);
                        TileType tileType = new TileType();
                        tileType.type = "water";
                        tileType.orientation = "normal";
                        tileType.tile = tile;
                        tiles[xTileIndex, zTileIndex] = tileType;
                    }
                    else
                    {
                        GameObject tile =
                            Instantiate(waterPrefabs[value], tilePosition, Quaternion.identity) as GameObject;
                        tile.transform.Rotate(0f, 90.0f, 0.0f, Space.Self);
                        TileType tileType = new TileType();
                        tileType.type = "water";
                        tileType.orientation = "normal";
                        tileType.tile = tile;
                        tiles[xTileIndex, zTileIndex] = tileType;
                    }
                }
                else if (tiles[xTileIndex - 1, zTileIndex].type.Equals("water"))
                {
                    int value = (int)Math.Ceiling(Random.Range(-0.1f, 8f));
                    if (value <= 7)
                    {
                        GameObject tile =
                            Instantiate(waterPrefabs[value], tilePosition, Quaternion.identity) as GameObject;
                        TileType tileType = new TileType();
                        tileType.type = "water";
                        tileType.orientation = "normal";
                        tileType.tile = tile;
                        tiles[xTileIndex, zTileIndex] = tileType;
                    }
                    else
                    {
                        GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                        TileType tileType = new TileType();
                        tileType.type = "plane";
                        tileType.orientation = "normal";
                        tileType.tile = tile;
                        tiles[xTileIndex, zTileIndex] = tileType;
                    }
                }
                else
                {
                    // instantiate a new Tile
                    GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                    TileType tileType = new TileType();
                    tileType.type = "plane";
                    tileType.orientation = "normal";
                    tileType.tile = tile;
                    tiles[xTileIndex, zTileIndex] = tileType;
                }
    }
}

public class TileType
{
    public string type;
    public string orientation;
    public GameObject tile;
}