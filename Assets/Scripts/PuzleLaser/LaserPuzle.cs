using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserPuzle : MonoBehaviour
{
    public GameObject poste;
    [SerializeField] private GameObject doorLaser;
    public Transform laserOrigin;
    public float range;

    public bool isRotating;
    public bool isTorre;
    public float rotationSpeed = 10f;
    public float targetRotationY;
    public bool puzzleComplete;
    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer=GetComponent<LineRenderer>();
        isRotating=false;
    }
    private void FixedUpdate()
    {
        isTorre=false;
        lineRenderer.SetPosition(0,laserOrigin.position);
        Vector3 rayOrigin=poste.transform.position;

        RaycastHit hit;

        if (isRotating)
        {
            // Rotar el objeto hacia el ángulo objetivo
            float currentY = Mathf.LerpAngle(transform.eulerAngles.y, targetRotationY, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentY, transform.eulerAngles.z);

            // Verificar si hemos alcanzado el ángulo objetivo
            if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetRotationY)) < 0.1f)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetRotationY, transform.eulerAngles.z);
                isRotating = false;
            }

        }
        if (Physics.Raycast(rayOrigin,poste.transform.forward,out hit, range))
        {
            lineRenderer.SetPosition(1,hit.point);
            if(hit.collider.tag == "Torre")
            {
                isTorre=true;
                puzzleComplete = true;  
            }
        }
        if (isTorre == true)
        {
            Destroy(doorLaser);
        }
        else
        {
            lineRenderer.SetPosition(1, rayOrigin + (poste.transform.forward * range));
        }
    }
    
    public void RotateTo(float degrees)
    {
        targetRotationY = degrees;
        isRotating = true;
    }
}
