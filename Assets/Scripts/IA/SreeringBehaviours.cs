using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SreeringBehaviours : MonoBehaviour
{
    /// <summary>
    /// se dirige al target
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    public static Vector3 seek(BasicAgent agent, Vector3 targetPosition)
    {
        Vector3 desiredVel = targetPosition - agent.transform.position;
        return calculateSteer(agent, desiredVel);
    }

    /// <summary>
    /// corre del jugador 
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    public static Vector3 flee(BasicAgent agent, Vector3 targetPosition)
    {
        Vector3 desiredVel = agent.transform.position - targetPosition;
        return calculateSteer(agent, desiredVel);
    }
    /// <summary>
    /// Freando de personaje una cierta distancia
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="targetPosition"></param>
    /// <param name="slowingRadious"></param>
    /// <returns></returns>
    public static Vector3 arrival(BasicAgent agent, Vector3 targetPosition, float slowingRadious, float threshold)
    {
        Vector3 newVel = agent.GetComponent<Rigidbody>().velocity;
        float distance = Vector3.Distance(agent.transform.position, targetPosition);
        float slowingCoeficient = (distance < threshold) ? distance / slowingRadious : 0;

        newVel *= slowingCoeficient;
        return newVel;
    }
    /// <summary>
    /// no aplica movimiento solo la primera posicion
    /// </summary>
    /// <returns></returns>
    public static Vector3 wander(BasicAgent agent)
    {
        Vector3 velCopy = agent.transform.GetComponent<Rigidbody>().velocity;
        velCopy.Normalize();
        velCopy *= agent.m_wanderDisplacement;
        Vector3 randomDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        randomDirection.Normalize();
        randomDirection *= agent.m_wanderRadious;
        randomDirection += velCopy;
        randomDirection += agent.transform.position;
        return randomDirection;
    }

    public static Vector3 Pursuit(BasicAgent agent, BasicAgent target)
    {
        Vector3 rb = target.transform.GetComponent<Rigidbody>().velocity.normalized;
        Vector3 distance = target.transform.position - agent.transform.position;
        float T = distance.magnitude / agent.m_maxVel;
        Vector3 futurePosition = target.transform.position + rb * T;
        return seek(agent, futurePosition);
    }
    public static Vector3 Evade(BasicAgent agent, Transform targetPosition)
    {
        Rigidbody rb = targetPosition.GetComponent<Rigidbody>();
        Vector3 distance = rb.position - agent.transform.position;
        float updatesAhead = distance.magnitude / agent.m_maxVel;
        Vector3 futurePosition = rb.position + rb.velocity * updatesAhead;
        return flee(agent, futurePosition);
    }
    public static Vector3 FollowLeader(BasicAgent agent, Transform target, float Leader_Behind_Disst, float slowingRadius, float threshold)
    {
        Vector3 tv = target.transform.GetComponent<Rigidbody>().velocity;
        Vector3 force = new Vector3();

        tv.Scale(-Vector3.one);
        tv.Normalize();
        tv *= Leader_Behind_Disst;
        Vector3 behind = target.position + tv;
        return behind;
    }
    //public static Vector3 Separation(BasicAgent agent, Vector3 SreeringBehaviours)
    //{
    //    Vector3 force = new Vector3();
    //    int neighborCount = 0;
    //    foreach (SreeringBehaviours b in agent.instance.boids)
    //    {
    //        if (b != this && Vector3.Distance(b.transform.position, transform.position) <= SEPARATION_RADIUS)
    //        {
    //            force += b.transform.position - transform.position;
    //            neighborCount++;
    //        }
    //    }
    //}
    public static List<Collider> GetNeighborAhead(BasicAgent agent, List<Collider> list, List<Collider> list1)
    {
        List<Collider> neighborsAhead = new List<Collider>();
        Vector3 qa = agent.transform.GetComponent<Rigidbody>().velocity;
        qa.Normalize();
        qa += agent.m_queueAHEAD;
        Vector3 ahead = qa + agent.transform.position;

        foreach (Collider neighborCollider in list)
        {
            if (neighborCollider.CompareTag("LiderEnemy"))
            {
                Vector3 neighborPosition = neighborCollider.transform.position;
                float distance = Vector3.Distance(neighborPosition, ahead);
                if (distance <= agent.MAX_QUEUE_RADIUS)
                {
                    neighborsAhead.Add(neighborCollider);
                }
            }
        }
        return neighborsAhead;
    }


    public static Vector3 Queue(BasicAgent agent, List<Collider> ret, List<Collider> list)
    {
        Vector3 velCopy = agent.transform.GetComponent<Rigidbody>().velocity;


        List<Collider> neighbor = GetNeighborAhead(agent, ret, list);
        // Aquí se realizarían más operaciones basadas en la detección de vecinos adelante.
        // Esto podría implicar calcular una fuerza de cola en función de los vecinos detectados.
        if (neighbor != null)
        {
            Vector3.Scale(new Vector3(.3f, .3f, .3f), velCopy);
        }
        Vector3 queueForce = new Vector3(0, 0, 0); // Vector de fuerza de cola (actualmente nulo).

        // Lógica adicional para determinar la fuerza de la cola, basada en la detección de vecinos.

        return queueForce; // Devolver la fuerza calculada para la cola.
    }

    /// <summary>
    /// calcula el calbio de posicion hacia el target
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="desiredVel"></param>
    /// <returns></returns>
    private static Vector3 calculateSteer(BasicAgent agent, Vector3 desiredVel)
    {
        Rigidbody agentRB = agent.GetComponent<Rigidbody>();
        desiredVel.Normalize();
        desiredVel *= agent.getMaxVel();
        Vector3 steering = desiredVel - agentRB.velocity;
        steering = truncate(steering, agent.getMaxSteerForce());
        steering /= agentRB.mass;
        steering += agentRB.velocity;
        steering = truncate(steering, agent.getSpeed());
        steering.y = 0;
        return steering;
    }

    /// <summary>
    /// rota el eje z hacia el agent
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="currentVel"></param>
    public static void lookAt(Transform agent, Vector3 currentVel)
    {
        agent.transform.LookAt(agent.position + currentVel);
    } 
   
    public static void rotation(Transform rb,float speed,BasicAgent target)
    {
        Vector3 relativePos = target.transform.position - rb.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        rb.transform.rotation = Quaternion.Lerp(rb.transform.rotation, rotation, Time.deltaTime * speed);
        //rb.transform.rotation = new(rb.transform.rotation.x,0,rb.transform.rotation.y,Time.deltaTime*speed);
        rb.transform.rotation = Quaternion.Euler(0, rb.transform.rotation.eulerAngles.y, 0);
    }
    private static Vector3 truncate(Vector3 vector, float maxValue)
    {
        if (vector.magnitude <= maxValue)
        {
            return vector;
        }
        vector.Normalize();
        return vector *= maxValue;
    }
}
