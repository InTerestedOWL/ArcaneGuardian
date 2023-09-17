using System.Collections;
using System.Collections.Generic;
using AG.Combat;
using AG.UI;
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

    private InformationWindow iWindow;

    Dictionary<int,string> specialDialogs = new Dictionary<int, string>();

    private GameObject player;
    private GameObject poi;
    private BuildingSystem bs;

    private SkillTree st;
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

    private bool doOnceDuringPause = true;

    private bool waveIsBossWave = false;


    public int skillPointsEveryXWave = 3;
    private int skillPointsToAdd = 0;
    private float timeToNextWave = 60;
    private float skipToTime = 5;

    [SerializeField]
    private GlobalAudioSystem globalAudioSystem;
    [SerializeField]
    private AudioClip undeadVoice;
    [SerializeField]
    private AudioClip undeadBossVoice;
    [SerializeField]
    private AudioClip humanVoice;
    [SerializeField]
    private AudioClip humanBossVoice;
    [SerializeField]
    private AudioClip elfVoice;
    [SerializeField]
    private AudioClip elfBossVoice;
    [SerializeField]
    private AudioClip goblinVoice;
    [SerializeField]
    private AudioClip goblinBossVoice;
    [SerializeField]
    private AudioClip announcerVoice;
    [SerializeField]
    private AudioClip waveStart;
    [SerializeField]
    private AudioClip alarm;
    private bool countdownPlaying = false;

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

    public void setSpawnLocations(Transform[] sls){
        spawnLocation = sls;
    }
    public void updateEnemiesAliveUI() {
        if (resourceUI != null)
            resourceUI.setEnemiesAliveText(spawnedEnemies.Count);
    }
    private void addSpecialDialogs(){
        specialDialogs.Add(startGoblin,"Goblins: 'Blue light. It's so shiny. I must have it!'");
        specialDialogs.Add(startSkeletons,"Skeletons: 'Ah, at last, we've found it. The Source of Magi, the very essence of power we seek.'");
        specialDialogs.Add(startElves,"Elves: 'The Source of Magi is too powerful! It must be destroyed at all costs!'"); 
        specialDialogs.Add(startHumans,"Humans: 'With this power our kingdom will rule the entire world! GIVE UP OR DIE!'");
        specialDialogs.Add(startAllTogether,"Now all enemies join forces!");

        specialDialogs.Add(goblinBossWave,"Goblinking Muruk: 'I GET BLUE LIGHT! YOU DIE!'");
        specialDialogs.Add(skeletonBossWave,"Skeletonking Brathas: 'With this power we can stop our suffering. Now you will suffer!'");
        specialDialogs.Add(elvesBossWave,"Elvenking Egolass: 'You are already corrupted by it's power. I will end your misery!'");
        specialDialogs.Add(humanBossWave,"Humanking Farian: 'You had your chance. This will be a bloodbath!'");        
    }
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        poi = GameObject.FindWithTag("POI");
        bs = GameObject.FindWithTag("BuildingGrid").GetComponent<BuildingSystem>();
        st = GameObject.Find("Skill Tree Container").GetComponent<SkillTree>();
        iWindow = GameObject.Find("Information Window").GetComponent<InformationWindow>();
        resourceUI = GameObject.Find("Resource Container").GetComponent<ResourceUI>();

        goblinBossWave = startSkeletons-1;
        skeletonBossWave = startElves-1;
        elvesBossWave = startHumans-1;
        humanBossWave = startAllTogether-1;
        addSpecialDialogs();
        enemies.Add(goblins);
        enemies.Add(skeletons);
        enemies.Add(elves);
        enemies.Add(humans);
         
        resourceUI.setTimeToNextWaveText("Building");
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
    private Vector3 GenerateEnemyPosition(){
        Vector3 poiPos = poi.transform.position;
        float x = (float)Random.Range(25f,50f);
        float z = (float)Random.Range(25f,50f);

        int signX = Random.Range(0,2);
        int signZ = Random.Range(0,2);
        if(signX>0){
            x= -x;
        }
        if(signZ>0){
            z= -z;
        }
        return new Vector3(poiPos.x+x,5,poiPos.z+z);
    }
    void FixedUpdate()
    {
        if(spawnTimer <=0)
        {
            //spawn an enemy

            if(enemiesToSpawn.Count>0)
            {
                Vector3 pos = GenerateEnemyPosition();
                spawnIndex = Random.Range(0, spawnLocation.Length);
                GameObject enemy = (GameObject)Instantiate(enemiesToSpawn[0], pos,Quaternion.identity); // spawn first enemy in our list
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

        if (timeToNextWave <= 3 && timeToNextWave > 0 && !countdownPlaying) {
            countdownPlaying = true;
            StartCoroutine(PlayCountdown());
        }

        if (waveTimer<=0 && spawnedEnemies.Count <=0 && initialWaveStarted)
        {
            if(doOnceDuringPause){
                healAtWaveEnd();
                
                if(skillPointsToAdd > 0){
                    st.AddSkillPoints(skillPointsToAdd);
                    iWindow.popupInformationWindow("You received "+skillPointsToAdd+" Skill Points! Open your  Skill Tree (L) to spend them!");
                }
                
                

                skillPointsToAdd = 0;
                waveIsBossWave = false;
                doOnceDuringPause = false;
                if (currentWave > 0) {
                    globalAudioSystem.PlayMusic(GamePhase.RestPhase);
                }
            }

            if(timeToNextWave <= 0){
                globalAudioSystem.PlayVoice(waveStart);  
                timeToNextWave = 60;           
                currentWave++;
                
                resourceUI.setCurrentWaveText(currentWave);
                resourceUI.setTimeToNextWaveText("Attack!");
                GenerateWave();
                checkAndSetWaveIsBossWave();
                //Increase skillpoints additionfor this wave
                if(currentWave % skillPointsEveryXWave == 0){
                    skillPointsToAdd++;
                }
                if(waveIsBossWave){
                    skillPointsToAdd++;
                }
                waveDialog();             
                doOnceDuringPause = true;
            }
            else{
                timeToNextWave -= Time.fixedDeltaTime;
                resourceUI.setTimeToNextWaveText(timeToNextWave);
            }
            
        }
    }
    private IEnumerator PlayCountdown(){
        for(int i = 3; i > 0; i--){
            globalAudioSystem.PlayVoice(alarm);
            yield return new WaitForSeconds(1);
        }
        countdownPlaying = false;
    }

    private void checkAndSetWaveIsBossWave(){
        waveIsBossWave= (currentWave == goblinBossWave) || (currentWave == skeletonBossWave) || (currentWave == elvesBossWave)||(currentWave == humanBossWave);
    }
    public void waveDialog(){
        Debug.Log("Im in waveDialog!");
        if(specialDialogs.ContainsKey(currentWave)){
            Debug.Log("Im in specialsDialog!");
            iWindow.popupInformationWindow(specialDialogs[currentWave]);
        }    
        else if(waveIsBossWave){
            Debug.Log("Im in bosswaveDialog!");
            iWindow.popupInformationWindow("Bosswave!");
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

        // Play voices
        if (currentWave > startAllTogether) {
            // Skip
        } else if (currentWave == startAllTogether) {
            globalAudioSystem.PlayVoice(announcerVoice);
        } else if (currentWave == startHumans) {
            globalAudioSystem.PlayVoice(humanVoice);
        } else if (currentWave == startElves) {
            globalAudioSystem.PlayVoice(elfVoice);
        } else if (currentWave == startSkeletons) {
            globalAudioSystem.PlayVoice(undeadVoice);
        } else if (currentWave == startGoblin) {
            globalAudioSystem.PlayVoice(goblinVoice);
        } else if (currentWave == goblinBossWave) {
            globalAudioSystem.PlayVoice(goblinBossVoice);
        } else if (currentWave == skeletonBossWave) {
            globalAudioSystem.PlayVoice(undeadBossVoice);
        } else if (currentWave == elvesBossWave) {
            globalAudioSystem.PlayVoice(elfBossVoice);
        } else if (currentWave == humanBossWave) {
            globalAudioSystem.PlayVoice(humanBossVoice);
        }
        
        if(currentWave >= startAllTogether){
            randEnemyListStart = 0;
            randEnemyListEnd = enemies.Count;
            //decide if bosswave
            int bosswaveValue = Random.Range(0, 100);
            if(bosswaveValue < 10){
                int randBossId = Random.Range(0, bosses.Count);
                generatedEnemies.Add(bosses[randBossId].enemyPrefab);
                globalAudioSystem.PlayMusic(GamePhase.BossWave);
                waveIsBossWave = true;
            } else {
                globalAudioSystem.PlayMusic(GamePhase.NormalWave);
            }
        }
        else if(currentWave >= startHumans){
            randEnemyListStart = 3;
            randEnemyListEnd = 4;
            if(currentWave == humanBossWave){
                globalAudioSystem.PlayMusic(GamePhase.HumanWave);
                generatedEnemies.Add(bosses[3].enemyPrefab);
            } else {
                globalAudioSystem.PlayMusic(GamePhase.NormalWave);
            }
        }
        else if(currentWave >= startElves){
            randEnemyListStart = 2;
            randEnemyListEnd = 3;
            if(currentWave == elvesBossWave){
                globalAudioSystem.PlayMusic(GamePhase.ElfWave);
                generatedEnemies.Add(bosses[2].enemyPrefab);
            } else {
                globalAudioSystem.PlayMusic(GamePhase.NormalWave);
            }
        }
        else if(currentWave >= startSkeletons){
            randEnemyListStart = 1;
            randEnemyListEnd = 2;
            if(currentWave == skeletonBossWave){
                globalAudioSystem.PlayMusic(GamePhase.SkeletonWave);
                generatedEnemies.Add(bosses[1].enemyPrefab);
            } else {
                globalAudioSystem.PlayMusic(GamePhase.NormalWave);
            }
        }
        else if(currentWave >= startGoblin){
            randEnemyListStart = 0;
            randEnemyListEnd = 1;
            if(currentWave == goblinBossWave){
                globalAudioSystem.PlayMusic(GamePhase.GoblinWave);
                generatedEnemies.Add(bosses[0].enemyPrefab);
            } else {
                globalAudioSystem.PlayMusic(GamePhase.NormalWave);
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
    public void healAtWaveEnd(){
        player.GetComponent<CombatTarget>().SetToMaxHealth();
        poi.GetComponent<CombatTarget>().SetToMaxHealth();
        POIBuilding poi_building = bs.poi_building;
        if(poi_building != null){
            foreach(PlaceableObject po in poi_building.getPlacedBuildings()){
                po.GetComponentInParent<CombatTarget>().SetToMaxHealth();
            }
        }
    }
}


