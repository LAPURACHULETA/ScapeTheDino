using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPuzle : MonoBehaviour
{
    public GameObject poste;
    public Transform laserOrigin;
    public float range;

    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer=GetComponent<LineRenderer>();
    }
    private void FixedUpdate()
    {
        lineRenderer.SetPosition(0,laserOrigin.position);
        Vector3 rayOrigin=poste.transform.position;

        RaycastHit hit;

        if(Physics.Raycast(rayOrigin,poste.transform.forward,out hit, range))
        {
            lineRenderer.SetPosition(1,hit.point);
        }
        if (hit.collider.tag=="Torre")
        {
            Debug.Log("torre");
        }
        else
        {
            lineRenderer.SetPosition(1, rayOrigin + (poste.transform.forward * range));
        }
    }
}
