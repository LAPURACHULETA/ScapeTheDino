using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAgent : MonoBehaviour
{

    public float m_speed, m_maxSteerForce;
    public float m_maxVel;
    public float m_wanderDisplacement, m_wanderRadious;

    public float m_vida;
    protected float m_vidaActual;

    public Transform targetPlayer;
    public Transform targetWall;
    public Vector3? wanderNextPosition;

    internal float MAX_QUEUE_RADIUS;
    internal Vector3 m_queueAHEAD;

    private void Start()
    {
        m_vidaActual = m_vida;
    }
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
        return targetPlayer;

    }
    public Transform getTargetLeader()
    {
        return targetWall;
    }
    public void Die(GameObject obj)
    {
        Destroy(obj);
        Debug.Log("Me mori");
    }
    public void TakeDamage(float damage)
    {
        m_vidaActual -= damage;
    }
 
}
