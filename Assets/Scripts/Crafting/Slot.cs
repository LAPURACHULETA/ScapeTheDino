using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Item item;
    public int index;
    
    private Inventory inventory;
    private void Start()
    {
        inventory=GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }
    private void Update()
    {
        if(transform.childCount<=0)
        {
            inventory.isFull[index] = false;
        }
    }
    public void DropItem()
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<SpawnInventory>().SpawnDroppedItem();
            GameObject.Destroy(child.gameObject);
        }
    }
}
