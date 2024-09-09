using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboManagerTrampas : MonoBehaviour
{
    [SerializeField] private List<GameObject> combos;
    [SerializeField] private GameObject pendulum;
    [SerializeField] private GameObject molotov;
    [SerializeField] private GameObject bomb;
    [SerializeField] private GameObject barbs;

    Interactive interactive;
    Selected selected;
    public void ActivateRandomObject()
    {
        int randomIndex = Random.Range(0, combos.Count);
        GameObject randomObject = combos[randomIndex];
        randomObject.SetActive(true);

    }
    public enum State
    {
        None,
        Pendulum,
        Molotov,
        Bomb,
        Barbs,
        
    }

    public State state;
    private void Start()
    {
      interactive=FindObjectOfType<Interactive>();
    }

    public void changeState(State newState)
    {
        if (newState == state)
        {
            return;
        }

        state = newState;

        switch (state)
        {
            case State.None:
                break;
            case State.Pendulum:
                Pendulum();
                break;
            case State.Molotov:
                Molotov();
                break;
            case State.Bomb:
                Bomb();
                break;
            case State.Barbs:
                Barbs();
                break;

        }

    }
    void Pendulum()
    {
        pendulum.SetActive(true);
    }
    void Molotov()
    {
        molotov.SetActive(true);
    }
    void Bomb()
    {
        bomb.SetActive(true);
    }
    void Barbs()
    {
        barbs.SetActive(true);
    }
   
}
