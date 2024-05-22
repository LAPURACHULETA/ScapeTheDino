using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> combos;
  
    public void ActivateRandomObject()
    {
        int randomIndex = Random.Range(0, combos.Count);
        GameObject randomObject = combos[randomIndex];
        randomObject.SetActive(true);
        
    }

}
