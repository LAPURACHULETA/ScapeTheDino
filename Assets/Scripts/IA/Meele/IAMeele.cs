using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMeele : MonoBehaviour
{
    [SerializeField] private float eyesPerceptRadious, earsPerceptRadious;

    [SerializeField] private float slowingRadious, thershold;
    [SerializeField] private float timerToHit;

    [SerializeField] private Transform eyesPercept, earsPercept;

    Rigidbody rb;
    Collider[] col_eyesPerceibed, col_earspPerceibed;
    BasicAgent basicAgent;
    //Boid boid;

    AgentStates agentStates;
    [SerializeField]

    private float timerHit;
    public string enemyTag;
    // Start is called before the first frame update
    void Start()
    {
        basicAgent = GetComponent<BasicAgent>();
        rb = GetComponent<Rigidbody>();
        agentStates = AgentStates.None;
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        col_eyesPerceibed = Physics.OverlapSphere(eyesPercept.position, eyesPerceptRadious);
        col_earspPerceibed = Physics.OverlapSphere(earsPercept.position, earsPerceptRadious);

        PerceptionManager();
        DecisionManager();
    }
    public void PerceptionManager()
    {
        if (!basicAgent)
        {
            return;
        }
        basicAgent.targetPlayer = null;
        basicAgent.targetWall = null;

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
   
    void DecisionManager()
    {
        if (basicAgent.targetPlayer != null)
        {
            if (basicAgent.targetPlayer.CompareTag(enemyTag))
            {
                agentStates = AgentStates.Attack;
            }
        }
        ActionManager();

        MovementManager();
    }
  
    void ActionManager()
    {
        switch (agentStates)
        {
            case AgentStates.None:
                break;
            case AgentStates.Attack:
                break;
            case AgentStates.Evade:
                break;
        
        }
    }
    void MovementManager()
    {
        switch (agentStates)
        {
            case AgentStates.None:
                rb.velocity = Vector3.zero;
                break;
            case AgentStates.Attack:
                Attack();
                break;
           
        }
    }

    private void Attack()
    {
        rb.velocity = SreeringBehaviours.Pursuit(basicAgent, basicAgent.targetPlayer.GetComponent<BasicAgent>());
        //rb.velocity = SreeringBehaviours.seek(basicAgent, basicAgent.target.position);
        //SreeringBehaviours.lookAt(transform, rb.velocity);
        SreeringBehaviours.rotation(transform, 5, basicAgent.targetPlayer.GetComponent<BasicAgent>());
        if (Vector3.Distance(transform.position, basicAgent.targetPlayer.position) <= slowingRadious)
        {
            rb.velocity = SreeringBehaviours.arrival(basicAgent, basicAgent.targetPlayer.position, slowingRadious, thershold);
        }

        foreach (Collider tmp in col_eyesPerceibed)
        {
            if (tmp.CompareTag(enemyTag))
            {
                timerHit += Time.deltaTime;
                if (timerHit >= timerToHit)
                {
                    if (tmp.GetComponent<HealthPlayer>() is var life && life != null)
                    {
                        life.DamagePlayer();
                    }
                    //if (tmp.GetComponent<HealthEnemy>() is var healthEnemy && healthEnemy != null)
                    //{
                    //    healthEnemy.TakeDamageEnemy(damageHeal);
                    //}

                    Debug.Log("Le pego");
                    timerHit = 0;
                }
            }

        }
        foreach (Collider tmp in col_earspPerceibed)
        {
            if (tmp.CompareTag(enemyTag))
            {
                rb.velocity = SreeringBehaviours.Pursuit(basicAgent, basicAgent.targetPlayer.GetComponent<BasicAgent>());
                //SreeringBehaviours.lookAt(transform, rb.velocity);
            }
        }
    }

   
    private enum AgentStates
    {
        None,
        Attack,
        Evade,
        LeaderFollow,

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
