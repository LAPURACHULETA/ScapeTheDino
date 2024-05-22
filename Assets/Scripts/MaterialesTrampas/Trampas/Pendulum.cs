using UnityEngine;
using UnityEngine.Rendering;

public class Pendulum : MonoBehaviour
{
    [SerializeField] private float groundPerceptRadious;
    [SerializeField] private int damageToEnemy;
    [SerializeField] private Transform groundPercept;

    Collider[] col_GroundPerceibed;

    ShooterAgentStates agentStates;
    bool inEnemy;
    float timerToDestroy;
    public string enemyTag;


    void Start()
    {
        inEnemy = false;
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
        inEnemy = false;
       
        if (col_GroundPerceibed != null)
        {
            foreach (Collider tmp in col_GroundPerceibed)
            {
                if (tmp.CompareTag(enemyTag))
                {
                    inEnemy = true;
                }
            }
        }
    }
    void decisionManager_EnemyTower()
    {

        if (inEnemy)
        {
            agentStates = ShooterAgentStates.InEnemy;
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

            case ShooterAgentStates.InEnemy:
                PendulumDamage();
                break;

        }
    }

    private void PendulumDamage()
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
    private void PendulumInLive()
    {
        timerToDestroy += Time.deltaTime;
        if (timerToDestroy >= 10f)
        {
            Destroy(gameObject);
        }
    }
 
    private enum ShooterAgentStates
    {
        None,
        InEnemy,
    }
    private void OnDrawGizmos()
    {
        //Percepcion ojos y  escuha 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundPercept.position, groundPerceptRadious);
    }
}
