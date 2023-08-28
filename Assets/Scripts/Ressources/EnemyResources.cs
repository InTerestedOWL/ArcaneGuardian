using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyResources : MonoBehaviour
{
    [SerializeField] private int goldStart;
    [SerializeField] private int points;
    [SerializeField] private float dropVariance;
    // Start is called before the first frame update
    private PlayerResources pr;

    public void dropsGold(){
        System.Random random = new System.Random();
        float minValue = goldStart-goldStart*dropVariance;
        float maxValue = goldStart+goldStart*dropVariance;
        float randomDrop = (float)(random.NextDouble() * (maxValue - minValue) + minValue);
        int dropGold = (int) randomDrop;
        Debug.Log("I Dropped "+dropGold+" Gold");
        pr.addGold(dropGold);
    }
    public void addPoints(){
        pr.addPoints(points);
    }
    void Start()
    {
        pr = GameObject.Find("Player").GetComponent<PlayerResources>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
