using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;


public class SpawnManagerLevel : MonoBehaviour
{
    public static SpawnManagerLevel Instance { get; private set; }
    private void Awake()
    {
        // Implementación Singleton
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            ///Destroy(gameObject); // Si ya existe una instancia, destruye este objeto.
        }
    }
    [Header("Prefabs Trampas")]
    [SerializeField] private GameObject[] trampas;

    [Header("Spawn Points Level 1")]
    [SerializeField] private GameObject[] spawnPointsTrampasLv1;

    [Header("Spawn Points Level 2")]
    [SerializeField] private GameObject[] spawnPointsTrampasLv2;

    [Header("Spawn Points Level 3")]
    [SerializeField] private GameObject[] spawnPointsTrampasLv3;

    [Header("Spawn Time contoller Trampas")]
    [SerializeField] private float spawningTimeTrampas;
    [SerializeField] private float waveNumberTrampas;
    [SerializeField] private float spawnTimerTrampas;
  

    [Header("Percepcion Trampas")]
    [SerializeField] private float trampasPreceptRadious;
    [SerializeField] private float startDelayOledad_Trampas, spawnIntervalTime_Trampas;
    [SerializeField] private Transform[] TrampasPercept;

    Collider[] col_TrampasPerceibed_Nivel1;
    Collider[] col_TrampasPerceibed_Nivel2;
    Collider[] col_TrampasPerceibed_Nivel3;

    [Header("Prefabs Enemys")]
    [SerializeField] private GameObject[] enemysMeele;

    [Header("Spawn Points Level 1")]
    [SerializeField] private GameObject[] spawnPointsEnemysLv1;
    [Header("Spawn Points Level 2")]
    [SerializeField] private GameObject[] spawnPointsEnemysLv2;
    [Header("Spawn Points Level 3")]
    [SerializeField] private GameObject[] spawnPointsEnemysLv3;

    [Header("Spawn Time contoller Oleadas")]
    [SerializeField] private float maxNumberOfEnemys;
    [SerializeField] private float spawnTimer;
    [SerializeField] private float spawnTimerToRespawning;

    [SerializeField] private float spawnTimerCounter; // Temporizador para spawn
    [SerializeField] private float pauseTimerCounter; // Temporizador para pausa

    [Header("Percepcion Levels")]
    [SerializeField] private float levelPreceptRadious;
    [SerializeField] private float startDelayOledad,spawnIntervalTime;
    [SerializeField] private Transform[] LevelPercept;

    Collider[] col_LevelPerceibed_Nivel1;
    Collider[] col_LevelPerceibed_Nivel2;
    Collider[] col_LevelPerceibed_Nivel3;
    Collider[] colliders_Trampas;
    //List<GameObject> objetosDetectados = new List<GameObject>();
    BasicAgent basicAgent;

    LevelStates agentStates;
    bool inLevel1, inLevel2, inLevel3;
    bool inPause;
    int random,numEnemys;
    public bool inBattle, battle;
    public string playerTag,enemyTag,enemyCheft;
  
    void Start()
    {
       
        basicAgent = GetComponent<BasicAgent>();
        agentStates = LevelStates.None;
        inLevel1 = false;
        inLevel2 = false;
        inLevel3 = false;
        inBattle = false;

    }
   
    private void FixedUpdate()
    {
        col_LevelPerceibed_Nivel1 = Physics.OverlapSphere(LevelPercept[0].position, levelPreceptRadious);
        col_LevelPerceibed_Nivel2 = Physics.OverlapSphere(LevelPercept[1].position, levelPreceptRadious);
        col_LevelPerceibed_Nivel3 = Physics.OverlapSphere(LevelPercept[2].position, levelPreceptRadious);

        col_TrampasPerceibed_Nivel1 = Physics.OverlapSphere(TrampasPercept[0].position, trampasPreceptRadious);
        col_TrampasPerceibed_Nivel2 = Physics.OverlapSphere(TrampasPercept[1].position, trampasPreceptRadious);
        col_TrampasPerceibed_Nivel3 = Physics.OverlapSphere(TrampasPercept[2].position, trampasPreceptRadious);

        perceptionManager_Level(); 
        decisionManager();
    }

    public void perceptionManager_Level()
    {
        inLevel1 = false;
        inLevel2 = false;
        inLevel3 = false;

        if (col_LevelPerceibed_Nivel1.Length > 0 /*col_LevelPerceibed_Nivel1[0] != null*/)
        {
            foreach (Collider tmp in col_LevelPerceibed_Nivel1)
            {
                if (tmp.CompareTag(playerTag))
                {
                    inLevel1 = true;
                    //Debug.Log("Spawn1");
                }
                if (tmp.CompareTag(enemyTag))
                {
                    //Debug.Log(enemyTag);
                    inBattle = true;
                    GetinBlattle(inBattle);
                }
                else
                {
                    //Debug.Log("false");
                    inBattle = false;
                   
                }
            }
        }
   
        if (col_LevelPerceibed_Nivel2.Length > 0 /*col_LevelPerceibed_Nivel2[1] != null*/)
        {
            foreach (Collider tmp in col_LevelPerceibed_Nivel2)
            {
                if (tmp.CompareTag(playerTag))
                {
                    inLevel2 = true;
                    ////Debug.Log("Spawn2");
                    
                }
                if (tmp.CompareTag(enemyTag))
                {
                    inBattle = true;
                    GetinBlattle(inBattle);
                }
                else
                {
                    inBattle = false;
                    
                }
            }
        }

        if (col_LevelPerceibed_Nivel3.Length > 0 /*col_LevelPerceibed_Nivel3[2] != null*/)
        {
            foreach (Collider tmp in col_LevelPerceibed_Nivel3)
            {
                if (tmp.CompareTag(playerTag))
                {
                    inLevel3 = true;
                    Debug.Log("Spawn3");
                   
                }
                if (tmp.CompareTag(enemyTag))
                {
                    inBattle = true;
                    GetinBlattle(inBattle);
                }
                else
                {
                    inBattle = false;
                    GetinBlattle(inBattle);
                }
            }
        }
    }
  
    void decisionManager()
    {
        if (inLevel1)
        {
            agentStates = LevelStates.Level_1;
        }
        else if(inLevel2)
        {
            agentStates = LevelStates.Level_2;
        }
        else if (inLevel3)
        {
            agentStates = LevelStates.Level_3;
        }
        else
        {
            agentStates = LevelStates.None;
        }
        actionManager();
    }
    void actionManager()
    {
        switch (agentStates)
        {
            case LevelStates.None:
                break;
            case LevelStates.Level_1:
                SpawnLevel1Trampas();
                ////SpawnLevel1Enemys();
                break;
            case LevelStates.Level_2:
               /// SpawnLevel2Enemys();
                SpawnLevel2Trampas();
                break;
            case LevelStates.Level_3:
               // SpawnLevel3Enemys();
                SpawnLevel3Trampas();
                break;

        }
    }
    void SpawnLevel1Enemys()
    {
        if (inPause)
        {
            pauseTimerCounter += Time.deltaTime;

            if (pauseTimerCounter >= spawnTimerToRespawning)
            {
                numEnemys = 0;
                pauseTimerCounter = 0; // Reinicia el temporizador de pausa
                inPause = false;
            }
            return; 
        }

        spawnTimerCounter += Time.deltaTime;

        if (numEnemys >= maxNumberOfEnemys)
        {
            //Debug.Log("sedetiiene");
            inPause = true;
           
            return;
        }

        if (spawnTimerCounter >= spawnTimer)
        {
            //Debug.Log("apareciendo");

            int randomSpawnIndex = UnityEngine.Random.Range(0, spawnPointsEnemysLv1.Length);
            int randomEnemyIndex = UnityEngine.Random.Range(0, enemysMeele.Length);

            Instantiate(
                enemysMeele[randomEnemyIndex],
                spawnPointsEnemysLv1[randomSpawnIndex].transform.position + new Vector3(0, 1, 0),
                Quaternion.identity
            );

            spawnTimerCounter = 0;
            numEnemys++;
        }
    }
    void SpawnLevel1Trampas()
    {
        spawningTimeTrampas += Time.deltaTime;
        //Debug.Log(spawningTime+"Level1");
        if (spawningTimeTrampas >= spawnTimerTrampas && !inPause)
        {
            bool pointFound = false;

            // SPAWN VACIOS
            for (int attempt = 0; attempt < spawnPointsTrampasLv1.Length; ++attempt)
            {
                random = UnityEngine.Random.Range(0, spawnPointsTrampasLv1.Length);

                // Comprobar si el punto de spawn está vacío 
                colliders_Trampas = Physics.OverlapSphere(spawnPointsTrampasLv1[random].transform.position, 1.0f);

                if (colliders_Trampas.Length == 0) // Si el punto está vacío
                {
                    Instantiate(trampas[UnityEngine.Random.Range(0, trampas.Length)],
                         spawnPointsTrampasLv1[random].transform.position + new Vector3(0, 1, 0),
                         Quaternion.identity);
                    pointFound = true;
                    break;
                }
                if (!pointFound)
                {
                    return;
                }
            }
        }
        if (spawningTimeTrampas >= 10)
        {
            spawningTimeTrampas = 0;
            inPause = false;
        }
    }

    void SpawnLevel2Enemys()
    {
        if (inPause)
        {
            pauseTimerCounter += Time.deltaTime;

            if (pauseTimerCounter >= spawnTimerToRespawning)
            {
                numEnemys = 0;
                pauseTimerCounter = 0; // Reinicia el temporizador de pausa
                inPause = false;
            }
            return;
        }

        spawnTimerCounter += Time.deltaTime;

        if (numEnemys >= maxNumberOfEnemys)
        {
            Debug.Log("sedetiiene");
            inPause = true;

            return;
        }

        if (spawnTimerCounter >= spawnTimer)
        {
            Debug.Log("apareciendo");

            int randomSpawnIndex = UnityEngine.Random.Range(0, spawnPointsEnemysLv2.Length);
            int randomEnemyIndex = UnityEngine.Random.Range(0, enemysMeele.Length);

            Instantiate(
                enemysMeele[randomEnemyIndex],
                spawnPointsEnemysLv2[randomSpawnIndex].transform.position + new Vector3(0, 1, 0),
                Quaternion.identity
            );

            spawnTimerCounter = 0;
            numEnemys++;
        }

    }
    void SpawnLevel2Trampas()
    {
        spawningTimeTrampas += Time.deltaTime;
        //Debug.Log(spawningTime+"Level1");
        if (spawningTimeTrampas >= spawnTimerTrampas && !inPause)
        {
            bool pointFound = false;

            // SPAWN VACIOS
            for (int attempt = 0; attempt < spawnPointsTrampasLv2.Length; ++attempt)
            {


                random = UnityEngine.Random.Range(0, spawnPointsTrampasLv2.Length);

                // Comprobar si el punto de spawn está vacío 
                colliders_Trampas = Physics.OverlapSphere(spawnPointsTrampasLv2[random].transform.position, 1.0f);

                if (colliders_Trampas.Length == 0) // Si el punto está vacío
                {
                   Instantiate(trampas[UnityEngine.Random.Range(0, trampas.Length)],
                        spawnPointsTrampasLv2[random].transform.position + new Vector3(0, 1, 0),
                        Quaternion.identity);
                    pointFound = true;
                    break;
                }
                if (!pointFound)
                {
                    return;
                }
            }
        }
        if (spawningTimeTrampas >= 10)
        {
            spawningTimeTrampas = 0;
            inPause = false;
        }
    }
    public bool GetinBlattle(bool name)
    {
        battle = name;
        return battle;
    }

    public bool SetinBlattle()
    {
        //Debug.Log(listTorres);
        return battle;
    }
    void SpawnLevel3Enemys()
    {
        if (inPause)
        {
            pauseTimerCounter += Time.deltaTime;

            if (pauseTimerCounter >= spawnTimerToRespawning)
            {
                numEnemys = 0;
                pauseTimerCounter = 0; // Reinicia el temporizador de pausa
                inPause = false;
            }
            return;
        }

        spawnTimerCounter += Time.deltaTime;

        if (numEnemys >= maxNumberOfEnemys)
        {
            Debug.Log("sedetiiene");
            inPause = true;

            return;
        }

        if (spawnTimerCounter >= spawnTimer)
        {
            Debug.Log("apareciendo");

            int randomSpawnIndex = UnityEngine.Random.Range(0, spawnPointsEnemysLv3.Length);
            int randomEnemyIndex = UnityEngine.Random.Range(0, enemysMeele.Length);

            Instantiate(
                enemysMeele[randomEnemyIndex],
                spawnPointsEnemysLv3[randomSpawnIndex].transform.position + new Vector3(0, 1, 0),
                Quaternion.identity
            );

            spawnTimerCounter = 0;
            numEnemys++;
        }
    }
    void SpawnLevel3Trampas()
    {
        spawningTimeTrampas += Time.deltaTime;
        //Debug.Log(spawningTime+"Level1");
        if (spawningTimeTrampas >= spawnTimerTrampas && !inPause)
        {
            bool pointFound = false;

            // SPAWN VACIOS
            for (int attempt = 0; attempt < spawnPointsTrampasLv3.Length; ++attempt)
            {


                random = UnityEngine.Random.Range(0, spawnPointsTrampasLv3.Length);

                // Comprobar si el punto de spawn está vacío 
                Collider[] colliders = Physics.OverlapSphere(spawnPointsTrampasLv1[random].transform.position, 1.0f);

                if (colliders.Length == 0) // Si el punto está vacío
                {
                    Instantiate(trampas[UnityEngine.Random.Range(0, trampas.Length)],
                        spawnPointsTrampasLv3[random].transform.position + new Vector3(0, 1, 0),
                        Quaternion.identity);
                    pointFound = true;
                    break;
                }
                if (!pointFound)
                {
                    return;
                }
            }
        }
        if (spawningTimeTrampas >= 10)
        {
            spawningTimeTrampas = 0;
            inPause = false;
        }

    }

    private enum LevelStates
    {
        None,
        Level_1,
        Level_2,
        Level_3,
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(LevelPercept[0].position, levelPreceptRadious);
        Gizmos.DrawWireSphere(LevelPercept[1].position, levelPreceptRadious);
        Gizmos.DrawWireSphere(LevelPercept[2].position, levelPreceptRadious);

        Gizmos.DrawWireSphere(TrampasPercept[0].position, trampasPreceptRadious);
        Gizmos.DrawWireSphere(TrampasPercept[1].position, trampasPreceptRadious);
        Gizmos.DrawWireSphere(TrampasPercept[2].position, trampasPreceptRadious);
        Gizmos.DrawWireSphere(TrampasPercept[3].position, trampasPreceptRadious);
        Gizmos.DrawWireSphere(TrampasPercept[4].position, trampasPreceptRadious);
        Gizmos.DrawWireSphere(TrampasPercept[5].position, trampasPreceptRadious);
        Gizmos.DrawWireSphere(TrampasPercept[6].position, trampasPreceptRadious);
        Gizmos.DrawWireSphere(TrampasPercept[7].position, trampasPreceptRadious);
        Gizmos.DrawWireSphere(TrampasPercept[8].position, trampasPreceptRadious);
    }
}
