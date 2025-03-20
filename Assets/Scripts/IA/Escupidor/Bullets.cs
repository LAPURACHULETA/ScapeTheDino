using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] BulletsStates bulletsStates;
    [SerializeField] private float speed, bulletDamage;
    float timeLive;
    Rigidbody rb;
    Collider[] col_eyesPerceibed;
    [SerializeField]
    float eyesPerceptRadious;
    [SerializeField]
    Transform eyesPercept;
    BasicAgent basicAgent;
    public string enemyTag,isGroundTag;
    /// <summary>
    /// Inicializa las referencias al agente basico y al componete Rigibody 
    /// </summary>
    public void Start()
    {
        basicAgent = FindObjectOfType<BasicAgent>();
        rb = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// Método FixedUpdate que controla el comportamiento de la bala en función de su estado.
    /// </summary>
    private void FixedUpdate()
    {
        switch (bulletsStates)
        {
            case BulletsStates.Bullet:
                BulletEnemy();
                break;
        }
        col_eyesPerceibed = Physics.OverlapSphere(eyesPercept.position, eyesPerceptRadious);
        perceptionManager();
    }
    public void perceptionManager()
    {
        basicAgent.targetPlayer = null;
        if (col_eyesPerceibed != null)
        {
            foreach (Collider tmp in col_eyesPerceibed)
            {
                if (tmp.CompareTag(enemyTag))
                {
                    if (tmp.transform.parent.GetComponent<HealthPlayer>() is var life && life != null)
                    {
                        life.DamagePlayer(bulletDamage); 
                    }
                }
                else if (tmp.CompareTag(isGroundTag))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    /// <summary>
    /// Mueve la bala hacia adelante con una velocidad dada y aplica daño a los objetos dentro de un radio especificado.
    /// </summary>
    public void BulletEnemy()
    {
        rb.AddForce(transform.forward * speed);
        TimeLive();
    }

    /// <summary>
    /// Controla el tiempo de vida de la bala y la destruye si excede un límite.
    /// </summary>
    public void TimeLive()
    {
        timeLive += Time.deltaTime;
        if (timeLive >= 2f)
        {
            Destroy(gameObject);
            return;
        }
    }
    /// <summary>
    /// Enumeración que define los estados posibles de la bala.
    /// </summary>
    private enum BulletsStates
    {
        Bullet,
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(eyesPercept.position, eyesPerceptRadious);
    }
}
