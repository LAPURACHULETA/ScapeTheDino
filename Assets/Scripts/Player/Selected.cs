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

    public string nameoftag;
    private float valueButton;
    private Rigidbody rb;
    GameManager manager;
    ComboManagerTrampas comboManager;  
    // Update is called once per frame
    private void Start()
    {
        rb = GetComponent<Rigidbody>();     
        manager=FindObjectOfType<GameManager>();
        comboManager = FindObjectOfType<ComboManagerTrampas>();
    }
    void Update()
    {
        DetectionOfObject();
    }
    public void OnInteractive(InputValue context)
    {
        valueButton = context.Get<float>();
    }
    void DetectionOfObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distanceMaterial))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.black);
            GetName(hit.collider.tag);
            if (manager.state == GameManager.State.InGame)
            {
                if (hit.collider.tag == "Obstacle")
                {
                    if (valueButton == 1)
                    {
                        rb.AddForce(Vector3.up * jumpforce);
                    }
                }
                if (hit.collider.tag == "Torre")
                {
                    if (valueButton == 1)
                    {
                        hit.collider.gameObject.GetComponent<Interactive>().RatateLaser();
                    }
                }
                if (hit.collider.tag == "DoorCombo")
                {

                    if (valueButton == 1)
                    {
                        Debug.Log("combo");
                        manager.changeState(GameManager.State.InPuzzle);
                        puzzleCombo.SetActive(true);
                    }
                }
                if (hit.collider.tag == "DoorKeypad")
                {

                    if (valueButton == 1)
                    {
                        Debug.Log("keypad");
                        manager.changeState(GameManager.State.InPuzzle);
                        puzzleKeypad.SetActive(true);
                    }
                }

                if (hit.collider.tag == "Pendulum")
                {
                    if (valueButton == 1)
                    {
                        manager.changeState(GameManager.State.InPuzzle);
                        comboManager.ActivateRandomObject();
                    }
                }
            }
            else { }

        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
        }
    }
    string GetName(string name)
    {
        nameoftag = name;
        return nameoftag;
    } 
}
