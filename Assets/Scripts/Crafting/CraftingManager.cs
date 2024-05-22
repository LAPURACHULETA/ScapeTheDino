using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    private Item currentItem;

    [SerializeField]private Image customCursor;
    [SerializeField] private Slot[] craftingSlots;
    [SerializeField] private List<Item> itemList;
    [SerializeField] private string[] recipes;
    [SerializeField] private Item[] recipesResults;
    [SerializeField] private Slot resultSlot;
    CustomCursor custom;
    private void Start()
    {
        custom=FindObjectOfType<CustomCursor>();
    }
    private void Update()
    {
        var mouse = Mouse.current.position;
        if (!custom.canMover)
        {
            if (currentItem != null)
            {
                customCursor.gameObject.SetActive(false);
                Slot nearstSlot = null;
                float shortestDistance = float.MaxValue;
                foreach (Slot slot in craftingSlots)
                {
             
                    var posMouse = mouse.ReadValue();

                    float dist = Vector2.Distance(posMouse, slot.transform.position);
                    if (dist < shortestDistance)
                    {
                        shortestDistance = dist;
                        nearstSlot = slot;
                    }
                }
                nearstSlot.gameObject.SetActive(true);
                nearstSlot.GetComponent<Image>().sprite = currentItem.GetComponent<Image>().sprite;
                nearstSlot.item = currentItem;
                itemList[nearstSlot.index] = currentItem;
                currentItem = null;
                CheckForCreatedRecipes();
            }
        }
        
    }
    private void CheckForCreatedRecipes()
    {
        resultSlot.gameObject.SetActive(false);
        resultSlot.item = null;

        string currentRecipeString = "";
        foreach(Item item in itemList)
        {
            if (item != null)
            {
                currentRecipeString += item.itemName;
            }
            else
            {
                currentRecipeString += "null";
            }
        }
        for(int i = 0; i < recipes.Length; i++) 
        {
            if (recipes[i] ==currentRecipeString)
            {
                resultSlot.gameObject.SetActive(true);
                resultSlot.GetComponent<Image>().sprite = recipesResults[i].GetComponent<Image>().sprite;
                resultSlot.item = recipesResults[i];
            }
        }
    }
    public void OnClickSlot(Slot slot)
    {
        slot.item= null;
        itemList[slot.index] = null;
        slot.gameObject.SetActive(false);
        CheckForCreatedRecipes();
    }
    public void OuMouseDownItem(Item item)
   {
        if (currentItem == null)
        {
            currentItem = item;
            customCursor.gameObject.SetActive(true);
            customCursor.sprite=currentItem.GetComponent<Image>().sprite;
        }
   }
}
