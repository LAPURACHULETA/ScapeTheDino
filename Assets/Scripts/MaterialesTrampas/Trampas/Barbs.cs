using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barbs : MonoBehaviour
{
    [SerializeField] private float groundPerceptRadious, barbsPerceptRadious, damageHeal;
    [SerializeField] private Transform groundPercept, barbsPercept;

    Collider[] col_GroundPerceibed, col_BarbsPerceibed;
    BasicAgent basicAgent;

    ShooterAgentStates agentStates;
    float timerBarbs;
    bool inGround, inBrabs;
    public string enemyTag, groundTag;
    void Start()
    {
        basicAgent = GetComponent<BasicAgent>();
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
        if (!basicAgent)
        {
            return;
        }
        basicAgent.targetPlayer = null;
        inGround = false;
        inBrabs = false;

        if (col_GroundPerceibed != null)
        {
            foreach (Collider tmp in col_GroundPerceibed)
            {
                if (tmp.CompareTag(groundTag))
                {
                    basicAgent.targetPlayer = tmp.transform;
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
                    basicAgent.targetPlayer = tmp.transform;
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

        movementManager();

    }
    void actionManager()
    {
        switch (agentStates)
        {
            case ShooterAgentStates.None:
                break;

            case ShooterAgentStates.InGround:
                BarbsDamage();
                break;

        }
    }
    void movementManager()
    {
        switch (agentStates)
        {
            case ShooterAgentStates.None:
                break;
            case ShooterAgentStates.InGround:
                break;

        }
    }
    private void BarbsDamage()
    {
        timerBarbs += Time.deltaTime;
        if (timerBarbs >= 2f)
        {
            foreach (Collider tmp in col_GroundPerceibed)
            {
                if (tmp.CompareTag(enemyTag))
                {
                    if (tmp.GetComponent<HealthPlayer>() is var life && life != null)
                    {
                        life.DamagePlayer();
                    }
                }
                else
                {

                }
            }
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
