using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerResources : MonoBehaviour
{
    [SerializeField] private int gold;
    
    ResourceUI resourceUI;

    private int score = 0;
    // Start is called before the first frame update
    public void addGold(int g){
        gold += g;
        resourceUI.setGold(gold);
    }
    public void subtractGold(int g){
        gold -= g;
        resourceUI.setGold(gold);
    }
    public void setGold(int g){
        gold = g;
        resourceUI.setGold(gold);
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
