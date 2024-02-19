using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
   
    public void Die(/*GameObject obj*/)
    {
        //Destroy(obj);
        Debug.Log("Me mori");
    }
    public void TakeDamagePlayer(float damage,float vida,float vidaActual)
    {
        vidaActual=damage - vida;
    }
}
