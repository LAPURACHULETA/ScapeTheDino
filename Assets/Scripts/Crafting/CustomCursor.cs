using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomCursor : MonoBehaviour
{
    private bool valueButton;
    public bool canMover;
    public Vector2 mousePos;
    public void MouseClick(InputValue context)
    {
        valueButton = context.Get<bool>();
    }
    private void Awake()
    {

        var mouse = Mouse.current.position;
        //Debug.Log(mouse.ReadValue());
        if(valueButton)
        {
            transform.position = mouse.ReadValue();
            
        }  
    }
    private void Update()
    {
        var mouse = Mouse.current.position;
        //Debug.Log(mouse.ReadValue());
        if (valueButton||canMover)
        {
            transform.position = mouse.ReadValue();
            
        }
    }
    public void MovementMouse()
    {
        canMover = true;
    }
    public void noMouse()
    {
        canMover = false;
    }
}
