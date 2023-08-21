using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] private int gold;
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
    public int getGold(){
        return gold;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
