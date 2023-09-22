using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AG.Control;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using UnityEngine.Rendering.UI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TerrainGenerator : MonoBehaviour
{
    private const float viewerMoveThresholdForChunkUpdate = 25f;

    private const float sqrViewerMoveThresholdForChunkUpdate =
        viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

    public int colliderLODIndex;
    public LODInfo[] detailLevels;
    public Transform viewer;

    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public ObjectDataSettings objectDataSettings;

    public GameObject player;
    public GameObject poi;

    public TextureData textureSettings;

    public Material mapMaterial;

    private float[,] fallOffMap;
    private Vector2 viewerPosition;
    private Vector2 viewerPositionOld;
    private int chunksVisibleInViewDst;
    private int numObjects;
    private int chunkCountDivided;
    private Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    private List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();
    private int loadedChunks;

    private bool hasBuildedNavMesh = false;
    private Coroutine navMeshCR = null;

    void Start()
    {
        textureSettings.ApplyToMaterial(mapMaterial);
        textureSettings.UpdateMeshHeights(mapMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);
        float maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
        chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / meshSettings.meshWorldSize);
        numObjects = Random.Range(objectDataSettings.minObjectsPerTile, objectDataSettings.maxObjectsPerTile + 1);
        fallOffMap = FallOffGenerator.GenerateFalloffMap(meshSettings.chunkCount * meshSettings.numVertsPerLine);
        chunkCountDivided = meshSettings.chunkCount / 2;

        UpdateVisibleChunks();
    }

    void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);

        if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
        {
            viewerPositionOld = viewerPosition;
            UpdateVisibleChunks();
        }
    }

    void UpdateVisibleChunks()
    {
        HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2>();
        for (int i = visibleTerrainChunks.Count - 1; i >= 0; i--)
        {
            alreadyUpdatedChunkCoords.Add(visibleTerrainChunks[i].coord);
            visibleTerrainChunks[i].UpdateTerrainChunk();
            visibleTerrainChunks[i].onMeshChanged += OnMeshChanged;
        }

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / meshSettings.meshWorldSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / meshSettings.meshWorldSize);

        Vector2 lastViewedChunkCoord = new Vector2(meshSettings.chunkCount, meshSettings.chunkCount);

        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
                if (viewedChunkCoord.x < chunkCountDivided + 1 && viewedChunkCoord.x > (chunkCountDivided + 1) * -1 &&
                    viewedChunkCoord.y < chunkCountDivided + 1 &&
                    viewedChunkCoord.y > (chunkCountDivided + 1) * -1)
                {
                    lastViewedChunkCoord = viewedChunkCoord;
                    if (!alreadyUpdatedChunkCoords.Contains(viewedChunkCoord))
                    {
                        if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                        {
                            terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                        }
                        else
                        {
                            TerrainChunk newChunk = new TerrainChunk(viewedChunkCoord, heightMapSettings, meshSettings,
                                objectDataSettings,
                                detailLevels, colliderLODIndex, transform, viewer,
                                mapMaterial, fallOffMap);
                            terrainChunkDictionary.Add(viewedChunkCoord, newChunk);
                            newChunk.onVisibilityChanged += OnChunkVisibilityChanged;
                            newChunk.onMeshChanged += OnMeshChanged;
                            newChunk.Load();
                        }
                    }
                }
            }
        }
    }

    void OnChunkVisibilityChanged(TerrainChunk chunk, bool isVisible)
    {
        if (isVisible)
        {
            visibleTerrainChunks.Add(chunk);
        }
        else
        {
            visibleTerrainChunks.Remove(chunk);
        }
    }

    void OnMeshChanged(TerrainChunk chunk)
    {
        UnityEngine.Random.InitState((int)(heightMapSettings.noiseSettings.seed + (long)chunk.coord.x * 100 +
                                           chunk.coord.y)); //so it doesn't form a (noticable) pattern of similar tiles
        PlaceObject(chunk);
        RandomizeInitState();
        if (navMeshCR != null)
        {
            StopCoroutine(BuildNavMesh());
        }

        loadedChunks++;
        LoadingHandler.SetLoadingPercentage((loadedChunks * 100) / (meshSettings.chunkCount * meshSettings.chunkCount));
        navMeshCR = StartCoroutine(BuildNavMesh());
    }

    IEnumerator BuildNavMesh()
    {
        yield return new WaitForSeconds(0.3f);
        if (hasBuildedNavMesh)
        {
            if (!RandomPointOnNavMesh.found && RandomPointOnNavMesh.i >= RandomPointOnNavMesh.TIMES)
            {
                Vector3 playerPosition = RandomPointOnNavMesh.GetPoinntForPlayerAndPOIOnNavMesh(meshSettings);
                player.GetComponent<NavMeshAgent>().enabled = true;
                poi.GetComponent<NavMeshAgent>().enabled = true;
                poi.GetComponent<POIController>().enabled = true;
                player.transform.position = playerPosition;
                LoadingHandler.AddLoadingPercentage(10);
            }

            GetComponent<NavMeshSurface>().UpdateNavMesh(GetComponent<NavMeshSurface>().navMeshData);
        }
        else
        {
            GetComponent<NavMeshSurface>().BuildNavMesh();
            hasBuildedNavMesh = true;
            navMeshCR = null;
            Vector3 playerPosition = RandomPointOnNavMesh.GetPoinntForPlayerAndPOIOnNavMesh(meshSettings);
            if (RandomPointOnNavMesh.found)
            {
                player.GetComponent<NavMeshAgent>().enabled = true;
                poi.GetComponent<NavMeshAgent>().enabled = true;
                poi.GetComponent<POIController>().enabled = true;
                player.transform.position = playerPosition;
                LoadingHandler.AddLoadingPercentage(10);
            }
        }
    }

    public void PlaceObject(TerrainChunk chunk)
    {
        for (int i = 0; i < numObjects; i++)
        {
            int prefabType = UnityEngine.Random.Range(0, objectDataSettings.placeableObjects.Length);
            Vector3 startPoint = RandomPointAboveTerrain(chunk);

            RaycastHit hit;
            bool hasHit = Physics.Raycast(startPoint, Vector3.down, out hit);
            if (hasHit && hit.collider.CompareTag("Terrain") && hit.point.y > 0.3f && hit.point.y < 16.0f)
            {
                Quaternion orientation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
                RaycastHit boxHit;
                if (Physics.BoxCast(startPoint, objectDataSettings.placeableObjectSizes[prefabType], Vector3.down,
                        out boxHit, orientation) && boxHit.collider.CompareTag("Terrain"))
                {
                    Instantiate(objectDataSettings.placeableObjects[prefabType],
                        new Vector3(startPoint.x, hit.point.y, startPoint.z), orientation, chunk.meshObject.transform);
                }
            }
        }
    }

    private void RandomizeInitState()
    {
        UnityEngine.Random.InitState((int)System.DateTime.UtcNow
            .Ticks); //casting a long to an int "loops" it (like modulo)
    }

    private Vector3 RandomPointAboveTerrain(TerrainChunk chunk)
    {
        return new Vector3(
            UnityEngine.Random.Range((chunk.coord.x * meshSettings.numVertsPerLine) - meshSettings.numVertsPerLine / 2,
                (chunk.coord.x * meshSettings.numVertsPerLine) + meshSettings.numVertsPerLine / 2),
            transform.position.y + heightMapSettings.maxHeight * 2,
            UnityEngine.Random.Range((chunk.coord.y * meshSettings.numVertsPerLine) - meshSettings.numVertsPerLine / 2,
                (chunk.coord.y * meshSettings.numVertsPerLine) + meshSettings.numVertsPerLine / 2)
        );
    }
}

[System.Serializable]
public struct LODInfo
{
    [Range(0, MeshSettings.numSupportedLODs - 1)]
    public int LOD;

    public float visibleDstThreshold;

    public float sqrVisibleDstThreshold
    {
        get { return visibleDstThreshold * visibleDstThreshold; }
    }
}