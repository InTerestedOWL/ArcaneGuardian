using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapPreview : MonoBehaviour
{
    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer MeshRenderer;

    public enum DrawMode
    {
        NoiseMap,
        Mesh,
        FalloffMap,
        Trees
    }

    public DrawMode drawMode;

    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureData textureData;
    public ObjectDataSettings objectDataSettings;
    public Material terrainMaterial;

    private GameObject previewMesh;

    public int numObjects;

    [Range(0, MeshSettings.numSupportedLODs - 1)]
    public int previewLevelOfDetail;

    public bool autoUpdate;

    public void DrawMapInEditor()
    {
        RemoveAllObjectsWhichAreSpawnedRandomly();
        textureData.ApplyToMaterial(terrainMaterial);
        textureData.UpdateMeshHeights(terrainMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);
        HeightMap heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine,
            meshSettings.numVertsPerLine, heightMapSettings, Vector2.zero, new Vector2(0f,0f), meshSettings.numVertsPerLine, false, new float[0,0], 1);

        previewMesh = GameObject.Find("PreviewMesh");
        //numObjects = Random.Range(objectDataSettings.minObjectsPerTile, objectDataSettings.maxObjectsPerTile);

        if (drawMode == DrawMode.NoiseMap)
        {
            DrawTexture(TextureGenerator.TextureFromHeightMap(heightMap));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, previewLevelOfDetail));
        }
        else if (drawMode == DrawMode.Trees)
        {
            DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, previewLevelOfDetail));
            PlaceObject();
        }
        else if (drawMode == DrawMode.FalloffMap)
        {
            DrawTexture(TextureGenerator.TextureFromHeightMap(
                new HeightMap(FallOffGenerator.GenerateFalloffMap(5*meshSettings.numVertsPerLine), 0, 1)));
        }
    }

    private void RemoveAllObjectsWhichAreSpawnedRandomly()
    {
        Debug.Log("Clean the action");
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");

        foreach (GameObject tree in trees)
        {
            DestroyImmediate(tree);
        }
    }

    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);

        textureRenderer.gameObject.SetActive(true);
        meshFilter.gameObject.SetActive(false);
    }

    public void DrawMesh(MeshData meshData)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        textureRenderer.gameObject.SetActive(false);
        meshFilter.gameObject.SetActive(true);
    }

    void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
            DrawMapInEditor();
        }
    }

    void OnTextureValuesUpdated()
    {
        textureData.ApplyToMaterial(terrainMaterial);
    }

    private void OnValidate()
    {
        if (meshSettings != null)
        {
            meshSettings.OnValuesUpdated -= OnValuesUpdated;
            meshSettings.OnValuesUpdated += OnValuesUpdated;
        }

        if (heightMapSettings != null)
        {
            heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
            heightMapSettings.OnValuesUpdated += OnValuesUpdated;
        }

        if (textureData != null)
        {
            textureData.OnValuesUpdated -= OnTextureValuesUpdated;
            textureData.OnValuesUpdated += OnTextureValuesUpdated;
        }
    }

    public void PlaceObject()
    {
        numObjects = 10;
        int count = 0;
        Debug.Log(numObjects);
        for (int i = 0; i < numObjects; count++)
        {
            int prefabType = Random.Range(0, objectDataSettings.placeableObjects.Length);
            Vector3 startPoint = RandomPointAboveTerrain();

            RaycastHit hit;
            Debug.Log(startPoint);
            bool hasHit = Physics.Raycast(startPoint, Vector3.down, out hit);
            Debug.Log(hasHit);
            if (hasHit && hit.point.y < 10.0f && hit.collider.CompareTag("Terrain"))
            {
                Quaternion orientation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
                RaycastHit boxHit;
                if (Physics.BoxCast(startPoint, objectDataSettings.placeableObjectSizes[prefabType], Vector3.down,
                        out boxHit, orientation) && boxHit.collider.CompareTag("Terrain"))
                {
                    GameObject po = Instantiate(objectDataSettings.placeableObjects[prefabType],
                        new Vector3(startPoint.x, hit.point.y, startPoint.z), orientation, previewMesh.transform);
                    po.tag = "Tree";
                    i++;
                }

                DrawBoxCastBox(startPoint, objectDataSettings.placeableObjectSizes[prefabType], orientation,
                    Vector3.down, 10000, Color.red);
            }

            if (count > numObjects * 2)
            {
                return;
            }

            Debug.DrawRay(startPoint, Vector3.down * 10000, Color.blue);
            //Debug.Break();
        }
    }

    private void RandomizeInitState()
    {
        UnityEngine.Random.InitState((int)System.DateTime.UtcNow
            .Ticks); //casting a long to an int "loops" it (like modulo)
    }

    private Vector3 RandomPointAboveTerrain()
    {
        return new Vector3(
            UnityEngine.Random.Range(transform.position.x - meshSettings.meshWorldSize / 2,
                transform.position.x + meshSettings.meshWorldSize / 2),
            transform.position.y + heightMapSettings.maxHeight * 2,
            UnityEngine.Random.Range(transform.position.z - meshSettings.meshWorldSize / 2,
                transform.position.z + meshSettings.meshWorldSize / 2)
        );
    }

    public static void DrawBoxCastOnHit(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction,
        float hitInfoDistance, Color color)
    {
        origin = CastCenterOnCollision(origin, direction, hitInfoDistance);
        DrawBox(origin, halfExtents, orientation, color);
    }

    //Draws the full box from start of cast to its end distance. Can also pass in hitInfoDistance instead of full distance
    public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction,
        float distance, Color color)
    {
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

    public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color)
    {
        DrawBox(new Box(origin, halfExtents, orientation), color);
    }

    public static void DrawBox(Box box, Color color)
    {
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

    public struct Box
    {
        public Vector3 localFrontTopLeft { get; private set; }
        public Vector3 localFrontTopRight { get; private set; }
        public Vector3 localFrontBottomLeft { get; private set; }
        public Vector3 localFrontBottomRight { get; private set; }

        public Vector3 localBackTopLeft
        {
            get { return -localFrontBottomRight; }
        }

        public Vector3 localBackTopRight
        {
            get { return -localFrontBottomLeft; }
        }

        public Vector3 localBackBottomLeft
        {
            get { return -localFrontTopRight; }
        }

        public Vector3 localBackBottomRight
        {
            get { return -localFrontTopLeft; }
        }

        public Vector3 frontTopLeft
        {
            get { return localFrontTopLeft + origin; }
        }

        public Vector3 frontTopRight
        {
            get { return localFrontTopRight + origin; }
        }

        public Vector3 frontBottomLeft
        {
            get { return localFrontBottomLeft + origin; }
        }

        public Vector3 frontBottomRight
        {
            get { return localFrontBottomRight + origin; }
        }

        public Vector3 backTopLeft
        {
            get { return localBackTopLeft + origin; }
        }

        public Vector3 backTopRight
        {
            get { return localBackTopRight + origin; }
        }

        public Vector3 backBottomLeft
        {
            get { return localBackBottomLeft + origin; }
        }

        public Vector3 backBottomRight
        {
            get { return localBackBottomRight + origin; }
        }

        public Vector3 origin { get; private set; }

        public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
        {
            Rotate(orientation);
        }

        public Box(Vector3 origin, Vector3 halfExtents)
        {
            this.localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
            this.localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

            this.origin = origin;
        }


        public void Rotate(Quaternion orientation)
        {
            localFrontTopLeft = RotatePointAroundPivot(localFrontTopLeft, Vector3.zero, orientation);
            localFrontTopRight = RotatePointAroundPivot(localFrontTopRight, Vector3.zero, orientation);
            localFrontBottomLeft = RotatePointAroundPivot(localFrontBottomLeft, Vector3.zero, orientation);
            localFrontBottomRight = RotatePointAroundPivot(localFrontBottomRight, Vector3.zero, orientation);
        }
    }

    //This should work for all cast types
    static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance)
    {
        return origin + (direction.normalized * hitInfoDistance);
    }

    static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        Vector3 direction = point - pivot;
        return pivot + rotation * direction;
    }
}