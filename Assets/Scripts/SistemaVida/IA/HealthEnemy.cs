using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    [Header("Vida de Enemigo")]
    [Space(10)]

    BasicAgent basicAgent;

    // Update is called once per frame
    private void Start()
    {
        basicAgent = GetComponent<BasicAgent>();

    }
    private void Update()
    {
        MyHelath();
    }

    public void DamageEnemy(int damge)
    {
        basicAgent.TakeDamage(damge);
        Destroy(this.gameObject);
    }
    private void MyHelath()
    {
        if (basicAgent.m_vida <= 0)
        {
            basicAgent.Die(this.gameObject);
        }
    }
  
}
