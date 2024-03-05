using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    Transform agent; 
    Transform iA;
    private GameObject bullet;
    private Transform objTrans;

    public float projectileSpeed = 10f;
    public float h = 10;
    private float gravity = 9.8f; 
    
    public void SetSpawnPoint(Transform spawn)
    {
        iA = spawn;
    }  
    public void SetAgent(Transform targetplayer)
    {
        agent = targetplayer;
    }
    public void Shoot()
    {
        Rigidbody bulletRb=bullet.GetComponent<Rigidbody>();

        Physics.gravity=Vector3.up*gravity;
        bulletRb.useGravity = true;
        bulletRb.velocity = CalculateIniticalVelocity();
    }
    Vector3 CalculateIniticalVelocity()
    {
        Vector3 desplazamientoP=objTrans.position-bullet.transform.position;
        float velY,velX,velZ;
        velY = Mathf.Sqrt(-2 * gravity * h);
        velX = desplazamientoP.x / ((-velY / gravity) + Mathf.Sqrt(2 * (desplazamientoP.y - h) / gravity));
        velZ = desplazamientoP.z / ((-velY / gravity) + Mathf.Sqrt(2 * (desplazamientoP.y - h) / gravity));
        return new Vector3(velX, velY, velZ);
    }
}
