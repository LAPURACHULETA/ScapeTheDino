using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]Animator animator;
    [Header("Moving Player")]
    [Space(10)] 
    [SerializeField] protected float timeCanRunPlayer, recoveryTime,speed;
    protected bool canRunPlayer;
    private float maxSpeed;
    BasicAgent agent;
   
    ChangeCameraPerson changeCameraPerson;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private float runValue;
    private float valueButton;

    // Start is called before the first frame update
    void Start()
    {
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

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;
       
        // Controlar animaciones
        if (movement.magnitude > 0.01f) // Activar más rápido la animación
        {
            animator.SetBool("Run", true);
            animator.SetBool("Drop", false);
        }
        if (movement.magnitude == 0f)
        {
            animator.SetBool("Run", false);

        }
        if (!changeCameraPerson.firtsPerson.activeInHierarchy && GameManager.Instance.state == GameManager.State.InGame)
        {
            if (movement.magnitude > 0.01f) 
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); 
            }
        }

        if (GameManager.Instance.state == GameManager.State.InGame)
        {
            transform.Translate(movement * speed * Time.deltaTime, Space.World);
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
            GameManager.Instance.ButtonPause();
            
        }
    }

    private void StopToRun()
    {
        agent.m_speed = maxSpeed;
    }
}
