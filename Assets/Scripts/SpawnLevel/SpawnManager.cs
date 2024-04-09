using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemysMeele;
    [SerializeField] private GameObject[] enemysCheft;

    [SerializeField] private GameObject[] spawnPointsEnemys;
    [SerializeField] private GameObject[] spawnPointsMaterials;

    [SerializeField] private GameObject t_Rex;
    [SerializeField] private GameObject escupidor;
    [SerializeField] private GameObject triceraptos;

    [SerializeField] private float LevelPreceptRadious;
    [SerializeField] private float startDelayOledad,spawnIntervalTime;
    [SerializeField] private Transform[] LevelPercept;

    Collider[] col_LevelPerceibed;
    BasicAgent basicAgent;

    ShooterAgentStates agentStates;
    float timerBullet, timerEvade;
    bool inLevel1, inLevel2, inLevel3;
    int index,random,numEnemys,numEnemyInOleda;
    public string playerTag;
    void Start()
    {
        basicAgent = GetComponent<BasicAgent>();
        agentStates = ShooterAgentStates.Level_1;
        inLevel1 = false;
        inLevel2 = false;
        inLevel3 = false;
    }

    /// <summary>
    /// Realiza operaciones de f?sica y l?gica de juego a una velocidad fija.
    /// </summary>
    private void FixedUpdate()
    {
        col_LevelPerceibed = Physics.OverlapSphere(LevelPercept[0].position, LevelPreceptRadious);
        col_LevelPerceibed = Physics.OverlapSphere(LevelPercept[1].position, LevelPreceptRadious);
        col_LevelPerceibed = Physics.OverlapSphere(LevelPercept[3].position, LevelPreceptRadious);

        perceptionManager();
        decisionManager_EnemyTower();
    }

    /// <summary>
    /// Gestiona la percepci?n del agente y establece el objetivo.
    /// </summary>
    public void perceptionManager()
    {
        basicAgent.targetPlayer = null;
        inLevel1 = false;
        inLevel2 = false;
        inLevel3 = false;

        if (col_LevelPerceibed[0] != null)
        {
            foreach (Collider tmp in col_LevelPerceibed)
            {
                if (tmp.CompareTag(playerTag))
                {
                    basicAgent.targetPlayer = tmp.transform;
                    inLevel1 = true;
                }
            }
        }
        if (col_LevelPerceibed[1] != null)
        {
            foreach (Collider tmp in col_LevelPerceibed)
            {
                if (tmp.CompareTag(playerTag))
                {
                    basicAgent.targetPlayer = tmp.transform;
                    inLevel1 = true;
                }
            }
        }
        if (col_LevelPerceibed[2] != null)
        {
            foreach (Collider tmp in col_LevelPerceibed)
            {
                if (tmp.CompareTag(playerTag))
                {
                    basicAgent.targetPlayer = tmp.transform;
                    inLevel1 = true;
                }
            }
        }
    }
    void decisionManager_EnemyTower()
    {
        if (inLevel1)
        {
            agentStates = ShooterAgentStates.Level_1;
        }
        else if(inLevel2)
        {
            agentStates = ShooterAgentStates.Level_2;
        }
        else if (inLevel3)
        {
            agentStates = ShooterAgentStates.Level_3;
        }
        actionManager();
        movementManager();

    }
    void actionManager()
    {
        switch (agentStates)
        {
            case ShooterAgentStates.Level_1:
                break;
            case ShooterAgentStates.Level_2:              
                break; 
            case ShooterAgentStates.Level_3:
                break;

        }
    }
    void movementManager()
    {
        switch (agentStates)
        {
            case ShooterAgentStates.Level_1:
                InvokeRepeating("Spawn", startDelayOledad, spawnIntervalTime);
                break;
            case ShooterAgentStates.Level_2:
                break;
            case ShooterAgentStates.Level_3:
                break;

        }
    }
    //void SpawnLevel1()
    //{
    //    index = Random.Range(0, enemysMeele.Length);
    //    // instantiate ball at random spawn location
    //    Instantiate(spawnPointsEnemys[index], Random.Range(spawnPointsEnemys, spawnPointsEnemys), spawnPointsEnemys[index].transform.rotation);
    //}
    void SpawnLevel2()
    {


    }
    void SpawnLevel3()
    {

    }
    private void SpawnNewEnemy(int enemigosGenerados)
    {

        for (int i = 0; i < enemigosGenerados; i++)
        {
            //random = Mathf.RoundToInt(Random.Range(0f, spawnPointsEnemys.Length));
            Instantiate(spawnPointsEnemys[random], spawnPointsEnemys[random].transform.position, Quaternion.identity);
        }

    }
    //Spawn Level 1
    private void Spawn()
    {
        SpawnNewEnemy(numEnemyInOleda);
    }
    private void EnemigosActuales()
    {
        numEnemys = FindObjectsOfType<IAMeele>().Length;
        if (numEnemys == 0)
        {
            numEnemyInOleda++;
            Spawn();
        }
    }
    private enum ShooterAgentStates
    {
        Level_1,
        Level_2,
        Level_3
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(LevelPercept[0].position, LevelPreceptRadious);
        Gizmos.DrawWireSphere(LevelPercept[1].position, LevelPreceptRadious);
        Gizmos.DrawWireSphere(LevelPercept[2].position, LevelPreceptRadious);
    }
}
