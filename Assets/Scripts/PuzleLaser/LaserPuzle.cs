using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class LaserPuzle : MonoBehaviour
{
    public static LaserPuzle Instance;

    [Header("Laser Settings")]
    public Transform laserOrigin; // Origen del l�ser
    public float range = 50f; // Rango del l�ser
    public LineRenderer lineRenderer;

    [Header("Rotation Settings")]
    public float rotationSpeed = 10f; // Velocidad de rotaci�n
    public float targetRotationY; // �ngulo objetivo
    public bool isRotating; // Si est� rotando actualmente

    [Header("Puzzle Settings")]
    public bool puzzleComplete; // Si el puzzle est� completo
    private List<GameObject> connectedTorres = new List<GameObject>(); // Torres conectadas

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        lineRenderer = GetComponent<LineRenderer>();
        isRotating = false;
    }

    private void FixedUpdate()
    {
        // Manejar la rotaci�n
        RotationTower();

        // Actualizar las conexiones del l�ser
        UpdateLaserConnections();

        // Verificar si todas las torres est�n conectadas
        puzzleComplete = CheckAllTowersConnected();

        if (puzzleComplete)
        {
            Destroy(ListOfTowers.Instance.doorLaser);
        }
    }

    /// <summary>
    /// Actualiza las conexiones del l�ser y verifica las torres conectadas.
    /// </summary>
    private void UpdateLaserConnections()
    {
        // Restablecer la lista de torres conectadas
        connectedTorres.Clear();

        lineRenderer.SetPosition(0, laserOrigin.position);
        Vector3 rayOrigin = laserOrigin.position;

        RaycastHit hit;

        // Emitir el rayo
        if (Physics.Raycast(rayOrigin, transform.forward, out hit, range))
        {
            lineRenderer.SetPosition(1, hit.point);

            // Si impacta una torre, a�adirla a la lista
            if (hit.collider.CompareTag("Torre"))
            {
                GameObject torre = hit.collider.gameObject;

                if (!connectedTorres.Contains(torre))
                {
                    connectedTorres.Add(torre);
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(1, rayOrigin + (transform.forward * range));
        }
    }

    /// <summary>
    /// Verifica si todas las torres est�n conectadas.
    /// </summary>
    private bool CheckAllTowersConnected()
    {
        List<GameObject> allTorres = ListOfTowers.Instance.SetListTorres();

        // Verifica que cada torre est� conectada
        foreach (GameObject torre in allTorres)
        {
            if (!connectedTorres.Contains(torre))
            {
                return false; // Una torre no est� conectada
            }
        }
        return true; // Todas las torres est�n conectadas
    }

    /// <summary>
    /// Maneja la rotaci�n del objeto hacia un �ngulo objetivo.
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
        }
    }

    /// <summary>
    /// Inicia una rotaci�n hacia el �ngulo especificado.
    /// </summary>
    public void RotateTo(float degrees)
    {
        targetRotationY = degrees;
        isRotating = true;
    }
}
