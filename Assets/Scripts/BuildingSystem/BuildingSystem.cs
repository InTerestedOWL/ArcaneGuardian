using System;
using System.Collections;
using System.Collections.Generic;
using AG.Control;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using System.Linq;
using AG.Combat;


public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;
    public GridLayout gridLayout;
    private Grid grid;

    private PlayerResources pr;

    private POIController poiController;

    private InformationWindow iWindow;

    private ConfirmationWindow cWindow;
    public POIBuilding poi_building;
    private bool buildingContext = false;
    private bool buildingPOI = false;
    private Vector3Int poiLastCenter;
    POIBuildingState pbs;

    [SerializeField] private Tilemap MainTilemap;
    
    [SerializeField] private TileBase tilePlacable;
    [SerializeField] private TileBase tileNotPlacable;
    [SerializeField] private TileBase tilePending;

    [SerializeField] private LayerMask gridLayerMask;
    [SerializeField] private Material isPlacableMat;
    [SerializeField] private Material isNotPlacableMat;

    [SerializeField] private float refundBuilding;
    
    private Material[] objectMaterials;
    private List<Material[]> objectChildMaterials;
    
    private PlaceableObject objectToPlace;
    
    private void Awake(){
        current = this;
        poi_building = new POIBuilding();
        
        grid = gridLayout.gameObject.GetComponent<Grid>();
        poiController = GameObject.Find("POI").GetComponent<POIController>();
        
    }
    void Start()
    {
        pr = GameObject.Find("Player").GetComponent<PlayerResources>();
        iWindow = GameObject.Find("Information Window").GetComponent<InformationWindow>();
        cWindow = GameObject.Find("Confirmation Window").GetComponent<ConfirmationWindow>();
        objectChildMaterials = new List<Material[]>();
    }
    
    
    public Vector3 GetMouseWorldPosition(){

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out RaycastHit raycastHit,1000,gridLayerMask)){
            
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
        obj.transform.SetParent(this.transform, true);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        objectMaterials = objectToPlace.GetComponent<MeshRenderer>().materials;

        Component[] childrenMeshRenderer = objectToPlace.GetComponentsInChildren<MeshRenderer>();
       
        foreach(MeshRenderer cmr in childrenMeshRenderer){
            objectChildMaterials.Add(cmr.materials);
        }
        
        
        obj.AddComponent<ObjectDrag>();
        if(getBuildingPOI()){
            poiLastCenter = gridLayout.WorldToCell(objectToPlace.GetCenter3D());
        }
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
    private bool BuildingCanBePlaced(PlaceableObject placeableObject){
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;
        area.size = new Vector3Int(area.size.x + 1, area.size.y + 1, area.size.z);
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach(var b in baseArray){
            if(b != tilePlacable && b != tilePending){
                return false;
            }
        }
        return true;
    }

    private bool POICanBePlaced(PlaceableObject placeableObject){
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;
        area.size = new Vector3Int(area.size.x + 1, area.size.y + 1, area.size.z);
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach(var b in baseArray){
            if(b == tileNotPlacable){
                return false;
            }
        }
        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size){
        MainTilemap.BoxFill(start, tileNotPlacable, start.x, start.y, start.x+size.x, start.y+size.y);
    }
    public void MakeAreaPending(Vector3Int start, Vector3Int size){       
        for(int x = start.x;x <= start.x+size.x; x++){
            for(int y= start.y;y <= start.y+size.y; y++){
                Vector3Int pos = new Vector3Int(x,y,0);
                
                if(MainTilemap.GetTile(pos) == tilePlacable){
                    MainTilemap.SetTile(pos,tilePending);
                }
            }
        }
    }
    public void MakeAreaPlacable(Vector3Int center){
        Debug.Log("MakeArea: "+center);
        int minX =  center.x - poi_building.getMakeAreaPlacableSize();
        int maxX =  center.x + poi_building.getMakeAreaPlacableSize() + 1;
        int minY =  center.y - poi_building.getMakeAreaPlacableSize();
        int maxY =  center.y + poi_building.getMakeAreaPlacableSize() + 1;
        int z = center.z;
        
        for(int i = minX;i < maxX; i++){
            for(int j = minY;j<maxY; j++){
                Vector3Int pos = new Vector3Int(i,j,z);
                
                if(MainTilemap.GetTile(pos) != tileNotPlacable){
                    MainTilemap.SetTile(pos,tilePlacable);
                }
            }
        }
    }
    public void MakePOIAreaPending(Vector3Int center){
        Debug.Log("MakeArea: "+center);
        int minX =  center.x - poi_building.getMakeAreaPlacableSize();
        int maxX =  center.x + poi_building.getMakeAreaPlacableSize() + 1;
        int minY =  center.y - poi_building.getMakeAreaPlacableSize();
        int maxY =  center.y + poi_building.getMakeAreaPlacableSize() + 1;
        int z = center.z;
        
        for(int i = minX;i < maxX; i++){
            for(int j = minY;j<maxY; j++){
                Vector3Int pos = new Vector3Int(i,j,z);
                
                if(MainTilemap.GetTile(pos) != tileNotPlacable){
                    MainTilemap.SetTile(pos,tilePending);
                }
            }
        }
    }
    public void clearPOIPending(Vector3Int center){
        int minX =  center.x - poi_building.getMakeAreaPlacableSize();
        int maxX =  center.x + poi_building.getMakeAreaPlacableSize() + 1;
        int minY =  center.y - poi_building.getMakeAreaPlacableSize();
        int maxY =  center.y + poi_building.getMakeAreaPlacableSize() + 1;
        int z = center.z;
        
        for(int i = minX;i < maxX; i++){
            for(int j = minY;j<maxY; j++){
                Vector3Int pos = new Vector3Int(i,j,z);
                
                if(MainTilemap.GetTile(pos) == tilePending){
                    MainTilemap.SetTile(pos,null);
                }
            }
        }
    }
    public void clearPending(Vector3Int center){
        int minX =  center.x - poi_building.getMakeAreaPlacableSize();
        int maxX =  center.x + poi_building.getMakeAreaPlacableSize() + 1;
        int minY =  center.y - poi_building.getMakeAreaPlacableSize();
        int maxY =  center.y + poi_building.getMakeAreaPlacableSize() + 1;
        int z = center.z;
        
        for(int i = minX;i < maxX; i++){
            for(int j = minY;j<maxY; j++){
                Vector3Int pos = new Vector3Int(i,j,z);
                
                if(MainTilemap.GetTile(pos) == tilePending){
                    MainTilemap.SetTile(pos,tilePlacable);
                }
            }
        }
    }
    public void placePOI(){
        poiController.stateMachine.ChangeState(AiStateId.POIBuilding);
    }
    public void followPOIConfirmation(){
        if(poi_building.getPlacedBuildings().Count > 0){
            int refundGold = 0;
            foreach(PlaceableObject po in poi_building.getPlacedBuildings()){           
            refundGold += (int)(po.getPrice() * refundBuilding);
            }
            int countBuildings = poi_building.getPlacedBuildings().Count;
            string text = "Soll der Quell der Magi dir wirklich folgen? Dabei werden alle deine Gebäude ("+countBuildings+") abgebaut und du erhältst "+refundGold+" Gold zurück.";
            cWindow.popupConfirmationWindow(text,followPOI,null);
        }
        else{
            followPOI();
        }
    }
    public void followPOI(){
        poiController.stateMachine.ChangeState(AiStateId.POIFollowPlayer);
    }
    public void freePOI(Vector3Int center){

        foreach(PlaceableObject po in poi_building.getPlacedBuildings()){
            Vector3Int start = gridLayout.WorldToCell(po.GetStartPosition());
            tileToPlacable(start,po.Size);
        }
        foreach(PlaceableObject po in poi_building.getPlacedBuildings()){           
            pr.addGold((int)(po.getPrice() * refundBuilding));  
            Destroy(po.gameObject);
        }
        poi_building.clearPlacedBuildings();
        int minX =  center.x - poi_building.getMakeAreaPlacableSize();
        int maxX =  center.x + poi_building.getMakeAreaPlacableSize() + 1;
        int minY =  center.y - poi_building.getMakeAreaPlacableSize();
        int maxY =  center.y + poi_building.getMakeAreaPlacableSize() + 1;
        int z = center.z;
        
        for(int i = minX;i < maxX; i++){
            for(int j = minY;j<maxY; j++){
                Vector3Int pos = new Vector3Int(i,j,z);
                if(MainTilemap.GetTile(pos) == tilePlacable){
                    MainTilemap.SetTile(pos,null);
                }
            }
        }
        poi_building.setPlaced(false);
    }
    public void tileToPlacable(Vector3Int start, Vector3Int size){
        MainTilemap.BoxFill(start, tilePlacable, start.x, start.y, start.x+size.x, start.y+size.y);
    }
    public void startBuilding(GameObject buildingObject){
        if(buildingContext){
            stopBuilding();
        }
        
        setBuildingContext(true);  
        InitializeWithObject(buildingObject);
        
        setBuildingPOI(buildingObject.tag == "POI_Building");
        if(buildingObject.tag == "POI_Building"){
            if(poi_building.getPlaced()){
                iWindow.popupInformationWindow("Quell der Magi ist bereits platziert!");
                stopBuilding();
            }
        }
        if(pr.getGold() < this.objectToPlace.getPrice()){
            Debug.Log("not enough minerals");
            iWindow.popupInformationWindow("Nicht genug Gold zum Bauen!");
            stopBuilding();
        }
        pbs = (POIBuildingState)poiController.stateMachine.states.Where(state => state is POIBuildingState).FirstOrDefault();       
    }

    public void stopBuilding(){
        if(this.getBuildingContext()){
            Destroy(objectToPlace.gameObject);
            clearPending(poi_building.getCenter3D());
            
            if(getBuildingPOI()){
                clearPOIPending(poiLastCenter);
            }
            setBuildingPOI(false);
            setBuildingContext(false);
            objectChildMaterials.Clear();
        }      
    }

    public void setBuildingContext(bool b){
        buildingContext = b;
    }
    public bool getBuildingContext(){
        return buildingContext;
    }
    public void setBuildingPOI(bool b){
        buildingPOI = b;
    }
    public bool getBuildingPOI(){

        return buildingPOI;
    }
 
    public void materialCanBePlaced(){
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
    public void materialCannotBePlaced(){
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
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)){               
            followPOIConfirmation();
        }
        
        if(buildingContext){

            if(Input.GetKeyDown(KeyCode.Mouse1)||Input.GetKeyDown(KeyCode.B)||Input.GetKeyDown(KeyCode.Escape)){
                stopBuilding();
                return;
            }
            if(Input.GetKeyDown(KeyCode.R)){               
                objectToPlace.Rotate();
            }
            if(getBuildingPOI()){
                clearPOIPending(poiLastCenter);
                poiLastCenter = gridLayout.WorldToCell(objectToPlace.GetCenter3D());
                MakePOIAreaPending(poiLastCenter);
                if(POICanBePlaced(objectToPlace)){
                    materialCanBePlaced();
                }
                else{
                    materialCannotBePlaced();
                }
            }
            else{
                clearPending(poi_building.getCenter3D());
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                MakeAreaPending(start,objectToPlace.Size);
                if(BuildingCanBePlaced(objectToPlace) && pbs.getArrived()){
                    materialCanBePlaced();   
                }
                else{
                    materialCannotBePlaced();
                }
            }

            if(Input.GetKeyDown(KeyCode.Mouse0)){
                if(getBuildingPOI()){
                    if(POICanBePlaced(objectToPlace)){
                        if(poi_building.getPlaced()==false){
                            poi_building.setPlaced(true);
                            poi_building.copySize(objectToPlace.Size);
                            poi_building.copyVertices(objectToPlace.Vertices);
                            poi_building.copyStartPosition(gridLayout.WorldToCell(objectToPlace.GetStartPosition()));
                            poi_building.copyCenter3D(gridLayout.WorldToCell(objectToPlace.GetCenter3D()));
                            placePOI();                            
                            TakeArea(poi_building.getStartPosition(),poi_building.getSize());
                            MakeAreaPlacable(poi_building.getCenter3D());
                            stopBuilding();
                        }  
                        else{
                            stopBuilding();
                        }
                    }else{
                        iWindow.popupInformationWindow("Kann hier nicht platziert werden!");
                    }
                }
                else if(!poi_building.getPlaced()){
                    iWindow.popupInformationWindow("Platziere zuerst den Quell der Magi!");
                }
                else if(!BuildingCanBePlaced(objectToPlace)){
                    iWindow.popupInformationWindow("Kann hier nicht platziert werden!");
                }
                else if(!pbs.getArrived()){
                    iWindow.popupInformationWindow("Quell der Magi noch nicht am Zielort!");
                }
                else if(BuildingCanBePlaced(objectToPlace)&& pbs.getArrived()){
                    objectToPlace.Place();
                    Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
                    TakeArea(start,objectToPlace.Size);

                    objectToPlace.GetComponent<MeshRenderer>().materials = objectMaterials;
                    objectToPlace.GetComponent<NavMeshObstacle>().enabled = true;
                    if(objectToPlace.GetComponent<CombatTarget>() != null){
                        objectToPlace.GetComponent<CombatTarget>().enabled = true;
                    }
                    if(objectToPlace.GetComponentInChildren<TurretController>() != null){
            
                        objectToPlace.GetComponentInChildren<TurretController>().enabled = true;
                    }                  
                    Component[] childrenMeshRenderer = objectToPlace.GetComponentsInChildren<MeshRenderer>();
                    int index = 0;
                    foreach(MeshRenderer cmr in childrenMeshRenderer){
                        cmr.materials = objectChildMaterials[index];
                        index++;
                    }
                    poi_building.addPlacedBuilding(objectToPlace);
                    pr.subtractGold(objectToPlace.getPrice());
                    setBuildingContext(false);
                    objectToPlace = null;
                    objectMaterials = null;
                    objectChildMaterials.Clear();                   
                }
            }
        }         
        if(!objectToPlace){
            return;
        }  
    }
}

