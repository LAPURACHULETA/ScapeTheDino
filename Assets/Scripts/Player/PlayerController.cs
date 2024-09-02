using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Moving Player")]
    [Space(10)] 
    [SerializeField] protected float timeCanRunPlayer, recoveryTime,speed;
    protected bool canRunPlayer;
    private float maxSpeed;
    BasicAgent agent;
    GameManager gameManager;
    ChangeCameraPerson changeCameraPerson;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private float runValue;
    private float valueButton;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        changeCameraPerson=FindObjectOfType<ChangeCameraPerson>();
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
        float horizontal = playerInput.actions["Move"].ReadValue<Vector2>().x;
        float vertical = playerInput.actions["Move"].ReadValue<Vector2>().y;

        // Crear un vector de movimiento basado en la entrada
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized; // Normalizar para evitar movimiento más rápido en diagonales

        // Si hay movimiento, calcular la nueva rotación
        if (!changeCameraPerson.firtsPerson.activeInHierarchy&& gameManager.state == GameManager.State.InGame)
        { 
            if (movement.magnitude > 0.1f)
            {
                // Calcular la rotación deseada
                Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
            }
        }

        // Calcular el movimiento
        Vector3 movementWithSpeed = movement * speed * Time.deltaTime;

        if (gameManager.state == GameManager.State.InGame)
        {
            // Mover el personaje
            transform.Translate(movementWithSpeed, Space.World);
            rb.AddForce(movementWithSpeed, ForceMode.VelocityChange);
        }
        
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

    public void OnPause(InputValue context)
    {
        valueButton = context.Get<float>();
        if (valueButton==1)
        {
            Debug.Log("pausa");
            gameManager.ButtonPause();
            
        }
    }

    private void StopToRun()
    {
        agent.m_speed = maxSpeed;
    }
}
