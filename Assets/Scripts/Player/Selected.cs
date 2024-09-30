using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
[RequireComponent(typeof(PlayerInput))]
public class Selected : MonoBehaviour
{
    [SerializeField]private float distanceMaterial;
    [SerializeField]private float distanceObstacle;
    [SerializeField]private float jumpforce;
    [SerializeField]private Camera camPlayer;
    [SerializeField] private GameObject puzzleCombo;
    [SerializeField] private GameObject puzzleKeypad;

    public Collider nameoftag;
    private float valueButton;
    private Rigidbody rb;
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
        GetName(other);
        if (GameManager.Instance.state == GameManager.State.InGame)
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
                        gameObject.GetComponent<Interactive>().RatateLaser();
                    }
                    break;
                case "DoorCombo":
                    if (valueButton == 1)
                    {
                        Debug.Log("combo");
                        GameManager.Instance.changeState(GameManager.State.InPuzzle);
                        puzzleCombo.SetActive(true);
                    }
                    break;
                case "DoorKeypad":
                    if (valueButton == 1)
                    {
                        Debug.Log("keypad");
                        GameManager.Instance.changeState(GameManager.State.InPuzzle);
                        puzzleKeypad.SetActive(true);
                    }
                    break;
                case "Pendulum":
                    if (valueButton == 1)
                    {
                        GameManager.Instance.changeState(GameManager.State.InPuzzle);
                        ComboManagerTrampas.Instance.ActivateRandomObject();
 
                    }
                    break;
                case "Barbs":
                    if (valueButton == 1)
                    {
                        GameManager.Instance.changeState(GameManager.State.InPuzzle);
                        ComboManagerTrampas.Instance.ActivateRandomObject();
 
                    }
                    break;
                case "Bomb":
                    if (valueButton == 1)
                    {
                        GameManager.Instance.changeState(GameManager.State.InPuzzle);
                        ComboManagerTrampas.Instance.ActivateRandomObject();
 
                    }
                    break;
                case "Molotov":
                    if (valueButton == 1)
                    {
                        GameManager.Instance.changeState(GameManager.State.InPuzzle);
                        ComboManagerTrampas.Instance.ActivateRandomObject();
 
                    }
                    break;

            }
        }
    }
   
    public Collider GetName(Collider name)
    {
        nameoftag = name;
        return nameoftag;
    } 
  
}
