using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMeele : MonoBehaviour
{
    [SerializeField] private float eyesPerceptRadious, earsPerceptRadious, damageHeal;

    [SerializeField] private float slowingRadious, thershold;

    [SerializeField] private Transform eyesPercept, earsPercept, leader;

    Rigidbody rb;
    Collider[] col_eyesPerceibed, col_earspPerceibed;
    BasicAgent basicAgent;
    //Boid boid;

    AgentStates agentStates;
    [SerializeField]

    float timerHit;
    public string enemyTag, enemyLeaderTag, leaderTag;
    // Start is called before the first frame update
    void Start()
    {

        basicAgent = GetComponent<BasicAgent>();
        //boid = GetComponent<Boid>();
        rb = GetComponent<Rigidbody>();
        agentStates = AgentStates.None;
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        col_eyesPerceibed = Physics.OverlapSphere(eyesPercept.position, eyesPerceptRadious);
        col_earspPerceibed = Physics.OverlapSphere(earsPercept.position, earsPerceptRadious);

        perceptionManager();
        decisionManager_Melee();
    }
    public void perceptionManager()
    {
        basicAgent.target = null;
        basicAgent.targetLeader = null;
        leader = null;

        if (col_eyesPerceibed != null)
        {
            foreach (Collider tmp in col_eyesPerceibed)
            {
                if (tmp.CompareTag(enemyTag))
                {
                    basicAgent.target = tmp.transform;
                }
                if (tmp.CompareTag(enemyLeaderTag))
                {
                    basicAgent.targetLeader = tmp.transform;
                }
            }
        }
        if (col_earspPerceibed != null)
        {
            foreach (Collider tmp in col_earspPerceibed)
            {
                if (tmp.CompareTag(enemyTag))
                {
                    basicAgent.target = tmp.transform;
                }
                if (tmp.CompareTag(leaderTag))
                {
                    leader = tmp.transform;
                }
                if (tmp.CompareTag(enemyLeaderTag))
                {
                    basicAgent.targetLeader = tmp.transform;
                }
            }
        }
    }
   
    void decisionManager_Melee()
    {
        if (basicAgent.target != null)
        {
            if (basicAgent.target.CompareTag(enemyTag))
            {
                agentStates = AgentStates.Attack;
                agentStates = AgentStates.Hit;
            }
        }
        else if (basicAgent.targetLeader != null)
        {
            if (basicAgent.targetLeader.CompareTag(enemyLeaderTag))
            {
                agentStates = AgentStates.AttackLeader;
                agentStates = AgentStates.HitLeader;
            }
        }

        else if (leader != null)
        {
            agentStates = AgentStates.LeaderFollow;
        }
        if (basicAgent.target == null && leader == null && basicAgent.targetLeader == null)
        {
            agentStates = AgentStates.None;
        }
        actionManager();

        movementManager();
    }
    void decisionManager_LeaderMelee()
    {
        if (basicAgent.target == null)
        {
            agentStates = AgentStates.FollowPath;
        }
        if (basicAgent.target != null)
        {
            if (basicAgent.target.CompareTag(enemyTag))
            {
                agentStates = AgentStates.Attack;
                agentStates = AgentStates.Hit;
            }
        }
        else if (basicAgent.targetLeader != null)
        {
            if (basicAgent.targetLeader.CompareTag(enemyLeaderTag))
            {
                agentStates = AgentStates.AttackLeader;
                agentStates = AgentStates.HitLeader;
            }
        }
        actionManager();

        movementManager();

    }
    void actionManager()
    {
        switch (agentStates)
        {
            case AgentStates.None:
                break;
            case AgentStates.Attack:
                break;
            case AgentStates.Evade:
                break;
            case AgentStates.FollowPath:
                break;
            case AgentStates.Hit:
                break;
            case AgentStates.LeaderFollow:
                break;
            case AgentStates.HitLeader:
                break;
            case AgentStates.AttackLeader:
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
            case AgentStates.Hit:
                Hit();
                break;
            case AgentStates.FollowPath:
                pathFollowing();
                break;
            case AgentStates.LeaderFollow:
                LeaderFollow();
                break;

            case AgentStates.HitLeader:
                HitLeader();
                break;
            case AgentStates.AttackLeader:
                AttackLeader();
                break;



        }
    }
    private void LeaderFollow()
    {
        var behaind = SreeringBehaviours.FollowLeader(basicAgent, leader, thershold, slowingRadious, thershold);
        var distancia = Vector3.Distance(transform.position, behaind);

        SreeringBehaviours.lookAt(transform, rb.velocity);

        if (distancia <= slowingRadious)
        {
            rb.velocity = SreeringBehaviours.arrival(basicAgent, behaind, slowingRadious, thershold);
        }
        else if (distancia > 0)
        {
            rb.velocity = SreeringBehaviours.seek(basicAgent, behaind);
        }

    }

    private void Attack()
    {
        rb.velocity = SreeringBehaviours.Pursuit(basicAgent, basicAgent.target.GetComponent<BasicAgent>());
        //rb.velocity = SreeringBehaviours.seek(basicAgent, basicAgent.target.position);
        SreeringBehaviours.lookAt(transform, rb.velocity);
        if (Vector3.Distance(transform.position, basicAgent.target.position) <= slowingRadious)
        {
            rb.velocity = SreeringBehaviours.arrival(basicAgent, basicAgent.target.position, slowingRadious, thershold);
        }
    }
    private void Hit()
    {
        foreach (Collider tmp in col_eyesPerceibed)
        {
            if (tmp.CompareTag(enemyTag))
            {
                timerHit += Time.deltaTime;
                if (timerHit >= .8f)
                {
                    //if (tmp.GetComponent<LifePlayer>() is var life && life != null)
                    //{
                    //    life.TakeDamagePlayer(damageHeal);
                    //}
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
                rb.velocity = SreeringBehaviours.Pursuit(basicAgent, basicAgent.target.GetComponent<BasicAgent>());
                SreeringBehaviours.lookAt(transform, rb.velocity);
            }
        }
    }
    private void AttackLeader()
    {
        rb.velocity = SreeringBehaviours.Pursuit(basicAgent, basicAgent.targetLeader.GetComponent<BasicAgent>());
        //rb.velocity = SreeringBehaviours.seek(basicAgent, basicAgent.target.position);
        SreeringBehaviours.lookAt(transform, rb.velocity);
        if (Vector3.Distance(transform.position, basicAgent.targetLeader.position) <= slowingRadious)
        {
            rb.velocity = SreeringBehaviours.arrival(basicAgent, basicAgent.targetLeader.position, slowingRadious, thershold);
        }
    }
    private void HitLeader()
    {
        foreach (Collider tmp in col_eyesPerceibed)
        {
            if (tmp.CompareTag(enemyLeaderTag))
            {
                timerHit += Time.deltaTime;
                if (timerHit >= .8f)
                {
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
            if (tmp.CompareTag(enemyLeaderTag))
            {
                rb.velocity = SreeringBehaviours.Pursuit(basicAgent, basicAgent.targetLeader.GetComponent<BasicAgent>());
                SreeringBehaviours.lookAt(transform, rb.velocity);
            }
        }
    }

    private void pathFollowing()
    {
    //    rb.velocity = boid.pathFollowing();
        SreeringBehaviours.lookAt(transform, rb.velocity);
    }


    private enum AgentStates
    {
        None,
        Attack,
        Hit,
        AttackLeader,
        HitLeader,
        FollowPath,
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