public class POIBuilding
{
    private bool placed;
    private Vector3Int size;
    private Vector3[] vertices;
    private Vector3Int center3D;
    private Vector3Int startPosition;
    private int makeAreaPlacableSize;
    private List<PlaceableObject> placed_buildings;

    public POIBuilding(){
        vertices = new Vector3[4];
        makeAreaPlacableSize = 15;
        placed_buildings = new List<PlaceableObject>();      
    }
    public int getMakeAreaPlacableSize(){
        return makeAreaPlacableSize;
    }
    public bool getPlaced(){
        return placed;
    }
    public void setPlaced(bool p){
        placed = p;
    }
    public Vector3Int getSize(){
        return size;
    }
    public Vector3[] getvertices(){
        return vertices;
    }
    public Vector3Int getCenter3D(){
        return center3D;
    }
    public Vector3Int getStartPosition(){
        return startPosition;
    }
    public void copySize(Vector3Int s){  
            size = s + Vector3Int.zero;
    }   
    public void copyCenter3D(Vector3Int c){
        center3D = c + Vector3Int.zero;
    } 
    public void copyStartPosition(Vector3Int s){
        startPosition = s + Vector3Int.zero;
    } 
    public void copyVertices(Vector3[] v){
        for(int i = 0;i < vertices.Length;i++){
            vertices[i]=v[i]+Vector3.zero;         
        }
    }    
    public List<PlaceableObject> getPlacedBuildings(){
        return placed_buildings;
    }
    public void addPlacedBuilding(PlaceableObject po)
    {
        placed_buildings.Add(po);
    }
    public void clearPlacedBuildings()
    {
        placed_buildings.Clear();
    }
}
