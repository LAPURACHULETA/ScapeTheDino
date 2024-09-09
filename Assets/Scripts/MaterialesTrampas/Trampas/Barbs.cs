using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barbs : MonoBehaviour
{
    [SerializeField] private float groundPerceptRadious, barbsPerceptRadious, damageHeal;
    [SerializeField] private Transform groundPercept, barbsPercept;
    [SerializeField] private int damageToEnemy;
    Collider[] col_GroundPerceibed, col_BarbsPerceibed;

    ShooterAgentStates agentStates;
    float timerBarbs;
    bool inGround, inBrabs;
    public string enemyTag, groundTag;
    void Start()
    {
       
        inGround = false;
        inBrabs = false;
    }

    private void FixedUpdate()
    {
        col_GroundPerceibed = Physics.OverlapSphere(groundPercept.position, groundPerceptRadious);
        col_BarbsPerceibed = Physics.OverlapSphere(barbsPercept.position, barbsPerceptRadious);

        perceptionManager();
        decisionManager_EnemyTower();
    }

    /// <summary>
    /// Gestiona la percepci?n del agente y establece el objetivo.
    /// </summary>
    public void perceptionManager()
    {
      
        inGround = false;
        inBrabs = false;

        if (col_GroundPerceibed != null)
        {
            foreach (Collider tmp in col_GroundPerceibed)
            {
                if (tmp.CompareTag(groundTag))
                {
                    
                    inGround = true;
                }
            }
        }

        if (col_BarbsPerceibed != null)
        {
            foreach (Collider tmp in col_BarbsPerceibed)
            {
                if (tmp.CompareTag(enemyTag))
                {
                   
                    inBrabs = true;
                }
            }
        }
    }
    void decisionManager_EnemyTower()
    {

        if (inBrabs)
        {
            agentStates = ShooterAgentStates.InGround;
        }
        else if (inGround)
        {
            inBrabs = false;
            agentStates = ShooterAgentStates.InGround;
        }
        else
        {
            agentStates = ShooterAgentStates.None;
        }
        actionManager();

    }
    void actionManager()
    {
        switch (agentStates)
        {
            case ShooterAgentStates.None:
                break;

            case ShooterAgentStates.InGround:
                TimeDestroy();
                BarbsDamage();
                break;

        }
    }
    private void BarbsDamage()
    {
        foreach (Collider tmp in col_GroundPerceibed)
        {
            if (tmp.CompareTag(enemyTag))
            {
                if (tmp.GetComponent<HealthEnemy>() is var life && life != null)
                {
                    life.DamageEnemy(damageToEnemy);
                }
            }
        }
    }
    private void TimeDestroy()
    {
        timerBarbs += Time.deltaTime;
        if (timerBarbs >= 10f)
        {
            Destroy(gameObject);
        }
    }
    private enum ShooterAgentStates
    {
        None,
        InGround,
    }
    private void OnDrawGizmos()
    {
        //Percepcion ojos y  escuha 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundPercept.position, groundPerceptRadious);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(barbsPercept.position, barbsPerceptRadious);
    }
}
