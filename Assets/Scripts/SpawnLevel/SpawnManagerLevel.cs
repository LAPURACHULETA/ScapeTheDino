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

    [SerializeField] private GameObject prf_Pendulum;
    [SerializeField] private GameObject prf_Barbs;
    [SerializeField] private GameObject prf_Bomb;
    [SerializeField] private GameObject prf_Molotov;

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
    [SerializeField] private GameObject[] enemysCheft;

    [SerializeField] private GameObject prf_T_Rex;
    [SerializeField] private GameObject prf_escupidor;
    [SerializeField] private GameObject prf_triceraptos;

    [Header("Spawn Points Level 1")]
    [SerializeField] private GameObject[] spawnPointsEnemysLv1;
    [Header("Spawn Points Level 2")]
    [SerializeField] private GameObject[] spawnPointsEnemysLv2;
    [Header("Spawn Points Level 3")]
    [SerializeField] private GameObject[] spawnPointsEnemysLv3;

    [Header("Spawn Time contoller Oleadas")]
    /*[SerializeField]*/ private float spawningTime;
    [SerializeField] private float waveNumber;
    [SerializeField] private float spawnTimer;

    [Header("Percepcion Levels")]
    [SerializeField] private float levelPreceptRadious;
    [SerializeField] private float startDelayOledad,spawnIntervalTime;
    [SerializeField] private Transform[] LevelPercept;

    Collider[] col_LevelPerceibed_Nivel1;
    Collider[] col_LevelPerceibed_Nivel2;
    Collider[] col_LevelPerceibed_Nivel3;
   
    BasicAgent basicAgent;

    LevelStates agentStates;
    bool inLevel1, inLevel2, inLevel3;
    bool inPause;
    bool pointFound = false;
    int index,random,numEnemys;
    public bool inBattle;
    public string playerTag,enemyTag,enemyCheft;
  
    void Start()
    {
       
        basicAgent = GetComponent<BasicAgent>();
        agentStates = LevelStates.None;
        inLevel1 = false;
        inLevel2 = false;
        inLevel3 = false;
        inPause = false;
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
                    Debug.Log("Spawn1");
                }
                if (tmp.CompareTag(enemyTag))
                {
                    inBattle = true;
                }
                else
                {
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
                    Debug.Log("Spawn2");
                    
                }
                if (tmp.CompareTag(enemyTag))
                {
                    inBattle = true;
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
                }
                else
                {
                    inBattle = false;
                }
            }
        }
    }
    public bool GetInBattle(Collider name)
    {
        inBattle = name;
        Debug.Log(inBattle);
        return inBattle;
    }
    public bool SetInBattle()
    {
        Debug.Log(inBattle);
        return inBattle;

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
                SpawnLevel1Enemys();
                break;
            case LevelStates.Level_2:
                //SpawnLevel2Enemys();
                //SpawnLevel2Trampas();
                break;
            case LevelStates.Level_3:
                //SpawnLevel3Enemys();
                //SpawnLevel3Trampas();
                break;           
                
        }
    }
    void SpawnLevel1Enemys()
    {
        spawningTime += Time.deltaTime;
        if (numEnemys > 3)
        {
            spawningTime = 0;
            return;
        }
        //Debug.Log(spawningTime+"Level1");
        if (spawningTime >= spawnTimer && !inPause)
        {
            for (int i = 0; i < waveNumber; ++i)
            {
                random = Mathf.RoundToInt(UnityEngine.Random.Range(0f, spawnPointsEnemysLv1.Length ));
            }
            Instantiate(enemysMeele[UnityEngine.Random.Range(0, enemysMeele.Length)], spawnPointsEnemysLv1[random].transform.position+new Vector3(0,1,0), Quaternion.identity);

            spawningTime = 0;
            numEnemys ++;
        }
        if (numEnemys == 3)
        {
            inPause = true;
        }
        
        if (spawningTime >= 10)
        {
            numEnemys = 0;
            spawningTime = 0;
            inPause = false;
        }
    }
    void SpawnLevel1Trampas()
    {
        spawningTime += Time.deltaTime;
        //Debug.Log(spawningTime+"Level1");
        if (spawningTime >= spawnTimer && !inPause)
        {
            bool pointFound = false;

            // SPAWN VACIOS
            for (int attempt = 0; attempt < spawnPointsTrampasLv1.Length; ++attempt)
            {
                

                random = UnityEngine.Random.Range(0, spawnPointsTrampasLv1.Length);

                // Comprobar si el punto de spawn está vacío 
                Collider[] colliders = Physics.OverlapSphere(spawnPointsTrampasLv1[random].transform.position, 1.0f);

                if (colliders.Length == 0) // Si el punto está vacío
                {
                    Instantiate(trampas[UnityEngine.Random.Range(0, trampas.Length)], spawnPointsTrampasLv1[random].transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    pointFound = true;
                    break; 
                }
                if (!pointFound)
                {
                    return;
                }
            }
        }
        if (spawningTime >= 10)
        {
            numEnemys = 0;
            spawningTime = 0;
            inPause = false;
        }

    }
    void SpawnLevel2Enemys()
    {
        spawningTime += Time.deltaTime;
        //Debug.Log(spawningTime + "Level2");
        if (spawningTime >= spawnTimer && !inPause)
        {
            for (int i = 0; i < waveNumber; ++i)
            {
                random = Mathf.RoundToInt(UnityEngine.Random.Range(0f, spawnPointsEnemysLv2.Length));

            }
            Instantiate(enemysMeele[UnityEngine.Random.Range(0, enemysMeele.Length)], spawnPointsEnemysLv2[random].transform.position + new Vector3(0, 1, 0), Quaternion.identity);

            spawningTime = 0;
            numEnemys++;
        }
        if (numEnemys == 3)
        {
            inPause = true;
        }
        if (spawningTime >= 10)
        {
            numEnemys = 0;
            spawningTime = 0;
            inPause = false;
        }

    }
    void SpawnLevel2Trampas()
    {
        spawningTime += Time.deltaTime;
        //Debug.Log(spawningTime+"Level1");
        if (spawningTime >= spawnTimer && !inPause)
        {
            bool pointFound = false;

            // Intentamos buscar un punto vacío
            for (int attempt = 0; attempt < spawnPointsTrampasLv2.Length; ++attempt)
            {
                // Usamos la versión correcta de Random.Range para índices enteros
                random = UnityEngine.Random.Range(0, spawnPointsTrampasLv2.Length);

                // Comprobar si el punto de spawn está vacío (sin objetos en un radio pequeño)
                Collider[] colliders = Physics.OverlapSphere(spawnPointsTrampasLv2[random].transform.position, 1.0f);

                if (colliders.Length == 0) // Si no hay colisionadores en el área, el punto está vacío
                {
                    // Instanciamos el objeto en el punto vacío
                    Instantiate(trampas[UnityEngine.Random.Range(0, trampas.Length)], spawnPointsTrampasLv2[random].transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    pointFound = true;
                    break; // Salimos del bucle si encontramos un punto vacío
                }

                if (!pointFound)
                {
                    Debug.Log("no hay");
                }
            }
        }
        if (spawningTime >= 10)
        {
            numEnemys = 0;
            spawningTime = 0;
            inPause = false;
        }
    }
    void SpawnLevel3Enemys()
    {
        spawningTime += Time.deltaTime;
        //Debug.Log(spawningTime + "Level3");
        if (spawningTime >= spawnTimer && !inPause)
        {
            for (int i = 0; i < waveNumber; ++i)
            {
                random = Mathf.RoundToInt(UnityEngine.Random.Range(0f, spawnPointsEnemysLv3.Length));

            }
            Instantiate(enemysMeele[UnityEngine.Random.Range(0, enemysMeele.Length)], spawnPointsEnemysLv3[random].transform.position + new Vector3(0, 1, 0), Quaternion.identity);

            spawningTime = 0;
            numEnemys++;
        }
        if (numEnemys == 3)
        {
            inPause = true;
        }
        if (spawningTime >= 10)
        {
            numEnemys = 0;
            spawningTime = 0;
            inPause = false;
        }
    }
    void SpawnLevel3Trampas()
    {
        spawningTime += Time.deltaTime;
        //Debug.Log(spawningTime+"Level1");
        if (spawningTime >= spawnTimer && !inPause)
        {
            bool pointFound = false;

            // Intentamos buscar un punto vacío
            for (int attempt = 0; attempt < spawnPointsTrampasLv3.Length; ++attempt)
            {
                // Usamos la versión correcta de Random.Range para índices enteros
                random = UnityEngine.Random.Range(0, spawnPointsTrampasLv3.Length);

                // Comprobar si el punto de spawn está vacío (sin objetos en un radio pequeño)
                Collider[] colliders = Physics.OverlapSphere(spawnPointsTrampasLv3[random].transform.position, 1.0f);

                if (colliders.Length == 0) // Si no hay colisionadores en el área, el punto está vacío
                {
                    // Instanciamos el objeto en el punto vacío
                    Instantiate(trampas[UnityEngine.Random.Range(0, trampas.Length)], spawnPointsTrampasLv3[random].transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    pointFound = true;
                    break; // Salimos del bucle si encontramos un punto vacío
                }

                if (!pointFound)
                {
                    Debug.Log("no hay");
                }
            }
        }
        if (spawningTime >= 10)
        {
            numEnemys = 0;
            spawningTime = 0;
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
