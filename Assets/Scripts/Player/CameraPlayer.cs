using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerInput))]
public class CameraPlayer : MonoBehaviour
{
    [Header("Camera")]
    [Space(10)]
    [SerializeField] private Vector2 look;
    [SerializeField] private Vector2 move;
    [SerializeField] private Vector2 nextPosition;
    [SerializeField] private Quaternion nextRotation;
    [SerializeField] private float rotationLerp;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject followTransform;
    private PlayerInput playerInput;
    [Header("Cambio de Vista")]
    [Space(10)]
    public float speed;
    PlayerController player;
    Cinemachine.CinemachineImpulseSource source;
    GameManager gameManager;
    private void Start()
    {
        player = GetComponent<PlayerController>();
        gameManager=FindObjectOfType<GameManager>();

        playerInput = GetComponent<PlayerInput>();
    }
    void Update()
    {
        if(gameManager.state == GameManager.State.InGame)
        {
            LookMouse();
        }
    }

    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }
    public void ChangePersonEfect()
    {
        source = GetComponent<Cinemachine.CinemachineImpulseSource>();
        /// <summary>
        /// direccion de avanze de la camara
        /// </summary>
        source.GenerateImpulse(Camera.main.transform.forward);
    }

    public void LookMouse()
    {
        
        /// <summary>
        /// rotacion del jugador 
        /// </summary>
        followTransform.transform.rotation *= Quaternion.AngleAxis(look.x * rotationSpeed, Vector3.up);
     
        followTransform.transform.rotation *= Quaternion.AngleAxis(look.y * -rotationSpeed, Vector3.right);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;
        var angle = followTransform.transform.localEulerAngles.x;
        Debug.Log(angle);
        Debug.Log(angles);
        /// <summary>
        /// anclamos la camara al objetivo para que este bloqueada en una rotacion vertical
        /// </summary>
        if (angle > 180 && angle < 340)
        {
            PlayerController.Instance.horizontal -= playerInput.actions["Move"].ReadValue<Vector2>().y;
            PlayerController.Instance.vertical -= playerInput.actions["Move"].ReadValue<Vector2>().x;
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }


        followTransform.transform.localEulerAngles = angles;

        nextRotation = Quaternion.Lerp(followTransform.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

        /// <summary>
        /// si el objeto esta en miviento o no calcula su posicion basada en su velociedad de direccion
        /// </summary>
        if (move.x == 0 && move.y == 0)
        {
            nextPosition = transform.position;
         
            if (GameManager.Instance.state == GameManager.State.InPuzzle)
            {
                ///<summary>
                ///Establecer la rotación del jugador basada en la transformación de la mirada
                ///</summary>
                transform.rotation = Quaternion.Euler(0, angles.y, 0);
                ///<summary>
                ///Restablecer la rotación en el eje Y de la transformación de la mirada
                ///</summary>
                followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
                
            }
           
            return;
        }
        float moveSpeed = speed / 100f;
        Vector3 position = (transform.forward * move.y * moveSpeed) + (transform.right * move.x * moveSpeed);
        nextPosition = transform.position + position;

        /// <summary>
        /// configuramos la rotacion en la rotacion local de la tranformacion del objetivo
        /// </summary>
        transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
        /// <summary>
        /// reseteamos la rotacion local de la tranformacion del seguimiento 
        /// </summary>
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
    }
}
