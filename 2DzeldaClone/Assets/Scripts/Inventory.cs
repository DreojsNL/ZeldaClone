using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> items;  // List to hold the inventory items
    private int selectedItemIndex = -1;  // Index of the currently selected item

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Cycle to the next item in the inventory
            selectedItemIndex++;
            if (selectedItemIndex >= items.Count)
            {
                selectedItemIndex = 0;
            }

            // Activate the selected item and deactivate others
            UpdateSelectedItem();
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // Cycle to the previous item in the inventory
            selectedItemIndex--;
            if (selectedItemIndex < 0)
            {
                selectedItemIndex = items.Count - 1;
            }

            // Activate the selected item and deactivate others
            UpdateSelectedItem();
        }


    }

    void UpdateSelectedItem()
    {
        // Activate the selected item and deactivate others
        for (int i = 0; i < items.Count; i++)
        {
            if (i == selectedItemIndex)
            {
                items[i].SetActive(true);
            }
            else
            {
                items[i].SetActive(false);
            }
        }
    }
}
