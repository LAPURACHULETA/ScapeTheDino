using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomCursor : MonoBehaviour
{
    private Vector2 valueButton;
    public void OnMouse(InputValue context)
    {
        valueButton = context.Get<Vector2>();
    }
    private void Awake()
    {
        var mouse = Mouse.current.position;
        //Debug.Log(mouse.ReadValue());
        transform.position = mouse.ReadValue();
    }
    private void Update()
    {
        //transform.position=valueButton;
        var mouse = Mouse.current.position;
        //Debug.Log(mouse.ReadValue());
        transform.position = mouse.ReadValue();
    }
}
