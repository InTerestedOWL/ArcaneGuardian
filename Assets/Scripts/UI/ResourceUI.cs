using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{

    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text currentWaveText;
    [SerializeField] private TMP_Text enemiesAliveText;
    [SerializeField] private TMP_Text enemiesToSpawnText;
    [SerializeField] private TMP_Text timeToNextWaveText;

    // Start is called before the first frame update
    public void setGold(int g){
        goldText.text = g.ToString();
    }
    public void setScore(int s){
        scoreText.text = s.ToString();
    }

    public void setCurrentWaveText(int cw){
        currentWaveText.text = cw.ToString();
    }
    public void setEnemiesAliveText(int ea){
        enemiesAliveText.text = ea.ToString();
    }
    public void setEnemiesToSpawnText(int ets){
        enemiesToSpawnText.text = ets.ToString();
    }
    public void setTimeToNextWaveText(float ttnw){
        int t = (int)ttnw;
        timeToNextWaveText.text = t.ToString();
    }
    public void setTimeToNextWaveText(string text){
        timeToNextWaveText.text = text.ToString();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
