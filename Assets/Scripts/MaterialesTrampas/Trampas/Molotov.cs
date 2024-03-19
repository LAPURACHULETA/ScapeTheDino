using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : MonoBehaviour
{

    [SerializeField] private float groundPerceptRadious, explosionPerceptRadious, damageHeal;
    [SerializeField] private Transform groundPercept, explosionPercept;
    Rigidbody rb;
    Collider[] col_GroundPerceibed, col_ExplosionPerceibed;
    BasicAgent basicAgent;

    ShooterAgentStates agentStates;
    float timerMolotov;
    bool inGround, inExplosion;
    public string enemyTag, groundTag;
    void Start()
    {
        basicAgent = GetComponent<BasicAgent>();
        rb = GetComponent<Rigidbody>();
        inGround = false;
        inExplosion = false;
    }

    private void FixedUpdate()
    {
        col_GroundPerceibed = Physics.OverlapSphere(groundPercept.position, groundPerceptRadious);
        col_ExplosionPerceibed = Physics.OverlapSphere(explosionPercept.position, explosionPerceptRadious);

        perceptionManager();
        decisionManager_EnemyTower();
    }

    /// <summary>
    /// Gestiona la percepci?n del agente y establece el objetivo.
    /// </summary>
    public void perceptionManager()
    {
        basicAgent.targetPlayer = null;
        inGround = false;
        inExplosion = false;

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

        if (col_ExplosionPerceibed != null)
        {
            foreach (Collider tmp in col_ExplosionPerceibed)
            {
                if (tmp.CompareTag(enemyTag))
                {
                    basicAgent.targetPlayer = tmp.transform;
                    inExplosion = true;
                }
            }
        }
    }
    void decisionManager_EnemyTower()
    {

        if (inExplosion)
        {
            agentStates = ShooterAgentStates.InExplosion;
        }
        else if (inGround)
        {
            inExplosion = false;
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
                Ground();
                break;
            case ShooterAgentStates.InExplosion:
                Explosion();
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
            case ShooterAgentStates.InExplosion:
                break;

        }
    }
    private void Ground()
    {
        timerMolotov += Time.deltaTime;
        if (timerMolotov >= 2f)
        {
            Destroy(gameObject);
        }
    }
    private void Explosion()
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
        }
    }

    private enum ShooterAgentStates
    {
        None,
        InGround,
        InExplosion
    }
    private void OnDrawGizmos()
    {
        //Percepcion ojos y  escuha 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundPercept.position, groundPerceptRadious);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(explosionPercept.position, explosionPerceptRadious);
    }
}
