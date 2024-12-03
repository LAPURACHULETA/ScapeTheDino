using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
[RequireComponent(typeof(PlayerInput))]
public class Selected : MonoBehaviour
{
    public static Selected Instance { get; private set; }
    [SerializeField] Animator animator;
    [SerializeField]private float distanceMaterial;
    [SerializeField]private float distanceObstacle;
    [SerializeField]private float jumpforce;
    [SerializeField]private Camera camPlayer;
    [SerializeField] private GameObject puzzleCombo;
    [SerializeField] private GameObject puzzleKeypad;

    public Collider nameoftag;
    private float valueButton;
    private GameObject trampasSeleccionada;
    private Rigidbody rb;
    private void Awake()
    {
        // Implementación Singleton
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            ///Destroy(gameObject); // Si ya existe una instancia, destruye este objeto.
        }
    }
    // Update is called once per frame
    private void Start()
    {
        rb = GetComponent<Rigidbody>();     
       
    }
    public void OnInteractive(InputValue context)
    {
        valueButton = context.Get<float>();
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Seleccted  "+SpawnManagerLevel.Instance.SetinBlattle());
        GetName(other);
        if(SpawnManagerLevel.Instance.SetinBlattle()==false && GameManager.Instance.state == GameManager.State.InGame)
        {
            switch (other.tag)
            {
                case "Obstacle":
                    if (valueButton == 1)
                    {
                        rb.AddForce(Vector3.up * jumpforce);
                    }
                    
                    break;
                case "Torre":
                    if (valueButton == 1)
                    {
                        TrueDrop();
                        other.gameObject.GetComponent<Interactive>().RatateLaser();
                    }
                    
                    break;
                case "DoorCombo":
                    if (valueButton == 1)
                    {
                        TrueDrop();
                        Debug.Log("combo");
                        GameManager.Instance.changeState(GameManager.State.InPuzzle);
                        puzzleCombo.SetActive(true);
                    }
                   
                    break;
                case "DoorKeypad":
                    if (valueButton == 1)
                    {
                        TrueDrop();
                        ///Debug.Log("keypad");
                        GameManager.Instance.changeState(GameManager.State.InPuzzle);
                        puzzleKeypad.SetActive(true);
                    }
               
                    break;
            }
        }
        else
        {
              
        }
        if (GameManager.Instance.state == GameManager.State.InGame)
        {

            switch (other.tag)
            {
          
                case "Pendulum":
                    if (valueButton == 1)
                    {
                        SetTrampa(other.transform.gameObject);
                        TrueDrop();
                        GameManager.Instance.changeState(GameManager.State.InPuzzle);
                        ComboManagerTrampas.Instance.ActivateRandomObject();
                    }
                    
                    break;
                case "Barbs":
                    if (valueButton == 1)
                    {
                        TrueDrop();
                        GameManager.Instance.changeState(GameManager.State.InPuzzle);
                        ComboManagerTrampas.Instance.ActivateRandomObject();
                        SetTrampa(other.transform.gameObject);
                    }
                    
                    break;
                case "Bomb":
                    if (valueButton == 1)
                    {
                        TrueDrop();
                        GameManager.Instance.changeState(GameManager.State.InPuzzle);
                        ComboManagerTrampas.Instance.ActivateRandomObject();
                        SetTrampa(other.transform.gameObject);
                    }
                    
                    break;
                case "Molotov":
                    if (valueButton == 1)
                    {
                        TrueDrop();
                        GameManager.Instance.changeState(GameManager.State.InPuzzle);
                        ComboManagerTrampas.Instance.ActivateRandomObject();
                        SetTrampa(other.transform.gameObject);
                    }
                 
                    break;
            }
        }
        else
        {
            
        }
    }
    public GameObject SetTrampa(GameObject name)
    {
        trampasSeleccionada = name;
        return trampasSeleccionada;
    }

    public GameObject GetTrampa()
    {
        //Debug.Log(listTorres);
        return trampasSeleccionada;
    }
    public void TrueDrop()
    {
        animator.SetBool("Drop", true);
    }
    public Collider GetName(Collider name)
    {
        nameoftag = name;
       // Debug.Log(nameoftag);
        return nameoftag;
    } 
    public Collider SetName()
    {
        //Debug.Log(nameoftag);
        return nameoftag;
        
    }
}
