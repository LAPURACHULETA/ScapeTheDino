using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Molotov : MonoBehaviour
{

    [SerializeField] private float groundPerceptRadious, explosionPerceptRadious, damageHeal;
    [SerializeField] private Transform groundPercept, explosionPercept;
    [SerializeField] private int damageToEnemy;
    [SerializeField] private GameObject myTramp;

    Rigidbody rb;
    Collider[] col_GroundPerceibed;

    ShooterAgentStates agentStates;
    float timerMolotov;
    bool inGround;
    public string enemyTag, groundTag;
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        inGround = false;
        
    }

    private void FixedUpdate()
    {
        col_GroundPerceibed = Physics.OverlapSphere(groundPercept.position, groundPerceptRadious);

        perceptionManager();
        decisionManager_EnemyTower();
    }

    /// <summary>
    /// Gestiona la percepci?n del agente y establece el objetivo.
    /// </summary>
    public void perceptionManager()
    {
     
        inGround = false;
       

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

    }
    void decisionManager_EnemyTower()
    {

        if (inGround)
        {
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
                Ground();
                break;
        }
    }

    private void Ground()
    {
        if (inGround)
        {
            timerMolotov += Time.deltaTime;
            //Debug.Log(timerMolotov);
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
            if (timerMolotov >= 2f)
            {
                Destroy(myTramp);
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
