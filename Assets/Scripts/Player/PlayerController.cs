using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Moving Player")]
    [Space(10)] 
    [SerializeField] protected float timeCanRunPlayer, recoveryTime;
    protected bool canRunPlayer;


    private float maxSpeed;
    BasicAgent agent;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Vector2 input;
    private float runValue;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<BasicAgent>();
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        maxSpeed = agent.m_speed;
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
        input = playerInput.actions["Move"].ReadValue<Vector2>();

        //float horizontal = playerInput.actions["Move"].ReadValue<Vector2>().x;
        //float vertical = playerInput.actions["Move"].ReadValue<Vector2>().y;

        float angle = Vector3.Angle(transform.forward, Vector3.forward); 

        Vector3 Mov = new Vector3(input.x, 0, input.y) * /*agent.m_speed*/10 * Time.deltaTime; 
        Vector3 newForw = Quaternion.AngleAxis(angle, Vector3.up) * Mov; 
        transform.Translate(newForw, Space.World);
        //rb.velocity = newForw;
        rb.AddForce(newForw);
    }

    public void OnRun(InputValue context)
    {
        runValue = context.Get<float>();
        if (runValue == 1 && !canRunPlayer)
        {
            agent.m_speed += 5;
            Invoke("StopToRun", 2f);
            canRunPlayer = true;
            runValue = 0;
        }
    }
  
    private void StopToRun()
    {
        agent.m_speed = maxSpeed;
    }
}
