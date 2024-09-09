using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckComboTampas : MonoBehaviour
{
    [SerializeField] private List<InputActionReference> comboActions; // Lista de acciones de entrada para cada botón del combo
    [SerializeField] private float comboTimeout; // Tiempo límite para realizar el combo en segundos
    [SerializeField] private GameObject objCombo;
    [SerializeField] private InputActionReference anyKeyAction;

    private List<InputAction> currentCombo;
    private Coroutine comboTimeoutCoroutine;
    
    GameManager gameManager;
    ComboManagerTrampas comboManager;
    Interactive interactive;
    Selected selected;
    private void Start()
    {
        currentCombo = new List<InputAction>();
        gameManager = FindObjectOfType<GameManager>();
        comboManager = FindObjectOfType<ComboManagerTrampas>();
        interactive=FindObjectOfType<Interactive>();        
        selected = FindObjectOfType<Selected>();
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
            objCombo.SetActive(false);
            checkedTrampActivated();
            // combo exitoso
            gameManager.changeState(GameManager.State.InGame);
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
        gameManager.changeState(GameManager.State.InGame);
    }

    private IEnumerator ComboTimeoutRoutine()
    {
        yield return new WaitForSeconds(comboTimeout);
        Debug.Log("tiempo terminado!");
        ResetCombo();

    }
    private void checkedTrampActivated()
    {
        if (interactive.nameOfTrampa == "Pendulum")
        {
            comboManager.changeState(ComboManagerTrampas.State.Pendulum);
            return;
        }
        if (interactive.nameOfTrampa == "Molotov")
        {
            comboManager.changeState(ComboManagerTrampas.State.Molotov);
            return;
        }
        if (interactive.nameOfTrampa == "Bomb")
        {
            comboManager.changeState(ComboManagerTrampas.State.Bomb);
            return;
        }
        if (interactive.nameOfTrampa == "Barbs")
        {
            comboManager.changeState(ComboManagerTrampas.State.Barbs);
            return;
        }
    }
}
