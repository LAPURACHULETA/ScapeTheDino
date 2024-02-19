using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAgent : MonoBehaviour
{

    [SerializeField] float m_speed, m_maxSteerForce;
    public float m_maxVel;
    public float m_wanderDisplacement, m_wanderRadious;
    public Transform target;
    public Transform targetLeader;
    public Vector3? wanderNextPosition;

    internal float MAX_QUEUE_RADIUS;
    internal Vector3 m_queueAHEAD;

    /// <summary>
    /// Gives access to max speed of agent
    /// </summary>
    /// <returns>float m_speed</returns>
    public float getSpeed()
    {
        return m_speed;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float getMaxVel()
    {
        return m_maxVel;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float getMaxSteerForce()
    {
        return m_maxSteerForce;
    }
    public Transform getTarget()
    {
        return target;

    }
    public Transform getTargetLeader()
    {
        return targetLeader;
    }
}
