using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private Vector2 input;
    [SerializeField] private bool canRunPlayer;
    private float timeRun;
    private float timeRestRun;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        canRunPlayer = false;
    }
    private void FixedUpdate()
    {
        /// <summary>
        /// mmovimient del personaje
        /// </summary>
        input = playerInput.actions["Move"].ReadValue<Vector2>();
        rb.AddForce(new Vector3(input.x, 0f, input.y) * speed);
        Debug.Log(speed);
        /// <summary>
        /// contro de correr del personaje
        /// </summary>
        if (canRunPlayer)
        {
            timeRun += Time.deltaTime;
        }
        if (timeRun >= 2)
        {
            canRunPlayer = false;
            speed -= 5f; 
            timeRun = 0;
        }
        if(!canRunPlayer)
        {
            timeRestRun += Time.deltaTime;
        }
        if(timeRestRun >= 2)
        {
            canRunPlayer=true;
            timeRestRun = 0;
        }
    }
    public void RunPlayer(InputAction.CallbackContext context)
    {
        if(context.performed && !canRunPlayer)
        {
            canRunPlayer = true;
            speed += 5;
        }
    }
}
