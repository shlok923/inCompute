using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotherboardLevelManager : MonoBehaviour
{
    [SerializeField] private PlaceObjects ramSlot; // Array of objects to place
    [SerializeField] private PlaceObjects[] levelFiles;
    [SerializeField] private PickupObjects motherboardFile;
    private bool levelComplete = false;
    private bool ramPlaced = false;

    private void Start()
    {
        
    }


    private void Update()
    {
        CheckRAMSlotState();
        if (!levelComplete) CheckLevelStatus();
    }

    private void CheckLevelStatus()
    {
        if (ramPlaced && !levelComplete)
        {
            for (int i = 0; i < levelFiles.Length; i++)
            {
                if (!levelFiles[i].IsObjectPlaced())
                {
                    return;
                }
            }
            levelComplete = true;
            LevelCompleteStuff();
            Debug.Log("Level complete! All objects have been placed.");
        }
    }
    
    private void CheckRAMSlotState()
    {
        //Debug.Log("Checking RAM slot state...");
        if (ramSlot.IsObjectPlaced() && motherboardFile != null)
        {
            Debug.Log("RAM has been placed, motherboard file is now available.");
            ramPlaced = true;
            motherboardFile.gameObject.SetActive(true);
        }
    }

    private void LevelCompleteStuff()
    {
        // Do stuff when level is complete
    }
}
