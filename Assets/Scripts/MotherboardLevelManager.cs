using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotherboardLevelManager : MonoBehaviour
{
    [SerializeField] private PlaceObjects ramSlot; // Array of objects to place
    [SerializeField] private PlaceObjects[] levelFilesPlaces;
    [SerializeField] private PickupObjects motherboardFile;
    [SerializeField] private PlaceObjects cmosBattery;

    [SerializeField] private GameObject[] placeholders; 
    [SerializeField] public float lerpDuration = 2f;     // Duration for the lerp
    private Vector3 originalPosition = new Vector3(3.5f, 0, 1);

    private bool levelComplete = false;
    private bool ramPlaced = false;
    private bool batteryPlaced = false;

    private void Start()
    {

    }


    private void Update()
    {
        if (!ramPlaced) CheckRAMSlotState();
        if (!levelComplete) CheckLevelStatus();
        if (!batteryPlaced)
        {
            CMOSBatteryCheck();
        }
        else { Debug.Log("Battery is placed"); }
    }

        private void CheckLevelStatus()
    {
        if (ramPlaced && !levelComplete)
        {
            for (int i = 0; i < levelFilesPlaces.Length; i++)
            {
                if (!levelFilesPlaces[i].IsObjectPlaced())
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

    private void CMOSBatteryCheck()
    {
        if (cmosBattery.IsObjectPlaced() && !batteryPlaced)
        {
            batteryPlaced = true;
            ResetObjectsToOrigin();

        }
    }

    public void ResetObjectsToOrigin()
    {
        StartCoroutine(ResetToOriginCoroutine());
    }

    private IEnumerator ResetToOriginCoroutine()
    {
        float elapsedTime = 0f;

        // Save the current positions of the objects
        Vector3[] startPositions = new Vector3[placeholders.Length];
        for (int i = 0; i < placeholders.Length; i++)
        {
            startPositions[i] = placeholders[i].transform.position;
        }

        while (elapsedTime < lerpDuration)
        {
            float progress = elapsedTime / lerpDuration;

            for (int i = 0; i < placeholders.Length; i++)
            {
                // Lerp each object towards its original position
                placeholders[i].transform.position = Vector3.Lerp(startPositions[i], originalPosition, progress);
            }

            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure all objects are set exactly at their original positions
        for (int i = 0; i < placeholders.Length; i++)
        {
            placeholders[i].transform.position = originalPosition;
        }
    }

    private void LevelCompleteStuff()
    {
        // Do stuff when level is complete
    }
}
