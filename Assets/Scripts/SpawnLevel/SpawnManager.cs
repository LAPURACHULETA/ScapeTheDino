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
    [SerializeField] private LayerMask whatIsPlayer;

    Collider[] col_LevelPerceibed;
   

    BasicAgent basicAgent;

    ShooterAgentStates agentStates;
    float timerBullet, timerEvade;
    bool inLevel1, inLevel2, inLevel3;
    int index,random,numEnemys;
    int numEnemyInOleda = 5;
    public string playerTag;

    void Start()
    {
        basicAgent = GetComponent<BasicAgent>();
        agentStates = ShooterAgentStates.None;
        inLevel1 = false;
        inLevel2 = false;
        inLevel3 = false;
    }

    /// <summary>
    /// Realiza operaciones de f?sica y l?gica de juego a una velocidad fija.
    /// </summary>
    private void FixedUpdate()
    {
        col_LevelPerceibed = Physics.OverlapSphere(LevelPercept[0].position, LevelPreceptRadious, whatIsPlayer);
        if (col_LevelPerceibed == null)
        {
            col_LevelPerceibed = Physics.OverlapSphere(LevelPercept[1].position, LevelPreceptRadious, whatIsPlayer);
        }
        
        if (col_LevelPerceibed == null)
        {
            col_LevelPerceibed = Physics.OverlapSphere(LevelPercept[2].position, LevelPreceptRadious, whatIsPlayer);
        }
        
        if (col_LevelPerceibed == null)
        {
            return;
        }
        perceptionManager();
        decisionManager();
    }

    /// <summary>
    /// Gestiona la percepci?n del agente y establece el objetivo.
    /// </summary>
    public void perceptionManager()
    {
        if (basicAgent)
        {
            basicAgent.targetPlayer = null;
            inLevel1 = false;
            inLevel2 = false;
            inLevel3 = false;
        }

        if (col_LevelPerceibed != null)
        {  
            if (true )///comparar el tag para detectar player
            {
                //basicAgent.targetPlayer = tmp.transform;
                inLevel1 = true;
                Debug.Log("Spawn1");
            }
        }
      
        //if (col_LevelPerceibed[1] != null)
        //{
        //    foreach (Collider tmp in col_LevelPerceibed)
        //    {
        //        if (tmp.CompareTag(playerTag))
        //        {
        //            //basicAgent.targetPlayer = tmp.transform;
        //            inLevel1 = true;
        //            Debug.Log("Spawn2");
        //        }
        //    }
        //}
     
        //if (col_LevelPerceibed[2] != null)
        //{
        //    foreach (Collider tmp in col_LevelPerceibed)
        //    {
        //        if (tmp.CompareTag(playerTag))
        //        {
        //            //basicAgent.targetPlayer = tmp.transform;
        //            inLevel1 = true;
        //            Debug.Log("Spawn3");
        //        }
        //    }
        //}
    }
    void decisionManager()
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
    }
    void actionManager()
    {
        switch (agentStates)
        {
            case ShooterAgentStates.Level_1:
                Debug.Log("Default");
                SpawnLevel1();
                break;
            case ShooterAgentStates.Level_2:              
                break; 
            case ShooterAgentStates.Level_3:
                break;
            //default:
                
        }
    }

    void SpawnLevel1()
    {
       //InvokeRepeating("Spawn", startDelayOledad, spawnIntervalTime);
        Invoke("Spawn", spawnIntervalTime);
        agentStates = ShooterAgentStates.Level_2;
    }
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
            int random = Mathf.RoundToInt(UnityEngine.Random.Range(0, spawnPointsEnemys.Length-1));

            Debug.Log(spawnPointsEnemys[random].name);
            UnityEditor.EditorGUIUtility.PingObject(enemysMeele[random]);
            Instantiate(enemysMeele[random], spawnPointsEnemys[random].transform.position, Quaternion.identity);

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
        None,
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
