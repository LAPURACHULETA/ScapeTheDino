using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Combos : MonoBehaviour
{
    [SerializeField] private List<InputActionReference> comboActions; // Lista de acciones de entrada para cada botón del combo
    [SerializeField] private float comboTimeout; // Tiempo límite para realizar el combo en segundos
    [SerializeField] private GameObject objCombo;
    [SerializeField] private GameObject doorCombo;
    [SerializeField] private InputActionReference anyKeyAction;

    private List<InputAction> currentCombo;
    private Coroutine comboTimeoutCoroutine;
    private bool puzzleComplete;
    GameManager gameManager;
    private void Start()
    {
        currentCombo = new List<InputAction>();
        gameManager=FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        foreach (var actionReference in comboActions)
        {
            actionReference.action.performed += CheckCombo;
            actionReference.action.Enable();
        }
        if (anyKeyAction != null)
        {
            anyKeyAction.action.performed += CheckAnyKey;
            anyKeyAction.action.Enable();
        }

    }

    private void OnDisable()
    {
        foreach (var actionReference in comboActions)
        {
            actionReference.action.performed -= CheckCombo;
            actionReference.action.Disable();
        }

        if (anyKeyAction != null)
        {
            anyKeyAction.action.performed -= CheckAnyKey;
            anyKeyAction.action.Disable();
        }
    }
    private void Update()
    {
        comboTimeoutCoroutine = StartCoroutine(ComboTimeoutRoutine());
       
    }
    private void CheckAnyKey(InputAction.CallbackContext input)
    {
        // Verificar si la acción es parte del combo
        bool isValidAction = comboActions.Any(actionReference => actionReference.action == input.action);

        if (!isValidAction)
        {
            Debug.Log("¡Tecla incorrecta!");
            ResetCombo(); // Reiniciar el combo si se presionó una tecla incorrecta
        }
    }
    private void CheckCombo(InputAction.CallbackContext input)
    {
        currentCombo.Add(input.action);
        if (currentCombo.Count > comboActions.Count)
        {
            currentCombo.RemoveAt(0); // Mantener el tamaño del combo dentro de los límites
        } 
        if (CheckComboMatch())
        {
            Debug.Log("Combo exitoso!");
            Destroy(doorCombo);
            puzzleComplete = true;
            objCombo.SetActive(false);

            // combo exitoso
            gameManager.changeState(GameManager.State.InGame);
            ResetCombo();
        }
        //else
        //{
        //    // Si no se detecta un combo, se apaga
        //    //gameManager.changeState(GameManager.State.InGame);
        //    //if (comboTimeoutCoroutine != null)
        //    //{
        //    //    StopCoroutine(comboTimeoutCoroutine);
        //    //}
        //    //comboTimeoutCoroutine = StartCoroutine(ComboTimeoutRoutine());
        //}
    }

    private bool CheckComboMatch()
    {
        if (currentCombo.Count != comboActions.Count)
        {
            return false;
        }
        
        for (int i = 0; i < comboActions.Count; i++)
        {
            if (currentCombo[i] != comboActions[i].action)
            {
                return false;
            }
        }

        return true;
    }
    private void ResetCombo()
    {
        currentCombo.Clear();
        if (comboTimeoutCoroutine != null)
        {
            StopCoroutine(comboTimeoutCoroutine);
        }
        objCombo.SetActive(false);
        gameManager.changeState(GameManager.State.InGame);
    }
   
    private IEnumerator ComboTimeoutRoutine()
    {
        yield return new WaitForSeconds(comboTimeout);
        Debug.Log("tiempo terminado!");
        ResetCombo();
       
    }
}

