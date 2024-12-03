using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public List<GameObject> slots;
    private List<bool> occupied;

    private void Awake() {
        for (int i = 0; i < transform.childCount; i++) {
            slots[i] = transform.GetChild(i).gameObject;
            occupied[i] = false;
        }
    }

    public void AddElement(GameObject addedSlot) {
        for (int i = 0; i < slots.Count; i++) {
            if (!occupied[i]) {
                slots[i] = addedSlot;
                occupied[i] = true;
                return;
            }
        }
        Debug.Log("Inventory Space exceeded!");
    }
}
