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

        /*if (viewerPosition != viewerPositionOld)
        {
            foreach (TerrainChunk chunk in visibleTerrainChunks)
            {
                chunk.UpdateCollisionMesh();
            }
        }*/       
        
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
                if (viewedChunkCoord.x < chunkCountDivided + 1 && viewedChunkCoord.x > (chunkCountDivided + 1) * -1 && viewedChunkCoord.y < chunkCountDivided + 1 &&
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
        UnityEngine.Random.InitState((int)(heightMapSettings.noiseSettings.seed + (long)chunk.coord.x * 100 + chunk.coord.y));//so it doesn't form a (noticable) pattern of similar tiles
        PlaceObject(chunk);
        RandomizeInitState();
        if (navMeshCR != null) {
            StopCoroutine(BuildNavMesh());
        }
        
        loadedChunks++;
        LoadingHandler.SetLoadingPercentage((loadedChunks * 100) / (meshSettings.chunkCount * meshSettings.chunkCount));
        navMeshCR = StartCoroutine(BuildNavMesh());
    }

    IEnumerator BuildNavMesh()
    {
        yield return new WaitForSeconds(0.3f);
        if (hasBuildedNavMesh) {
            if (!RandomPointOnNavMesh.found && RandomPointOnNavMesh.i >= RandomPointOnNavMesh.TIMES) {
                Vector3 playerPosition  = RandomPointOnNavMesh.GetPoinntForPlayerAndPOIOnNavMesh(meshSettings);
                player.GetComponent<NavMeshAgent>().enabled = true;
                poi.GetComponent<NavMeshAgent>().enabled = true;
                poi.GetComponent<POIController>().enabled = true;
                player.transform.position = playerPosition;
                LoadingHandler.AddLoadingPercentage(10);
            }
            GetComponent<NavMeshSurface>().UpdateNavMesh(GetComponent<NavMeshSurface>().navMeshData);
                
        } else {
            GetComponent<NavMeshSurface>().BuildNavMesh();
            hasBuildedNavMesh = true;
            navMeshCR = null;
            Vector3 playerPosition  = RandomPointOnNavMesh.GetPoinntForPlayerAndPOIOnNavMesh(meshSettings);
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

        /*int numObjects = 10;*///UnityEngine.Random.Range(objectDataSettings.minObjectsPerTile, objectDataSettings.minObjectsPerTile);
        for (int i = 0; i < numObjects; i++) {
            int prefabType = UnityEngine.Random.Range(0, objectDataSettings.placeableObjects.Length);
            Vector3 startPoint = RandomPointAboveTerrain(chunk);

            RaycastHit hit;
            bool hasHit = Physics.Raycast(startPoint, Vector3.down, out hit);
            if (hasHit && hit.collider.CompareTag("Terrain") && hit.point.y > 0.3f && hit.point.y < 16.0f) {
                Quaternion orientation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
                RaycastHit boxHit;
                if (Physics.BoxCast(startPoint, objectDataSettings.placeableObjectSizes[prefabType], Vector3.down, out boxHit, orientation) && boxHit.collider.CompareTag("Terrain")) {
                    Instantiate(objectDataSettings.placeableObjects[prefabType], new Vector3(startPoint.x, hit.point.y, startPoint.z), orientation, chunk.meshObject.transform);
                }
                
                //DrawBoxCastBox(startPoint, objectDataSettings.placeableObjectSizes[prefabType], orientation, Vector3.down, 10000, Color.red);
            }
            //Debug.DrawRay(startPoint, Vector3.down * 10000, Color.blue);
            //Debug.Break();
        }
    }
    
    private void RandomizeInitState() {
        UnityEngine.Random.InitState((int)System.DateTime.UtcNow.Ticks);//casting a long to an int "loops" it (like modulo)
    }
    
    private Vector3 RandomPointAboveTerrain(TerrainChunk chunk) {
        return new Vector3(
            UnityEngine.Random.Range((chunk.coord.x * meshSettings.numVertsPerLine) - meshSettings.numVertsPerLine / 2, (chunk.coord.x * meshSettings.numVertsPerLine) + meshSettings.numVertsPerLine / 2),
            transform.position.y + heightMapSettings.maxHeight * 2,
            UnityEngine.Random.Range((chunk.coord.y * meshSettings.numVertsPerLine) - meshSettings.numVertsPerLine / 2, (chunk.coord.y * meshSettings.numVertsPerLine) + meshSettings.numVertsPerLine / 2)
        );
    }
    
    //Draws just the box at where it is currently hitting.
    public static void DrawBoxCastOnHit(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float hitInfoDistance, Color color) {
        origin = CastCenterOnCollision(origin, direction, hitInfoDistance);
        DrawBox(origin, halfExtents, orientation, color);
    }

    //Draws the full box from start of cast to its end distance. Can also pass in hitInfoDistance instead of full distance
    public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, Color color) {
        direction.Normalize();
        Box bottomBox = new Box(origin, halfExtents, orientation);
        Box topBox = new Box(origin + (direction * distance), halfExtents, orientation);

        Debug.DrawLine(bottomBox.backBottomLeft, topBox.backBottomLeft, color);
        Debug.DrawLine(bottomBox.backBottomRight, topBox.backBottomRight, color);
        Debug.DrawLine(bottomBox.backTopLeft, topBox.backTopLeft, color);
        Debug.DrawLine(bottomBox.backTopRight, topBox.backTopRight, color);
        Debug.DrawLine(bottomBox.frontTopLeft, topBox.frontTopLeft, color);
        Debug.DrawLine(bottomBox.frontTopRight, topBox.frontTopRight, color);
        Debug.DrawLine(bottomBox.frontBottomLeft, topBox.frontBottomLeft, color);
        Debug.DrawLine(bottomBox.frontBottomRight, topBox.frontBottomRight, color);

        DrawBox(bottomBox, color);
        DrawBox(topBox, color);
    }

    public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color) {
        DrawBox(new Box(origin, halfExtents, orientation), color);
    }
    public static void DrawBox(Box box, Color color) {
        Debug.DrawLine(box.frontTopLeft, box.frontTopRight, color);
        Debug.DrawLine(box.frontTopRight, box.frontBottomRight, color);
        Debug.DrawLine(box.frontBottomRight, box.frontBottomLeft, color);
        Debug.DrawLine(box.frontBottomLeft, box.frontTopLeft, color);

        Debug.DrawLine(box.backTopLeft, box.backTopRight, color);
        Debug.DrawLine(box.backTopRight, box.backBottomRight, color);
        Debug.DrawLine(box.backBottomRight, box.backBottomLeft, color);
        Debug.DrawLine(box.backBottomLeft, box.backTopLeft, color);

        Debug.DrawLine(box.frontTopLeft, box.backTopLeft, color);
        Debug.DrawLine(box.frontTopRight, box.backTopRight, color);
        Debug.DrawLine(box.frontBottomRight, box.backBottomRight, color);
        Debug.DrawLine(box.frontBottomLeft, box.backBottomLeft, color);
    }

    public struct Box {
        public Vector3 localFrontTopLeft { get; private set; }
        public Vector3 localFrontTopRight { get; private set; }
        public Vector3 localFrontBottomLeft { get; private set; }
        public Vector3 localFrontBottomRight { get; private set; }
        public Vector3 localBackTopLeft { get { return -localFrontBottomRight; } }
        public Vector3 localBackTopRight { get { return -localFrontBottomLeft; } }
        public Vector3 localBackBottomLeft { get { return -localFrontTopRight; } }
        public Vector3 localBackBottomRight { get { return -localFrontTopLeft; } }

        public Vector3 frontTopLeft { get { return localFrontTopLeft + origin; } }
        public Vector3 frontTopRight { get { return localFrontTopRight + origin; } }
        public Vector3 frontBottomLeft { get { return localFrontBottomLeft + origin; } }
        public Vector3 frontBottomRight { get { return localFrontBottomRight + origin; } }
        public Vector3 backTopLeft { get { return localBackTopLeft + origin; } }
        public Vector3 backTopRight { get { return localBackTopRight + origin; } }
        public Vector3 backBottomLeft { get { return localBackBottomLeft + origin; } }
        public Vector3 backBottomRight { get { return localBackBottomRight + origin; } }

        public Vector3 origin { get; private set; }

        public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents) {
            Rotate(orientation);
        }
        public Box(Vector3 origin, Vector3 halfExtents) {
            this.localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
            this.localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

            this.origin = origin;
        }


        public void Rotate(Quaternion orientation) {
            localFrontTopLeft = RotatePointAroundPivot(localFrontTopLeft, Vector3.zero, orientation);
            localFrontTopRight = RotatePointAroundPivot(localFrontTopRight, Vector3.zero, orientation);
            localFrontBottomLeft = RotatePointAroundPivot(localFrontBottomLeft, Vector3.zero, orientation);
            localFrontBottomRight = RotatePointAroundPivot(localFrontBottomRight, Vector3.zero, orientation);
        }
    }

    //This should work for all cast types
    static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance) {
        return origin + (direction.normalized * hitInfoDistance);
    }

    static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation) {
        Vector3 direction = point - pivot;
        return pivot + rotation * direction;
    }
    
    
}

[System.Serializable]
public struct LODInfo
{
    [Range(0,MeshSettings.numSupportedLODs -1)]
    public int LOD;
    public float visibleDstThreshold;

    public float sqrVisibleDstThreshold
    {
        get
        {
            return visibleDstThreshold * visibleDstThreshold;
        }
    }
}
