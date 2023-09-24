using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerResources : MonoBehaviour
{
    [SerializeField] private int gold;

    public List<TMP_Text> buildingCostTexts;
    
    private ResourceUI resourceUI;

    private Color colorGold = new Color32(225, 203, 77, 255);

    private Color colorRed = new Color32(186, 22, 39, 255);
    private int score = 0;
    // Start is called before the first frame update
    public void updateBuildingCostUI(){
        int number = 0;
        foreach(TMP_Text t in buildingCostTexts){
            number = 0;
            bool r = int.TryParse(t.text, out number);
            //Debug.Log("r:"+r+"number:"+number);
            if(r && number > gold){
                t.color = colorRed;
            }else{
                t.color = colorGold;
            }
        }
    }
    public void addGold(int g){
        gold += g;
        resourceUI.setGold(gold);
        updateBuildingCostUI();
    }
    public void subtractGold(int g){
        gold -= g;
        resourceUI.setGold(gold);
        updateBuildingCostUI();
    }
    public void setGold(int g){
        gold = g;
        resourceUI.setGold(gold);
        updateBuildingCostUI();
    }
    public void addScore(int s){
        score += s;
        resourceUI.setScore(score);
    }
    public int getGold(){
        return gold;
    }
    public int getScore(){
        return score;
    }
    void Start()
    {
        resourceUI = GameObject.Find("Resource Container").GetComponent<ResourceUI>();
        resourceUI.setGold(gold);
        resourceUI.setScore(score);
        updateBuildingCostUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
