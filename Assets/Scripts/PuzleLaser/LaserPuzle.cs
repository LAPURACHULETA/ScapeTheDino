using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class LaserPuzle : MonoBehaviour
{
    public static LaserPuzle Instance;

    [Header("Laser Settings")]
    public Transform laserOrigin; 
    public float range = 50f; 
    public LineRenderer lineRenderer;

    [Header("Rotation Settings")]
    public float rotationSpeed = 10f; 
    public float targetRotationY; 
    public bool isRotating; 

    [Header("Puzzle Settings")]
    public bool puzzleComplete; 
    private List<GameObject> connectedTorres = new List<GameObject>(); 
    
    bool connected;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        lineRenderer = GetComponent<LineRenderer>();
        isRotating = false;
        connected = false;
    }

    private void FixedUpdate()
    {
        
        // Manejar la rotación
        RotationTower();

        // Actualizar las conexiones del láser
        UpdateLaserConnections(); 


        puzzleComplete=CheckAllTowersConnected();
    }

    /// <summary>
    /// Actualiza las conexiones del láser y verifica las torres conectadas.
    /// </summary>
    private void UpdateLaserConnections()
    {
        if (!connected)
        {
            lineRenderer.SetPosition(0, laserOrigin.position);
            Vector3 rayOrigin = laserOrigin.position;

            RaycastHit hit;

            // Emitir el rayo
            if (Physics.Raycast(rayOrigin, transform.forward, out hit, range))
            {
                lineRenderer.SetPosition(1, hit.point);

                // Si impacta una torre, añadirla a la lista
                if (hit.collider.CompareTag("Torre"))
                {
                    GameObject torre = hit.collider.gameObject;

                    if (!connectedTorres.Contains(torre))
                    {
                        connectedTorres.Add(torre);
                        Debug.Log($"Torre conectada: {torre.name}");
                    }
                }
            }
            else
            {
                lineRenderer.SetPosition(1, rayOrigin + (transform.forward * range));
            }

        }
    }

    /// <summary>
    /// Verifica si todas las torres están conectadas.
    /// </summary>
    private bool CheckAllTowersConnected()
    {

        List<GameObject> allTorres = ListOfTowers.Instance.SetListTorres();

        foreach (GameObject torre in allTorres)
        {
            if (!connectedTorres.Contains(torre))
            {
                Debug.Log($"Torre no conectada: {torre.name}");
                return false; // Una torre no está conectada
            }
        }
        return true;
    }

    /// <summary>
    /// Maneja la rotación del objeto hacia un ángulo objetivo.
    /// </summary>
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
                targetRotationY = Mathf.Round((targetRotationY + 90f) % 360f);
            }
            isRotating = false;
        }
        if(!isRotating)
        {
            UpdateLaserConnections();
        }
    }

    /// <summary>
    /// Inicia una rotación hacia el ángulo especificado.
    /// </summary>
    public void RotateTo(float degrees)
    {
        targetRotationY = degrees;
        isRotating = true;
    }
}
