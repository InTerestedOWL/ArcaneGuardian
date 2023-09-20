using System.Collections;
using System.Collections.Generic;
using AG.UI.Draggable;
using UnityEngine;
using UnityEngine.EventSystems;
//based on https://forum.unity.com/threads/how-to-detect-if-mouse-is-over-ui.1025533/
public class MouseOverUI : MonoBehaviour
{
    int UIWindowLayer;
    int UILayer;
    private ActionBarSlotUI absu;
    private ResourceTooltips rt;
    void Start()
    {
        UIWindowLayer = LayerMask.NameToLayer("UI Window");
        UILayer = LayerMask.NameToLayer("UI");
    }
 
    void Update()
    {
       IsPointerOverUIElement();
    }
 
 
    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
 
    private void clearGO(){
        absu.showInfoBox(false);
        absu = null;
    }
    private void clearRT(){
        rt.showResourceTooltip(false);
        rt = null;
    }
    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        if(absu!=null){
            clearGO();
        }
        if(rt != null){
            clearRT();
        }
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaycastResult = eventSystemRaysastResults[index];
            if (curRaycastResult.gameObject.layer == UIWindowLayer){
                string n = curRaycastResult.gameObject.name;
                //Actionbar
                if(n.Contains("Action Bar Slot")){
                    absu = curRaycastResult.gameObject.GetComponent<ActionBarSlotUI>();
                    absu.showInfoBox(true);
                } else if(n.Contains("Skill Icon")||n.Contains("BuildingCostText")||n.Contains("BuildingCostIcon")){
                    absu = curRaycastResult.gameObject.transform.GetComponentInParent<ActionBarSlotUI>();
                    if(absu!= null){
                        absu.showInfoBox(true);
                    }          
                }
                return true;
            }
            else if(curRaycastResult.gameObject.layer == UILayer){
                string n = curRaycastResult.gameObject.name;
                if(n.Contains("Gold Container")
                ||n.Contains("Score Container")
                ||n.Contains("Wave Container")
                ||n.Contains("EnemiesToSpawn Container")
                ||n.Contains("CurrentWave Container")
                ||n.Contains("WaveTimeContainer")){
                    rt = curRaycastResult.gameObject.GetComponent<ResourceTooltips>();
                    rt.showResourceTooltip(true);
                }
                else if(n.Contains("EnemiesAliveText")||n.Contains("EnemiesAliveIcon")||
                    n.Contains("EnemiesToSpawnText")||n.Contains("EnemiesToSpawnIcon")||
                    n.Contains("CurrentWaveText")||n.Contains("CurrentWaveIcon")||
                    n.Contains("TimeToNextWave")||
                    n.Contains("ScoreIcon")||n.Contains("ScoreText")||
                    n.Contains("GoldText")||n.Contains("GoldIcon")){
                        Debug.Log("found trigger");
                        rt = curRaycastResult.gameObject.transform.GetComponentInParent<ResourceTooltips>();
                        if(rt != null){
                            Debug.Log("!=null");
                            rt.showResourceTooltip(true);
                        }
                    }
                    return true;
            }     
        }
        return false;
    }
 
 
    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
 
}