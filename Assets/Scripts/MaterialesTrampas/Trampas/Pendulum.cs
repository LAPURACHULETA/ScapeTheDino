using UnityEngine;
using UnityEngine.Rendering;

public class Pendulum : MonoBehaviour
{
    [SerializeField] private float groundPerceptRadious, damageHeal;
    [SerializeField] private Transform groundPercept;

    Collider[] col_GroundPerceibed;
    BasicAgent basicAgent;

    ShooterAgentStates agentStates;
    bool inGround;
    float timerToDestroy;
    public string enemyTag;

    void Start()
    {
        basicAgent = GetComponent<BasicAgent>();
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
        if (!basicAgent)
        {
            return;
        }

        basicAgent.targetPlayer = null;
        inGround = false;

       
        if (col_GroundPerceibed != null)
        {
            foreach (Collider tmp in col_GroundPerceibed)
            {
                if (tmp.CompareTag(enemyTag))
                {
                    basicAgent.targetPlayer = tmp.transform;
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
                PendulumInLive();
                break;

            case ShooterAgentStates.InGround:
                PendulumDamage();
                break;

        }
    }

    private void PendulumInLive()
    {
        timerToDestroy += Time.deltaTime;
        if (timerToDestroy >= 2f)
        {
            Destroy(gameObject);
        }
    }
    private void PendulumDamage()
    {
      
        foreach (Collider tmp in col_GroundPerceibed)
        {
            if (tmp.CompareTag(enemyTag))
            {
                if (tmp.GetComponent<HealthPlayer>() is var life && life != null)
                {
                    life.DamagePlayer();
                    Destroy(gameObject);

                }
            }
            else
            {

            }
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
    }
}
