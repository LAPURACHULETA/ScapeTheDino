using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAEscupidor : MonoBehaviour
{
    [SerializeField] private float eyesPerceptRadious, earsPerceptRadious, damageHeal;
    [SerializeField] private Transform eyesPercept, earsPercept;
    [SerializeField] private float explosionDistance = 5f; // Definir la distancia para detonar la explosión
    Rigidbody rb;
    Collider[] col_eyesPerceibed, col_earspPerceibed;
    BasicAgent basicAgent;
    /// <summary>
    /// ComingMisil comingMisil;
    /// </summary>

    [SerializeField] GameObject prf_misil, prf_bullet, spawnPointMisil, spawnPointBullet;
    ShooterAgentStates agentStates;

    float timerBullet, timerMisil;
    public string enemyTag, enemyLeaderTag;
    void Start()
    {
        basicAgent = GetComponent<BasicAgent>();
        rb = GetComponent<Rigidbody>();
        agentStates = ShooterAgentStates.None;

    }

    /// <summary>
    /// Realiza operaciones de f?sica y l?gica de juego a una velocidad fija.
    /// </summary>
    private void FixedUpdate()
    {
        col_eyesPerceibed = Physics.OverlapSphere(eyesPercept.position, eyesPerceptRadious);
        col_earspPerceibed = Physics.OverlapSphere(earsPercept.position, earsPerceptRadious);

        perceptionManager();
        decisionManager_EnemyTower();
    }

    /// <summary>
    /// Gestiona la percepci?n del agente y establece el objetivo.
    /// </summary>
    public void perceptionManager()
    {
        basicAgent.targetPlayer = null;
       /// basicAgent.targetLeader = null;

        if (col_eyesPerceibed != null)
        {
            foreach (Collider tmp in col_eyesPerceibed)
            {
                if (tmp.CompareTag(enemyTag))
                {
                    basicAgent.targetPlayer = tmp.transform;

                }
            }
        }

        if (col_earspPerceibed != null)
        {
            foreach (Collider tmp in col_earspPerceibed)
            {
                if (tmp.CompareTag(enemyTag))
                {
                    basicAgent.targetPlayer = tmp.transform;

                }
            }
        }
    }
    void decisionManager_EnemyTower()
    {

        if (basicAgent.targetPlayer != null)
        {
            agentStates = ShooterAgentStates.ShootTower;
        }
      
        actionManager();

        movementManager();

    }
    void actionManager()
    {
        switch (agentStates)
        {
            case ShooterAgentStates.None:
                break;
            case ShooterAgentStates.ShootTower:
                break;
        }
    }
    void movementManager()
    {
        switch (agentStates)
        {
            case ShooterAgentStates.None:
                rb.velocity = Vector3.zero;
                break;
            case ShooterAgentStates.ShootTower:
                ShootTower();
                break;

            
        }
    }
    private void ShootTower()
    {
        SreeringBehaviours.lookAt(transform, basicAgent.getTarget().position - transform.position);
        timerBullet += Time.deltaTime;
        timerMisil += Time.deltaTime;
        if (timerBullet >= 2f)
        {
            Bullet();
            timerBullet = 0;
        }
        if (timerMisil >= 3f)
        {
            //Misil();
            timerMisil = 0;
        }
    }
   
    /// <summary>
    /// Instancia una bala en el punto de aparici?n de balas.
    /// </summary>
    private void Bullet()
    {
        Instantiate(prf_bullet, spawnPointBullet.transform.position, transform.rotation);
    }

    /// <summary>
    /// Instancia un misil en el punto de aparici?n de misiles y configura su l?gica.
    /// </summary>
    //private void Misil()
    //{
    //    Vector3 missileSpawn = spawnPointMisil.transform.position;
    //    GameObject misil = Instantiate(prf_misil, missileSpawn, transform.rotation);
    //    comingMisil = misil.GetComponent<ComingMisil>();
    //    comingMisil.MisilLogic(basicAgent.target.GetComponent<BasicAgent>(), basicAgent.target.transform);
    //}
 
    private enum ShooterAgentStates
    {
        None,
        ShootTower,
      
    }
    private void OnDrawGizmos()
    {
        //Percepcion ojos y  escuha 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(eyesPercept.position, eyesPerceptRadious);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(earsPercept.position, earsPerceptRadious);
    }
}
