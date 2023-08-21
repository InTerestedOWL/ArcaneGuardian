using System.Drawing;
using UnityEngine;

public class POIBounds : MonoBehaviour
{
    public Material outlineMaterial;
    public GameObject planeOutlineObject;
    private int size;
    void Start()
    {
        BuildingSystem bs = GameObject.Find("Grid").GetComponent<BuildingSystem>();
        size = bs.poi_building.getMakeAreaPlacableSize();
        Create2DPlaneOutline();
    }

    void Create2DPlaneOutline()
    {
        // Create a new GameObject to hold the plane outline
        planeOutlineObject = new GameObject("2DPlaneOutline");

        // Attach a LineRenderer component to the plane outline object
        LineRenderer lineRenderer = planeOutlineObject.AddComponent<LineRenderer>();
        lineRenderer.material = outlineMaterial;
        // Set the number of positions (corners) for the line renderer
        lineRenderer.positionCount = 5;

        // Set the positions (corners) of the outline
        Vector3[] positions = new Vector3[5];
        positions[0] = new Vector3(-size / 2, 0.5f, -size / 2);
        positions[1] = new Vector3(size / 2, 0.5f, -size / 2);
        positions[2] = new Vector3(size / 2, 0.5f, size / 2);
        positions[3] = new Vector3(-size / 2, 0.5f, size / 2);
        positions[4] = new Vector3(-size / 2, 0.5f, -size / 2); // Close the loop

        lineRenderer.SetPositions(positions);

        // Customize the appearance of the line renderer
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        // Set the position of the plane outline object (adjust as needed)
        
        planeOutlineObject.gameObject.SetActive(false);
    }
    public void changePosition(Vector3Int pos){
        Debug.Log("pos should change");
        planeOutlineObject.gameObject.transform.position = new Vector3(100.0f,100.0f,1.0f);
    }
    public void changeSize(int s){
        size = s;
        Vector3[] positions = new Vector3[5];
        positions[0] = new Vector3(-size / 2, 0.5f, -size / 2);
        positions[1] = new Vector3(size / 2, 0.5f, -size / 2);
        positions[2] = new Vector3(size / 2, 0.5f, size / 2);
        positions[3] = new Vector3(-size / 2, 0.5f, size / 2);
        positions[4] = new Vector3(-size / 2, 0.5f, -size / 2); // Close the loop

        planeOutlineObject.GetComponent<LineRenderer>().SetPositions(positions);
    }
}
