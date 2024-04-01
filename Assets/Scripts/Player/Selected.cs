using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
[RequireComponent(typeof(PlayerInput))]
public class Selected : MonoBehaviour
{
    [SerializeField]private float distanceMaterial;
    [SerializeField]private float distanceObstacle;
    [SerializeField]private float jumpforce;
 
    private float valueButton;
    private Rigidbody rb;

    // Update is called once per frame
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
 
    }
    void Update()
    {
        DetectionOfObject();
  
    }
    public void OnInteractive(InputValue context)
    {
        valueButton = context.Get<float>();
    }
    void DetectionOfObject()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distanceMaterial))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.black);

            if (hit.collider.tag == "MaterialTrampas")
            {
                if (valueButton == 1)
                {
                    hit.collider.gameObject.GetComponent<Interactive>().TakeObject();
                }
            }

       
        }
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distanceObstacle))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

            if (hit.collider.tag == "Obstacle")
            {
                if (valueButton == 1)
                {
                    //hit.collider.gameObject.GetComponent<Interactive>().Parkour(rb,jumpforce);
                    rb.AddForce(Vector3.up * jumpforce);
                }
            }
        }
       
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}
