using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IATriceratops : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] private float eyesPerceptRadious, earsPerceptRadious;
    [SerializeField] private int damageToPlayer;

    [SerializeField] private Transform eyesPercept, earsPercept;

    Rigidbody rb;
    Collider[] col_eyesPerceibed, col_earspPerceibed;
    BasicAgent basicAgent;

    AgentStates agentStates;
    private float timerToSee,timerToStun;
    public string enemyTag,wall;
    bool inWall;
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
        basicAgent.targetPlayer = null;
        inWall = false;
        if (col_eyesPerceibed != null)
        {
            foreach (Collider tmp in col_eyesPerceibed)
            {
                if (tmp.CompareTag(enemyTag))
                {
                    basicAgent.targetPlayer = tmp.transform;
                }
                if (tmp.CompareTag(wall))
                {
                    inWall = true;
                }
                
            }
        }
        if (col_earspPerceibed != null)
        {
            foreach (Collider tmp in col_earspPerceibed)
            {
                ////Debug.Log(tmp.name);
                if (tmp.CompareTag(enemyTag))
                {
                    basicAgent.targetPlayer = tmp.transform;
                }
                
            }
        }
    }

    void DecisionManager()
    {
        if (inWall)
        {
            rb.velocity = Vector3.zero;
            timerToStun += Time.deltaTime;
            Debug.Log(timerToSee); 
            if (timerToStun >= 3f)
            {
                timerToSee = 0;
                timerToStun = 0;
                inWall = false;
            }
            
        }
        if (basicAgent.targetPlayer != null&&!inWall)
        {
            if (basicAgent.targetPlayer.CompareTag(enemyTag))
            {
                agentStates = AgentStates.Attack;
            }
        }
        else
        {
            agentStates = AgentStates.None;
        }
        actionManager();
        movementManager();
    }

    void actionManager()
    {
        switch (agentStates)
        {
            case AgentStates.None:
                animator.SetBool("isIdling", true);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isRunning", false);
                break;
            case AgentStates.Attack:
                animator.SetBool("isIdling", false);
                animator.SetBool("isAttacking", true);
                animator.SetBool("isRunning", false);
                break;
            
        }
    }
    void movementManager()
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
        ///Debug.Log(timerToSee);
        foreach (Collider tmp in col_earspPerceibed)
        {
            if (tmp.CompareTag(enemyTag))
            {
                timerToSee += Time.deltaTime;
                
                if (timerToSee <=4f)
                {
                    SreeringBehaviours.rotation(transform, 5, basicAgent.targetPlayer.parent.GetComponent<BasicAgent>());
                }
                else if(timerToSee <= 5f)
                {
                    rb.velocity = transform.forward*basicAgent.m_speed;
                }
                else if (timerToSee <= 6)
                {
                    timerToSee = 0;
                }
            }
        }

        foreach (Collider tmp in col_eyesPerceibed)
        {
            if (tmp.CompareTag(enemyTag))
            {
                if (tmp.transform.parent.GetComponent<HealthPlayer>() is var life && life != null)
                {
                    life.DamagePlayer(damageToPlayer);
                }
            }
        }

        //if (Vector3.Distance(transform.position, basicAgent.targetPlayer.position) <= slowingRadious)
        //{
        //    rb.velocity = SreeringBehaviours.arrival(basicAgent, basicAgent.targetPlayer.position, slowingRadious, thershold);
        //}
     
    }
 
    private enum AgentStates
    {
        None,
        Attack,
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
