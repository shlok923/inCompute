using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPULevelManager : MonoBehaviour
{

    [SerializeField] private Crystal[] crystals; // Array of crystal requirements
    [SerializeField] private PickupObjects GPULevelFile;

    private bool levelComplete = false; // Tracks if the level is complete

    private void Update()
    {
        if (levelComplete) return; // Skip checking if the level is already complete

        CheckLevelStatus();
    }

    private void CheckLevelStatus()
    {
        foreach (Crystal crystal in crystals)
        {
            if (crystal.CurrentMaterial.color == crystal.RequiredMaterial.color)
            {
                Debug.Log(crystal.gameObject.name + " required " + crystal.RequiredMaterial.name + " and got " + crystal.CurrentMaterial.name);
                continue;
            }

            else
            {
                Debug.Log(crystal.gameObject.name + " required " + crystal.RequiredMaterial.name + " but got " + crystal.CurrentMaterial.name);
                levelComplete = false;
                return;
            }
        }

        // If all crystals meet their requirements
        levelComplete = true;
        GPULevelFile.gameObject.SetActive(true);
        Debug.Log("Level complete! All crystals have the correct colors.");
    }
}
