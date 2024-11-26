using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class IAEscupidor : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] private float eyesPerceptRadious, earsPerceptRadious, damageHeal;
    [SerializeField] private Transform eyesPercept, earsPercept;
    Rigidbody rb;
    Collider[] col_eyesPerceibed, col_earspPerceibed;
    BasicAgent basicAgent;

    /// <summary>
    /// ComingMisil comingMisil;
    /// </summary>

    public GameObject prf_bullet, spawnPointBullet;
    ShooterAgentStates agentStates;
    float timerBullet, timerEvade;
    bool evadir,inEyes,inEars;
    public string enemyTag;

    void Start()
    {
        basicAgent = GetComponent<BasicAgent>();
        rb = GetComponent<Rigidbody>();
        agentStates = ShooterAgentStates.None;
        inEyes = false;
        inEars = false;
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
        inEyes = false;
        inEars = false; 
        
        if (col_eyesPerceibed != null)
        {
            foreach (Collider tmp in col_eyesPerceibed)
            {
                if (tmp.CompareTag(enemyTag))
                {
                    basicAgent.targetPlayer = tmp.transform;
                    inEyes = true;
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
                    inEars = true;
                }
            }
        }
    }
    void decisionManager_EnemyTower()
    {

        if (inEars)
        {
            evadir = true;
            agentStates = ShooterAgentStates.Evade;
        }
        else if (inEyes)
        {
            inEars = false;
            agentStates = ShooterAgentStates.Shoot;
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
                animator.SetBool("isIdling", true);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isRunning", false);
                break;
            case ShooterAgentStates.Shoot:
                animator.SetBool("isIdling", false);
                animator.SetBool("isAttacking", true);
                animator.SetBool("isRunning", false);
                break;
            case ShooterAgentStates.Evade:
                animator.SetBool("isIdling", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isRunning", true);
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
            case ShooterAgentStates.Shoot:
                ShootTower();
                rb.velocity = Vector3.zero;
                break;
            case ShooterAgentStates.Evade:
                Evade();
                break;
          
        }
    }
    private void Evade()
    {
        rb.velocity = SreeringBehaviours.Evade(basicAgent, basicAgent.targetPlayer);
        SreeringBehaviours.rotation2(transform, 5, basicAgent.targetPlayer.GetComponent<BasicAgent>());
    }
    private void ShootTower()
    {
        SreeringBehaviours.rotation(transform, 5, basicAgent.targetPlayer.GetComponent<BasicAgent>());
        timerBullet += Time.deltaTime;
        if (timerBullet >= 2f)
        {
            Bullet();
            timerBullet = 0;
        }
    }
    private void Bullet()
    {
        Instantiate(prf_bullet, spawnPointBullet.transform.position, transform.rotation);
    }
    private enum ShooterAgentStates
    {
        None,
        Shoot,
        Evade
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
