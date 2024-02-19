using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Moving Player")]
    [Space(10)]
    [SerializeField] protected float speed;  
    [SerializeField] protected float timeCanRunPlayer, recoveryTime;
    [SerializeField] protected bool canRunPlayer;

    private float maxSpeed;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Vector2 input;
    private float runValue;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        maxSpeed = speed;
        canRunPlayer = false;
    }
    private void FixedUpdate()
    {
        Movimiento();
        Recuperacion();
    }
    private void Recuperacion()
    {
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

    /// <summary>
    /// movimiento del personaje
    /// </summary>
    private void Movimiento()
    {
        //input = playerInput.actions["Move"].ReadValue<Vector2>();
        //rb.AddForce(new Vector3(input.x, 0f, input.y) * speed);
        //if (input.x <= 0 || input.y <= 0)
        //{
        //    rb.AddForce(new Vector3(0, 0, 0));
        //}
        float horizontal = playerInput.actions["Move"].ReadValue<Vector2>().x;
        float vertical = playerInput.actions["Move"].ReadValue<Vector2>().y;
        rb.AddForce(new Vector3(horizontal * speed/*agent.m_maxVel**/, 0f, vertical * speed /*agent.m_maxVel*/));

    }

    public void OnRun(InputValue context)
    {
        runValue = context.Get<float>();
        if (runValue == 1 && !canRunPlayer)
        {
            speed += 5;
            Invoke("StopToRun", 2f);
            canRunPlayer = true;
            runValue = 0;
        }
    }
  
    private void StopToRun()
    {
        speed = maxSpeed;
    }
}
