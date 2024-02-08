using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<string> items = new List<string>();

    public void AddItem(string item)
    {
        items.Add(item);
    }

    public bool HasItem(string item)
    {
        return items.Contains(item);
    }

    public void UseItem(string item)
    {
        if (HasItem(item))
        {
            // Implement logic for using the item
            items.Remove(item);
        }
        else
        {
            Debug.Log("Item not found in inventory: " + item);
        }
    }
}
