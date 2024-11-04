using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckComboPuzzle : MonoBehaviour
{
    public static CheckComboPuzzle Instance { get; private set; }

    [SerializeField] private List<InputActionReference> comboActions; // Lista de acciones de entrada para cada botón del combo
    [SerializeField] private float comboTimeout; // Tiempo límite para realizar el combo en segundos
    [SerializeField] private GameObject objCombo;
    [SerializeField] private GameObject doorCombo;
    [SerializeField] private InputActionReference anyKeyAction;
    [SerializeField] private string imA;

    private List<InputAction> currentCombo;
    private Coroutine comboTimeoutCoroutine;
    public bool puzzleComplete;

    private void Awake()
    {
        // Implementación Singleton
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            ///Destroy(gameObject); // Si ya existe una instancia, destruye este objeto.
        }
    }
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
        bool isValidAction = comboActions.Any(actionReference => actionReference.action.name == input.action.name);

        if (!isValidAction)
        {
            Debug.Log("¡Tecla incorrecta!");
            ResetCombo(); // Reiniciar el combo si se presionó una tecla incorrecta
        }
    }
    private void CheckCombo(InputAction.CallbackContext input)
    {
        if (anyKeyAction != null)
        {
            anyKeyAction.action.Disable();
        }

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
            GameManager.Instance.changeState(GameManager.State.InGame);
            ResetCombo();
        }

        if (anyKeyAction != null)
        {
            anyKeyAction.action.Enable();
        }
    }

    private bool CheckComboMatch()
    {
        if (currentCombo.Count != comboActions.Count)
        {
            return false;
        }

        // Comparar las acciones por su nombre
        for (int i = 0; i < comboActions.Count; i++)
        {
            if (currentCombo[i].name != comboActions[i].action.name)
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
        GameManager.Instance.changeState(GameManager.State.InGame);
    }
   
    private IEnumerator ComboTimeoutRoutine()
    {
        yield return new WaitForSeconds(comboTimeout);
        Debug.Log("tiempo terminado!");
        ResetCombo();
       
    }
    public bool GetCompletepuzzle(bool name)
    {
        puzzleComplete = name;
        // Debug.Log(nameoftag);
        return puzzleComplete;
    }
    public bool SetCompletePuzzles()
    {
        Debug.Log(puzzleComplete);
        return puzzleComplete;

    }
}

