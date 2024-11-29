using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class LaserPuzle : MonoBehaviour
{
    public static LaserPuzle Instance;

    [Header("Laser Settings")]
    public GameObject torre; 
    public Transform laserOrigin; 
    public float range = 50f; 
    public LineRenderer lineRenderer;

    [Header("Rotation Settings")]
    public float rotationSpeed = 10f; 
    public float targetRotationY;
    public bool isRotating;

    [Header("Puzzle Settings")]
    public bool puzzleComplete; 
    private bool offLaser;
  
    private List<GameObject> connectedTorres =new List<GameObject>(); 

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
           
        }

        lineRenderer = GetComponent<LineRenderer>();
        isRotating = false;
        offLaser = false;
    }

    private void FixedUpdate()
    {
        // Verificar rotación
        RotationTower();
        Laser();
        RemoveDisconnectedTowers();
        // Comprobar si todas las torres están conectadas
        puzzleComplete = CheckPuzzleComplete();

        // Depuración del estado del puzzle
        ///.Log($"Puzzle completo: {puzzleComplete}");

        // Destruir la puerta si el puzzle está completo
        if (puzzleComplete)
        {
            Destroy(ListOfTowers.Instance.doorLaser);
           // Debug.Log("¡Puzzle completado! Puerta destruida.");
        }
    }
    private void Laser()
    {
        
        // Dibujar línea inicial
        lineRenderer.SetPosition(0, gameObject.transform.position);
        Vector3 rayOrigin = transform.position;

        RaycastHit hit;
        // Emitir rayo
        if (Physics.Raycast(rayOrigin,transform.forward, out hit, range))
        {
            lineRenderer.SetPosition(1, hit.point);

            // Depurar detección de objetos
            //Debug.Log($"Rayo impactó con: {hit.collider.gameObject.name}");

            // Verificar si el rayo toca una torre
            if (gameObject.CompareTag("Torre")/*&& hit.point == rayEndPoint*/)
            {
                    
                GameObject torre = hit.collider.gameObject;

                // Agregar a la lista si no existe ya
                if (!connectedTorres.Contains(torre))
                {
                    connectedTorres.Add(torre);
                        
                    //Debug.Log("torres añadidas  "+connectedTorres);
                    //Debug.Log(ListOfTowers.Instance.SetListTorres().Count);
                    //Debug.Log($"Torre añadida: {torre.name}");
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(1, rayOrigin + (transform.forward * range));
            //Debug.Log("El rayo no impactó con nada.");
        }
        
       
    }
    private void RemoveDisconnectedTowers()
    {
        List<GameObject> currentlyConnected = new List<GameObject>();

        Vector3 rayOrigin = torre.transform.position;
        RaycastHit hit;

        foreach (GameObject torre in connectedTorres)
        {
            Vector3 direction = torre.transform.position - rayOrigin;

            if (Physics.Raycast(rayOrigin, direction.normalized, out hit, range))
            {
                if (hit.collider.gameObject == torre)
                {
                    currentlyConnected.Add(torre);
                }
            }
        }

        // Actualizar la lista
        connectedTorres = currentlyConnected;
    }
    private bool CheckPuzzleComplete()
    {
        foreach (GameObject torre in ListOfTowers.Instance.SetListTorres())
        {
            if (!connectedTorres.Contains(torre))
            {
                //Debug.Log($"Torre no conectada: {torre.name}");
                return false;
            }
            else
            {
                return true;
            }
        }
        return true;
    }

    private void RotationTower()
    {
        if (isRotating)
        {
            float currentY = Mathf.LerpAngle(transform.eulerAngles.y, targetRotationY, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentY, transform.eulerAngles.z);

            if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetRotationY)) < 0.1f)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetRotationY, transform.eulerAngles.z);
                isRotating = false;

                // Configurar el siguiente ángulo objetivo
                targetRotationY = Mathf.Round((targetRotationY + 90f) % 360f);
            }
        }
    }


    public void RotateTo(float degrees)
    {
        targetRotationY = degrees;
        isRotating = true;
    }

    public bool GetCompletepuzzle(bool name)
    {
        puzzleComplete = name;
        return puzzleComplete;
    }

    public bool SetCompletepuzzle()
    {
        Debug.Log(puzzleComplete);
        return puzzleComplete;
    }
}
