using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movimiento Player")]
    [Space(10)]
    [SerializeField] private float speed;
    [SerializeField] private float timeCanRunPlayer, recoveryTime;
    [SerializeField] private bool canRunPlayer;
    private float maxSpeed;

    [Header("Cambio de Vista")]
    [Space(10)]
    [SerializeField] private Transform firstPerson;
    [SerializeField] private Transform thirdPerson;
    [SerializeField] private Transform cameraPerson;
    [SerializeField] private bool canChangePerson;
    
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Vector2 input;
   
    // Start is called before the first frame update
    void Start()
    {

        cameraPerson.position = Vector3.Lerp(cameraPerson.position, thirdPerson.position, 5 * Time.deltaTime);
        cameraPerson.SetParent(thirdPerson);
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        maxSpeed = speed;
        canRunPlayer = false;
    }
    private void FixedUpdate()
    {
        /// <summary>
        /// movimiento del personaje
        /// </summary>
        input = playerInput.actions["Move"].ReadValue<Vector2>();
        rb.AddForce(new Vector3(input.x, 0f, input.y) * speed);
        /// <summary>
        /// control de recuperaion del personaje
        /// </summary>
        if (canRunPlayer)
        {
            timeCanRunPlayer += Time.deltaTime;
            if (timeCanRunPlayer > recoveryTime)
            {
                canRunPlayer = false;
                timeCanRunPlayer = 0;
            }
        }
        
    }
    public void RunPlayer(InputAction.CallbackContext context)
    {
        if(context.performed && !canRunPlayer)
        {
            speed += 5;
            Invoke("StopToRun", 2f);
            canRunPlayer = true;
        }
    }
    public void ChangePersone(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            canChangePerson = true;
        } 
        if (context.canceled)
        {
            canChangePerson = false;
        }
        if (canChangePerson)
        {
            cameraPerson.position = Vector3.Lerp(transform.position, firstPerson.position, 1000 * Time.deltaTime);
            cameraPerson.SetParent(firstPerson);
        }
        if (!canChangePerson)
        {
            cameraPerson.position = Vector3.Lerp(transform.position, thirdPerson.position, 1000 * Time.deltaTime);
            cameraPerson.SetParent(thirdPerson);
        }
    }
  
    private void StopToRun()
    {
        speed = maxSpeed;
    }
}
