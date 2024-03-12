using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    
    ShooterAgentStates agentStates;

    private int craft;
    private int game;
    void Start()
    {
       
    }
    /// <summary>
    /// Realiza operaciones de f?sica y l?gica de juego a una velocidad fija.
    /// </summary>
    private void FixedUpdate()
    {
        decisionManager();
    }

    /// <summary>
    /// Gestiona la percepci?n del agente y establece el objetivo.
    /// </summary>
   
    void decisionManager()
    {
        if (craft == 1)
        {
            agentStates = ShooterAgentStates.Crafting;
        }
        if (craft == 1)
        {
            agentStates = ShooterAgentStates.InGame;
        }
        actionManager();
    }
    void actionManager()
    {
        switch (agentStates)
        {
            case ShooterAgentStates.InGame:
                InGame();
                break;
            case ShooterAgentStates.Crafting:
                Crafting();
                break;
        }
    }
    private enum ShooterAgentStates
    {
        InGame,
        Crafting,
    }
    public void OnCrafting(InputValue context)
    {
        craft = context.Get<int>();
    }
    private void InGame()
    {
      
    }
    private void Crafting()
    {
        
    }
}
