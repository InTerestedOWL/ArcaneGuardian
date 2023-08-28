using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] private int gold;

    private int points = 0;
    // Start is called before the first frame update
    public void addGold(int g){
        gold += g;
    }
    public void subtractGold(int g){
        gold -= g;
    }
    public void setGold(int g){
        gold = g;
    }
    public void addPoints(int p){
        points = p;
    }
    public int getGold(){
        return gold;
    }
    public int getPoints(){
        return points;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
