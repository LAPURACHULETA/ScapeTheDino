using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float groundPerceptRadious, explosionPerceptRadious;
    [SerializeField] private Transform groundPercept, explosionPercept;
    [SerializeField] private int damageToEnemy;
    [SerializeField] private GameObject myTramp;

    Collider[] col_GroundPerceibed, col_ExplosionPerceibed;
    ShooterAgentStates agentStates;
    float timerMolotov;
    bool inGround;
    public string enemyTag, groundTag;
    void Start()
    {
 
        inGround = false;
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
                Explosion();
                break;

        }
    }
    private void Explosion()
    {
        timerMolotov += Time.deltaTime;
        if (timerMolotov >= 2f)
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

            Destroy(myTramp);
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
        Gizmos.DrawWireSphere(explosionPercept.position, explosionPerceptRadious);
    }
}
