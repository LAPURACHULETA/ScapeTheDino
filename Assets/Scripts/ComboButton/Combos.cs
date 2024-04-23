using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Combos : MonoBehaviour
{
    [SerializeField] private List<InputActionReference> comboActions; // Lista de acciones de entrada para cada botón del combo
    [SerializeField] private float comboTimeout; // Tiempo límite para realizar el combo en segundos
    [SerializeField] private GameObject objCombo;

    private List<InputAction> currentCombo;
    private Coroutine comboTimeoutCoroutine;

    private void Start()
    {
        currentCombo = new List<InputAction>();
    }

    private void OnEnable()
    {
        foreach (var actionReference in comboActions)
        {
            actionReference.action.performed += CheckCombo;
            actionReference.action.Enable();
        }
    }

    private void OnDisable()
    {
        foreach (var actionReference in comboActions)
        {
            actionReference.action.performed -= CheckCombo;
            actionReference.action.Disable();
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
            objCombo.SetActive(false);
            // combo exitoso
            ResetCombo();
        }
        else
        {
            // Si no se detecta un combo, reinicia el temporizador
            if (comboTimeoutCoroutine != null)
            {
                StopCoroutine(comboTimeoutCoroutine);
            }
            comboTimeoutCoroutine = StartCoroutine(ComboTimeoutRoutine());
        }
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
    }

    private IEnumerator ComboTimeoutRoutine()
    {
        yield return new WaitForSeconds(comboTimeout);
        
        Debug.Log("tiempo terminado!");
        ResetCombo();
    }
}

