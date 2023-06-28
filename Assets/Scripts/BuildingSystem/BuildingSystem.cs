using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;
    public GridLayout gridLayout;
    private Grid grid;

    private bool buildingContext = false;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;

    [SerializeField] private LayerMask gridLayerMask;
    [SerializeField] private Material isPlacableMat;
    [SerializeField] private Material isNotPlacableMat;
    private Material[] objectMaterials;
  
    
    private PlaceableObject objectToPlace;
    // Start is called before the first frame update
    private void Awake(){
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    public Vector3 GetMouseWorldPosition(){

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out RaycastHit raycastHit,1000,gridLayerMask)){
            Debug.Log(raycastHit.collider.gameObject);
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue, 1f);
            return raycastHit.point;
        }
        else{
            return Vector3.zero;
        }
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position){
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(GetMouseWorldPosition());
        GameObject obj = Instantiate(prefab,position,Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        objectMaterials = objectToPlace.GetComponent<MeshRenderer>().materials;
        obj.AddComponent<ObjectDrag>();
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap){
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach(var v in area.allPositionsWithin){
            Vector3Int pos = new Vector3Int(v.x,v.y,v.z);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }
    private bool CanBePlaced(PlaceableObject placeableObject){
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;
        area.size = new Vector3Int(area.size.x + 1, area.size.y + 1, area.size.z);
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach(var b in baseArray){
            if(b == whiteTile){
                return false;
            }
        }
        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size){
        MainTilemap.BoxFill(start, whiteTile, start.x, start.y, start.x+size.x, start.y+size.y);
    }
    public void startBuilding(GameObject buildingObject){
        print("buildingcontext="+buildingContext);
        if(!buildingContext){     
            print("buildingcontext changed to "+buildingContext);     
            setBuildingContext(true);
            InitializeWithObject(buildingObject);
        }
    }

    public void stopBuilding(){
        if(this.getBuildingContext()){
            Destroy(objectToPlace.gameObject);
            setBuildingContext(false);
        }      
    }

    public void setBuildingContext(bool activated){
        buildingContext = activated;
    }
    public bool getBuildingContext(){
        return buildingContext;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(buildingContext){
            if(CanBePlaced(objectToPlace)){
                Material[] mats = new Material[objectToPlace.GetComponent<MeshRenderer>().materials.Length];
                for(int i=0;i < mats.Length;i++){
                    mats[i] = isPlacableMat;
                }
                objectToPlace.GetComponent<MeshRenderer>().materials= mats;
                
                Component[] childrenMeshRenderer = objectToPlace.GetComponentsInChildren<MeshRenderer>();
                foreach(MeshRenderer cmr in childrenMeshRenderer){
                    cmr.material = isPlacableMat;
                }
                
            }
            else{
             
                
                Material[] mats = new Material[objectToPlace.GetComponent<MeshRenderer>().materials.Length];
                for(int i=0;i < mats.Length;i++){
                    mats[i] = isNotPlacableMat;
                }
                objectToPlace.GetComponent<MeshRenderer>().materials= mats;

                
                
                Component[] childrenMeshRenderer = objectToPlace.GetComponentsInChildren<MeshRenderer>();
                foreach(MeshRenderer cmr in childrenMeshRenderer){
                    cmr.material = isNotPlacableMat;
                }
            }
            if(Input.GetKeyDown(KeyCode.R)){
                objectToPlace.Rotate();
            }
            else if(Input.GetKeyDown(KeyCode.Mouse0)){
                if(CanBePlaced(objectToPlace)){
                    objectToPlace.Place();
                    Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                    TakeArea(start,objectToPlace.Size);
                    objectToPlace.GetComponent<MeshRenderer>().materials = objectMaterials;
                    objectToPlace.GetComponent<NavMeshObstacle>().enabled = true;
                    Component[] childrenMeshRenderer = objectToPlace.GetComponentsInChildren<MeshRenderer>();
                    foreach(MeshRenderer cmr in childrenMeshRenderer){
                        cmr.material = objectMaterials[0];
                    }
                    setBuildingContext(false);
                }
            }
        }         
        if(!objectToPlace){
            return;
        }  
    }
}
