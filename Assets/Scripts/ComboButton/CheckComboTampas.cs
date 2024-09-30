using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckComboTampas : MonoBehaviour
{
    public static CheckComboTampas Instance { get; private set; }
    [SerializeField] private List<InputActionReference> comboActions; // Lista de acciones de entrada para cada bot�n del combo
    [SerializeField] private float comboTimeout; // Tiempo l�mite para realizar el combo en segundos
    [SerializeField] private GameObject objCombo;
    [SerializeField] private InputActionReference anyKeyAction;

    private List<InputAction> currentCombo;
    private Coroutine comboTimeoutCoroutine;
    public bool trampActivate;
  
    Interactive interactive;
    Selected selected;

    private void Awake()
    {
        // Implementaci�n Singleton
        if (Instance == null)
        {
            Instance = this;
           
        }
        else
        {
            //Destroy(gameObject); // Si ya existe una instancia, destruye este objeto.
        }
    }
    private void Start()
    {
        currentCombo = new List<InputAction>();
        interactive=FindObjectOfType<Interactive>();        
        selected = FindObjectOfType<Selected>();
        trampActivate = false;
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
        // Verificar si la acci�n es parte del combo
        bool isValidAction = comboActions.Any(actionReference => actionReference.action.name == input.action.name);

        if (!isValidAction)
        {
            Debug.Log("Tecla incorrecta!");
            ResetCombo(); // Reiniciar el combo si se presion� una tecla incorrecta
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
            currentCombo.RemoveAt(0); // Mantener el tama�o del combo dentro de los l�mites
        }
        if (CheckComboMatch())
        {
            Debug.Log("Combo exitoso!");
            objCombo.SetActive(false);
            checkedTrampActivated(selected.SetName());
            // combo exitoso
            //GameManager.Instance.changeState(GameManager.State.InGame);
            ResetCombo();
        }

        if (anyKeyAction != null)
        {
            anyKeyAction.action.Enable();
        }
    }
    private void checkedTrampActivated(Collider other)
    {
        Debug.Log(other);
        switch (other.tag)
        {
            case "Pendulum":
                ComboManagerTrampas.Instance.changeState(ComboManagerTrampas.State.Pendulum); 
                break;
            case "Molotov":
                ComboManagerTrampas.Instance.changeState(ComboManagerTrampas.State.Molotov);
            
                break;
            case "Bomb":
                ComboManagerTrampas.Instance.changeState(ComboManagerTrampas.State.Bomb);
            
                break;
            case "Barbs":
                ComboManagerTrampas.Instance.changeState(ComboManagerTrampas.State.Barbs);
               
                break;
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
        GameManager.Instance.changeState(GameManager.State.InGame);
        currentCombo.Clear();
        if (comboTimeoutCoroutine != null)
        {
            StopCoroutine(comboTimeoutCoroutine);
        }
        objCombo.SetActive(false);
    }

    private IEnumerator ComboTimeoutRoutine()
    {
        yield return new WaitForSeconds(comboTimeout);
        Debug.Log("tiempo terminado!");
        ResetCombo();

    }

}
