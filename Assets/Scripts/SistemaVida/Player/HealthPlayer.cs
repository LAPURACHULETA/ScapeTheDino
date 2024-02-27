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

    BasicAgent basicAgent;
    private float r;
    private float g;
    private float b;
    private float a;

    // Update is called once per frame
    private void Start()
    {
        basicAgent=GetComponent<BasicAgent>();
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

    public void DamagePlayer()
    {
        basicAgent.TakeDamage(5);
        a += 1f;
    }
    private void MyHelath()
    {
        if(basicAgent.m_vida <= 0)
        {
            basicAgent.Die(this.gameObject);
        }
    }
    private void ChangeColor()
    {
        Color c = new Color(r, g, b, a);
        imagenSangre.color = c;
    }
}
