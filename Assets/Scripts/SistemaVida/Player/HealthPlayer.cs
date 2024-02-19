using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthPlayer : MonoBehaviour
{
    [Header("Vida de Jugador")]
    [Space(10)]
    [SerializeField]private Image imagenSangre;
    [SerializeField]private float vida;
    HealthManager healthManager;

    private float vidaActual;

    private float r;
    private float g;
    private float b;
    private float a;

    // Update is called once per frame
    private void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
        vidaActual = vida;
        r = imagenSangre.color.r;
        g = imagenSangre.color.g;
        b = imagenSangre.color.b;
        a = imagenSangre.color.a;

    }
    private void Update()
    {
        a -= 0.01f;
        a = Mathf.Clamp(a, 0, 1f);
        ChangeColor();
        MyHelath();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            healthManager.TakeDamagePlayer(5,vida,vidaActual);
            a += 1f;
        }
    }
    private void MyHelath()
    {
        if(vidaActual <= 0)
        {
            healthManager.Die();
        }
    }
    private void ChangeColor()
    {
        Color c = new Color(r, g, b, a);
        imagenSangre.color = c;
    }
}
