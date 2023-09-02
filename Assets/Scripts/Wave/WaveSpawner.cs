using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Grundlagen von Tutorial https://www.youtube.com/watch?v=7T-MTo8Uaio

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}
public class WaveSpawner : MonoBehaviour
{
    private ResourceUI resourceUI;
    public List<List<Enemy>> enemies = new List<List<Enemy>>();
    public List<Enemy> goblins = new List<Enemy>();

    public List<Enemy> skeletons = new List<Enemy>();

    public List<Enemy> elves = new List<Enemy>();

    public List<Enemy> humans = new List<Enemy>();

    public List<Enemy> bosses = new List<Enemy>();

    public List<Enemy> specials = new List<Enemy>();
    public int currentWave;
    public int waveValueStart;
    public int valuePerWave;

    public int startGoblin;
    public int startSkeletons;
    public int startElves;
    public int startHumans;
    public int startAllTogether;

    private int goblinBossWave;
    private int skeletonBossWave;
    private int elvesBossWave;
    private int humanBossWave;
    private int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    // Start is called before the first frame update
     public Transform[] spawnLocation;
    private int spawnIndex;
    public float waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;

    private bool initialWaveStarted = false;


    private float timeToNextWave = 60;
    private float skipToTime = 5;
 
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    public float getTimeToNextWave(){
        return timeToNextWave;
    }
    public int getSpawnedEnemiesCount(){
        return spawnedEnemies.Count;
    }

    public int getEnemiesToSpawnCount(){
        return enemiesToSpawn.Count;
    }
    public int getCurrentWave(){
        return currentWave;
    }
    public void updateEnemiesAliveUI(){
        resourceUI.setEnemiesAliveText(spawnedEnemies.Count);
    }
    void Start()
    {
        enemies.Add(goblins);
        enemies.Add(skeletons);
        enemies.Add(elves);
        enemies.Add(humans);

        goblinBossWave = startSkeletons-1;
        skeletonBossWave = startElves-1;
        elvesBossWave = startHumans-1;
        humanBossWave = startAllTogether-1;
       
        resourceUI = GameObject.Find("Resource Container").GetComponent<ResourceUI>();
        resourceUI.setTimeToNextWaveText("Aufbau");
    }

    public void startWaves(){
        if(!initialWaveStarted){
            initialWaveStarted = true;
            timeToNextWave = skipToTime;
        }
    }

    public void skipTimeToNextWave(){
        if(waveTimer<=0 && spawnedEnemies.Count <=0 && timeToNextWave > skipToTime){
            timeToNextWave = skipToTime;  
        }
    }

   void Update(){
        if(Input.GetKeyDown(KeyCode.Q)){
            if(!initialWaveStarted){
                startWaves();
            }
            else{
                skipTimeToNextWave();
            }      
        }
    }
    void FixedUpdate()
    {
        if(spawnTimer <=0)
        {
            //spawn an enemy
            if(enemiesToSpawn.Count>0)
            {
                spawnIndex = Random.Range(0, spawnLocation.Length);
                GameObject enemy = (GameObject)Instantiate(enemiesToSpawn[0], spawnLocation[spawnIndex].position,Quaternion.identity); // spawn first enemy in our list
                enemy.transform.SetParent(this.transform, true);
                enemiesToSpawn.RemoveAt(0); // and remove it
                spawnedEnemies.Add(enemy);
                resourceUI.setEnemiesAliveText(spawnedEnemies.Count);
                resourceUI.setEnemiesToSpawnText(enemiesToSpawn.Count);
                spawnTimer = spawnInterval;
            }
            else
            {
                waveTimer = 0; // if no enemies remain, end wave
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }
 
        if(waveTimer<=0 && spawnedEnemies.Count <=0 && initialWaveStarted)
        {
            if(timeToNextWave <= 0){                
                currentWave++;
                resourceUI.setCurrentWaveText(currentWave);
                resourceUI.setTimeToNextWaveText("Angriff!");
                GenerateWave();
                timeToNextWave = 60;
            }
            else{
                timeToNextWave -= Time.fixedDeltaTime;
                resourceUI.setTimeToNextWaveText(timeToNextWave);
            }
            
        }
    }
 
    public void GenerateWave()
    {
        waveValue = waveValueStart + currentWave * valuePerWave;
        waveDuration = waveDuration + valuePerWave/2;
        
        GenerateEnemies();
        resourceUI.setEnemiesToSpawnText(enemiesToSpawn.Count);
        spawnInterval = waveDuration / (float)enemiesToSpawn.Count; // gives a fixed time between each enemies
        waveTimer = waveDuration; // wave duration is read only

    }
 
    public void GenerateEnemies()
    {
        // Create a temporary list of enemies to generate
        // 
        // in a loop grab a random enemy 
        // see if we can afford it
        // if we can, add it to our list, and deduct the cost.
 
        // repeat... 
 
        //  -> if we have no points left, leave the loop
 
        List<GameObject> generatedEnemies = new List<GameObject>();
        int randEnemyListStart = 0;
        int randEnemyListEnd = enemies.Count;
        
        if(currentWave >= startAllTogether){
            randEnemyListStart = 0;
            randEnemyListEnd = enemies.Count;
            //decide if bosswave
            int bosswaveValue = Random.Range(0, 100);
            if(bosswaveValue < 10){
                int randBossId = Random.Range(0, bosses.Count);
                generatedEnemies.Add(bosses[randBossId].enemyPrefab);
            }
        }
        else if(currentWave >= startHumans){
            randEnemyListStart = 3;
            randEnemyListEnd = 4;
            if(currentWave == humanBossWave){
                generatedEnemies.Add(bosses[3].enemyPrefab);
            }
        }
        else if(currentWave >= startElves){
            randEnemyListStart = 2;
            randEnemyListEnd = 3;
            if(currentWave == elvesBossWave){
                generatedEnemies.Add(bosses[2].enemyPrefab);
            }
        }
        else if(currentWave >= startSkeletons){
            randEnemyListStart = 1;
            randEnemyListEnd = 2;
            if(currentWave == skeletonBossWave){
                generatedEnemies.Add(bosses[1].enemyPrefab);
            }
        }
        else if(currentWave >= startGoblin){
            randEnemyListStart = 0;
            randEnemyListEnd = 1;
            if(currentWave == goblinBossWave){
                generatedEnemies.Add(bosses[0].enemyPrefab);
            }
        }            
              
        while(waveValue>0 || generatedEnemies.Count <50)
        {
            int randEnemyListId = Random.Range(randEnemyListStart, randEnemyListEnd);
            int randEnemyId = Random.Range(0, enemies[randEnemyListId].Count);
            int randEnemyCost = enemies[randEnemyListId][randEnemyId].cost;
 
            if(waveValue-randEnemyCost>=0)
            {
                generatedEnemies.Add(enemies[randEnemyListId][randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if(waveValue<=0)
            {
                break;
            }
        }

        //look if special mob can be in this wave based on currentwave > humans
        if(currentWave >= startHumans){
            //decide if special mob in this wave
            int specialWaveValue = Random.Range(0, 100);
            if(specialWaveValue < 5){
                int randSpecialId = Random.Range(0, specials.Count);
                int randInsertPosition = Random.Range(0, generatedEnemies.Count);
                generatedEnemies.Insert(randInsertPosition,specials[randSpecialId].enemyPrefab);
                Debug.Log("added Specialmob");
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }
  
}


