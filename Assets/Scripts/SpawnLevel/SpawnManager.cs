using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject[] enemysMeele;
    [SerializeField] private GameObject[] enemysCheft;

    [SerializeField] private GameObject prf_T_Rex;
    [SerializeField] private GameObject prf_escupidor;
    [SerializeField] private GameObject prf_triceraptos;

    [Header("Spawn Points Level 1")]
    [SerializeField] private GameObject[] spawnPointsEnemysLv1;
    [SerializeField] private GameObject[] spawnPointsMaterialsLv1;
    [Header("Spawn Points Level 2")]
    [SerializeField] private GameObject[] spawnPointsEnemysLv2;
    [SerializeField] private GameObject[] spawnPointsMaterialsLv2;
    [Header("Spawn Points Level 3")]
    [SerializeField] private GameObject[] spawnPointsEnemysLv3;
    [SerializeField] private GameObject[] spawnPointsMaterialsLv3;

    [Header("Spawn Time contoller")]
    [SerializeField] private float spawningTime;
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
    int index,random,numEnemys;
    public string playerTag,enemyTag,enemyCheft;
  
    void Start()
    {
        basicAgent = GetComponent<BasicAgent>();
        agentStates = LevelStates.None;
        inLevel1 = false;
        inLevel2 = false;
        inLevel3 = false;
        inPause = false;
    }

    private void FixedUpdate()
    {
        col_LevelPerceibed_Nivel1 = Physics.OverlapSphere(LevelPercept[0].position, levelPreceptRadious);
        col_LevelPerceibed_Nivel2 = Physics.OverlapSphere(LevelPercept[1].position, levelPreceptRadious);
        col_LevelPerceibed_Nivel3 = Physics.OverlapSphere(LevelPercept[2].position, levelPreceptRadious);

        perceptionManager();
        decisionManager();
    }

    public void perceptionManager()
    {
        inLevel1 = false;
        inLevel2 = false;
        inLevel3 = false;
        inPause = false;
        if (col_LevelPerceibed_Nivel1.Length > 0/*col_LevelPerceibed_Nivel1[0] != null*/)
        {
            foreach (Collider tmp in col_LevelPerceibed_Nivel1)
            {
                if (tmp.CompareTag(playerTag))
                {
                    inLevel1 = true;
                    Debug.Log("Spawn1");
                }
            }
        }
   
        if (col_LevelPerceibed_Nivel2.Length > 0/*col_LevelPerceibed_Nivel2[1] != null*/)
        {
            foreach (Collider tmp in col_LevelPerceibed_Nivel2)
            {
                if (tmp.CompareTag(playerTag))
                {
                    inLevel2 = true;
                    Debug.Log("Spawn2");
                    
                }
            }
        }

        if (col_LevelPerceibed_Nivel3.Length > 0/*col_LevelPerceibed_Nivel3[2] != null*/)
        {
            foreach (Collider tmp in col_LevelPerceibed_Nivel3)
            {
                if (tmp.CompareTag(playerTag))
                {
                    inLevel3 = true;
                    Debug.Log("Spawn3");
                   
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
                SpawnLevel1();
                break;
            case LevelStates.Level_2:
                SpawnLevel2();
                break; 
            case LevelStates.Level_3:
                SpawnLevel3();
                break;           
                
        }
    }
    void SpawnLevel1()
    {
        spawningTime += Time.deltaTime;
        if (spawningTime >= spawnTimer && !inPause)
        {
            for (int i = 0; i < waveNumber; ++i)
            random = Mathf.RoundToInt(UnityEngine.Random.Range(0f, spawnPointsEnemysLv1.Length ));
            Instantiate(enemysMeele[UnityEngine.Random.Range(0, enemysMeele.Length)], spawnPointsEnemysLv1[random].transform.position+new Vector3(0,1,0), Quaternion.identity);

            spawningTime = 0;
            numEnemys ++;
        }
        else if (numEnemys == 3)
        {
            inPause = true;
            Instantiate(prf_T_Rex, spawnPointsEnemysLv1[random].transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            numEnemys++;
        }
        
    }
    void SpawnLevel2()
    {
        spawningTime += Time.deltaTime;
        if (spawningTime >= spawnTimer && !inPause)
        {
            for (int i = 0; i < waveNumber; ++i)
            {
                random = Mathf.RoundToInt(UnityEngine.Random.Range(0f, spawnPointsEnemysLv2.Length));
                Instantiate(enemysMeele[UnityEngine.Random.Range(0, enemysMeele.Length)], spawnPointsEnemysLv2[random].transform.position + new Vector3(0, 1, 0), Quaternion.identity);

            }

            spawningTime = 0;
            numEnemys++;
        }
        else if (numEnemys == 3)
        {
            inPause = true;

            Instantiate(prf_escupidor, spawnPointsEnemysLv2[random].transform.position + new Vector3(0, 1, 0), Quaternion.identity);            
            numEnemys++;
        }

    }
    void SpawnLevel3()
    {
        spawningTime += Time.deltaTime;
        if (spawningTime >= spawnTimer && !inPause)
        {
            for (int i = 0; i < waveNumber; ++i)
                random = Mathf.RoundToInt(UnityEngine.Random.Range(0f, spawnPointsEnemysLv3.Length));
            Instantiate(enemysMeele[UnityEngine.Random.Range(0, enemysMeele.Length)], spawnPointsEnemysLv3[random].transform.position + new Vector3(0, 1, 0), Quaternion.identity);

            spawningTime = 0;
            numEnemys++;
        }
        else if (numEnemys == 3)
        {
            inPause = true;
            Instantiate(prf_triceraptos, spawnPointsEnemysLv3[random].transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
    }
    
    private enum LevelStates
    {
        None,
        Level_1,
        Level_2,
        Level_3
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(LevelPercept[0].position, levelPreceptRadious);
        Gizmos.DrawWireSphere(LevelPercept[1].position, levelPreceptRadious);
        Gizmos.DrawWireSphere(LevelPercept[2].position, levelPreceptRadious);
    }
}
