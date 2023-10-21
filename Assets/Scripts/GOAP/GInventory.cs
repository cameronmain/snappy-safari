using System.Collections.Generic;
using UnityEngine;

public class GInventory {
    public List<GameObject> items = new List<GameObject>();

    public void AddItem(GameObject i) {

        items.Add(i);
    }

    public GameObject FindItemWithTag(string tag) {

        foreach (GameObject i in items) {

            if (i == null) break;
            // match
            if (i.tag == tag) {

                return i;
            }
        }
        return null;
    }

    public void RemoveItem(GameObject i) {

        int indexToRemove = -1;

        // iterate to check objects existence 
        foreach (GameObject g in items) {

            indexToRemove++;
            // it found then break
            if (g == i) {
                break;
            }
        }
        // if more items to remove
        if (indexToRemove >= -1) {

            // remove the item with this index
            items.RemoveAt(indexToRemove);
        }
    }
}
