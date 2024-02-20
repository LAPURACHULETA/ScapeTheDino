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
        float horizontal = playerInput.actions["Move"].ReadValue<Vector2>().x;
        float vertical = playerInput.actions["Move"].ReadValue<Vector2>().y;

        float angle = Vector3.Angle(transform.forward, Vector3.forward); 

        Vector3 Mov = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime; 
        Vector3 newForw = Quaternion.AngleAxis(angle, Vector3.up) * Mov; 
        transform.Translate(newForw, Space.World); 
        rb.AddForce(newForw);
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
