using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [SerializeField]Animator animator;
    [Header("Moving Player")]
    [Space(10)] 
    [SerializeField] protected float timeCanRunPlayer, recoveryTime,speed;
    protected bool canRunPlayer;
    private float maxSpeed;
    BasicAgent agent;
    public Vector3 movement;
    ChangeCameraPerson changeCameraPerson;
    private Rigidbody rb;
    private PlayerInput playerInput;
    //private float runValue;
    private float valueButton;
    public float horizontal;
    public float vertical;
    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {

        }
    }
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
         horizontal = playerInput.actions["Move"].ReadValue<Vector2>().x;
         vertical = playerInput.actions["Move"].ReadValue<Vector2>().y;

        var movement2 = new Vector3(horizontal, 0, vertical).normalized;
        GetMove(movement2); 
        if (GameManager.Instance.state == GameManager.State.InGame)
        {
            transform.Translate(movement2 * speed * Time.deltaTime, Space.World);
        }
        // Controlar animaciones
        if (movement2.magnitude > 0.01f) 
        {
            animator.SetBool("Run", true);
            animator.SetBool("Drop", false);
        }
        if (movement2.magnitude == 0f)
        {
            animator.SetBool("Run", false);

        }
        if (!changeCameraPerson.firtsPerson.activeInHierarchy && GameManager.Instance.state == GameManager.State.InGame)
        {
            if (movement2.magnitude > 0.01f) 
            {
                var targetRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); 
            }
        }

    }
    public Vector3 GetMove(Vector3 name)
    {
        movement = name;
        return movement;
    }

    public Vector3 SetMove()
    {
        //Debug.Log(listTorres);
        return movement;
    }
    //public void OnRun(InputValue context)
    //{
    //    runValue = context.Get<float>();
    //    if (runValue == 1 && !canRunPlayer)
    //    {
    //        agent.m_speed += 5;
    //        Invoke("StopToRun", 2f);
    //        canRunPlayer = true;
    //        runValue = 0;
    //    }
    //}

    public void OnPause(InputValue context)
    {
        valueButton = context.Get<float>();
        Debug.Log(valueButton);
        if (valueButton>0)
        {
            Debug.Log("pausa");

            GameManager.Instance.changeState(GameManager.State.Pause);
            
        }
    }

    //private void StopToRun()
    //{
    //    agent.m_speed = maxSpeed;
    ////}
}
