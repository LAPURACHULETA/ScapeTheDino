using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactive : MonoBehaviour
{
    private Inventory inventory;
    public GameObject item;
    private void Start()
    {
        inventory=GameObject.FindGameObjectWithTag("Player")
            .GetComponent<Inventory>(); 
    }
    public void TakeObject()
    {
        
        for (int i = 0;i<inventory.slots.Length;i++) 
        {
            if (inventory.isFull[i] == false)
            {
                //agrega al inventario 
                inventory.isFull[i] = true;
                Instantiate(item, inventory.slots[i].transform,false);
                Destroy(gameObject);
                break;
            }
        }
    }
}
