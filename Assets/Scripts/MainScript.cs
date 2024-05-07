using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum gameMode { playing,gameOver}

public class MainScript : MonoBehaviour
{
    static private MainScript S;
    static private Dictionary<eWeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Inscribed")]
    public bool spawnEnemies = true;
    public GameObject[] prefabEnemies;
    public WeaponDefinition[] weaponDefinitions;
    public float enemySpawnPerSec = 0.5f;
    public float enemyInsetDefault = 1.5f;
    public float gameRestartDelay = 2f;
    public float planetInset = 2.0f;
    public eWeaponType[] powerUpFreq = new eWeaponType[] {eWeaponType.blaster,eWeaponType.blaster,
                                          eWeaponType.spread,eWeaponType.shield};

    public GameObject prefabPowerUp;
    public GameObject ShipGO;
    public GameObject AllyPrefab;

    public List<Texture2D> PlanetTextures;
    public GameObject Planet;
    int numOfTextures = 0;
  

    [Header("Dynamic")]
    [SerializeField] private int kills = 0;
    public static bool fade = false;
    public int level = 0;
    private BoundSheckScript bndCheck;
    ShipScript shipScript;
    TimerScript timerScript;
    UIScript uiScript;
    public gameMode mode;
    private Vector3 AllyPosLeft = new Vector3(-13.7f, -31.23f, 0);
    private Vector3 AllyPosRight = new Vector3(13.7f, -31.23f, 0);
    GameObject goAllyLeft;
    GameObject goAllyRight;
    private float timeToRespawnAlly = 4.0f;
    AllyPos ally = AllyPos.none;
    float timeL = 0;
    float timeR = 0;
    float planetCreationTime = 0;
    // Start is called before the first frame update
    void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundSheckScript>();
        shipScript = ShipGO.GetComponent<ShipScript>();
        timerScript = gameObject.GetComponent<TimerScript>();
        uiScript = gameObject.GetComponent<UIScript>();
       
        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSec);

        WEAP_DICT = new Dictionary<eWeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }      
    }
    
    private void Update()
    {
        uiScript.UpdateKills(kills);
        uiScript.UpdateShieldLevel(shipScript.shieldLevel);
        if(timerScript.levelTimer <= 0 && mode == gameMode.playing)
        {
            timerScript.resetTimer();
            level++;
            uiScript.UpdateLevel(level);
            fade = true;
            
            if (enemySpawnPerSec >= 2.5f)
            {
                enemySpawnPerSec = 2.5f;
            }
            else enemySpawnPerSec += 0.1f;

        }

        if (fade)
        {
            uiScript.LevelUpfade();
        }



        //Ally Respawn
        if (goAllyLeft == null && (timeL + timeToRespawnAlly) < Time.time ) {
            LeftRespawn();
        }

        if (goAllyRight == null && (timeR + timeToRespawnAlly) < Time.time) {
            RightRespawn();
        }
        
        // Planet Creation
        if(Time.time > (planetCreationTime + 10f))
        {
            SpawnPlanet();
        }
    }


    public void SpawnEnemy()
    {
        if (!spawnEnemies)
        {
            Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSec);
            return;
        }

        int index = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[index]);
        float enemyInset = enemyInsetDefault;
        if(go.GetComponent<BoundSheckScript>() != null)
        {
            enemyInset = Mathf.Abs(go.GetComponent<BoundSheckScript>().radius);
        }
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyInset;
        float xMax = bndCheck.camWidth - enemyInset;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyInset;
        go.transform.position = pos;

        mode = gameMode.playing;
        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSec);
    }
    public void DelayRestart()
    {
        Invoke(nameof(Restart), gameRestartDelay);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    void Restart()
    {
        SceneManager.LoadScene("__Scene_0");
        
    }
    static public void HeroDied()
    {
        S.mode = gameMode.gameOver;
        S.uiScript.DisplayGameOver();
        S.timerScript.stopTimer();
        //S.DelayRestart();

    }

    static public WeaponDefinition GET_WEAPON_DEFINITION(eWeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt))
        {
            return WEAP_DICT[wt];
        }
        return (new WeaponDefinition());
    } 

    static public void SHIP_DESTROYED(EnemyScript e)
    {
        if (Random.value <= e.powerUpDropChance)
        {
            int ndx = Random.Range(0, S.powerUpFreq.Length);
            eWeaponType pUpType = S.powerUpFreq[ndx];
            GameObject go = Instantiate<GameObject>(S.prefabPowerUp);
            PowerUpScript pUp = go.GetComponent<PowerUpScript>();
            pUp.SetType(pUpType);
            pUp.transform.position = e.transform.position;
        }
        S.kills++;
    }

    GameObject RespawnAlly(Vector3 pos,AllyPos po)
    {

        AllyPrefab.SetActive(false);
        GameObject go = Instantiate(AllyPrefab);
        go.GetComponent<AllyScript>().allyPos = po;
            //allySc.allyPos = po;
        go.SetActive(true);
        go.transform.position = pos;
        return go;

    }

    void LeftRespawn()
    {
        goAllyLeft = RespawnAlly(AllyPosLeft, AllyPos.left);
        goAllyLeft.GetComponent<AllyScript>().AllyScDestroyedEvent += LeftAllyDestroyed;
    }
    void RightRespawn()
    {
        goAllyRight = RespawnAlly(AllyPosRight, AllyPos.right);
        goAllyRight.GetComponent<AllyScript>().AllyScDestroyedEvent += RightAllyDestroyed;
    }
    void LeftAllyDestroyed()
    {
        timeL = Time.time;
    }
    void RightAllyDestroyed()
    {
        timeR = Time.time;
    }
    

    void SpawnPlanet()
    {
        if (numOfTextures >= PlanetTextures.Count) numOfTextures = 0;
        GameObject go_pl = Instantiate(Planet);
        float y =  bndCheck.camHeight + planetInset;
        float z = 8.20f;
        float Xmin = -bndCheck.camWidth + planetInset;
        float Xmax = bndCheck.camWidth - planetInset;

        float x = Random.Range(Xmin, Xmax);

        go_pl.GetComponent<Renderer>().material.mainTexture = PlanetTextures[numOfTextures];
        go_pl.transform.position = new Vector3(x, y, z);
        planetCreationTime = Time.time;
        numOfTextures++;
    }

}
