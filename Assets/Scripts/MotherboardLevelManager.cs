using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotherboardLevelManager : MonoBehaviour
{
    [SerializeField] private PlaceObjects ramSlot; // Array of objects to place
    [SerializeField] private PickupObjects motherboardFile;
    [SerializeField] private PlaceObjects cmosBattery;
    [SerializeField] private PlaceObjects[] levelFilesPlaces; // Array of objects to place
    [SerializeField] private Material greenGlow;
    [SerializeField] private GameObject leftPartMainChip;
    [SerializeField] private GameObject rightPartMainChip;
    [SerializeField] private GameObject topPartMainChip;
    [SerializeField] private GameObject mainProcessor;
    [SerializeField] private Vector3 leftPartTargetPos = new Vector3(0, 0.270000011f, 0.373907089f);
    [SerializeField] private Vector3 rightPartTargetPos = new Vector3(0, 0.270000011f, 0.373907089f);
    [SerializeField] private Vector3 topPartTargetPos = new Vector3(3.58999991f, 24.2476387f, 0.873457432f);


    [SerializeField] private GameObject[] levelPlacesVisual;
    [SerializeField] private Material[] levelPlacesMaterials;

    //[SerializeField] private GameObject[] placeholders; 
    [SerializeField] public float lerpDuration = 2f;     // Duration for the lerp
    private Vector3 originalPosition = new Vector3(3.5f, 0, 1);

    private bool levelComplete = false;
    private bool ramPlaced = false;
    private bool batteryPlaced = false;
    public bool testing = false;
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
        if (testing) LevelCompleteStuff();
        //else { Debug.Log("Battery is placed"); }
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
            //StartCoroutine(SplitTheChip());
        }
    }

    private void CMOSBatteryCheck()
    {
        if (cmosBattery.IsObjectPlaced() && !batteryPlaced)
        {
            batteryPlaced = true;
            //ResetObjectsToOrigin();

            for (int i = 0; i < levelPlacesVisual.Length; i++)
            {
                levelPlacesVisual[i].GetComponent<Renderer>().material = levelPlacesMaterials[i];
            }

        }
    }

    private void LevelCompleteStuff()
    {
        leftPartMainChip.GetComponent<Renderer>().material = greenGlow;
        rightPartMainChip.GetComponent<Renderer>().material = greenGlow;
        mainProcessor.SetActive(false);
        StartCoroutine(SplitTheChip());
        StartCoroutine(LiftShuttle());
    }

    private IEnumerator SplitTheChip()
    {
        Debug.Log("SplitTheChip Coroutine Started");

        float elapsedTime = 0f;
        Vector3 leftPartStartPos = leftPartMainChip.transform.localPosition;
        Vector3 rightPartStartPos = rightPartMainChip.transform.localPosition;

        while (elapsedTime < lerpDuration)
        {
            float progress = elapsedTime / lerpDuration;

            // Smooth interpolation using Ease-In-Out
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            // Apply interpolated positions
            leftPartMainChip.transform.localPosition = Vector3.Lerp(leftPartStartPos, leftPartTargetPos, smoothProgress);
            rightPartMainChip.transform.localPosition = Vector3.Lerp(rightPartStartPos, rightPartTargetPos, smoothProgress);

            elapsedTime += Time.deltaTime; // Increment time
            yield return null; // Wait for next frame
        }

        // Ensure final positions are exactly the target positions
        leftPartMainChip.transform.localPosition = leftPartTargetPos;
        rightPartMainChip.transform.localPosition = rightPartTargetPos;

        Debug.Log("SplitTheChip Coroutine Finished");
    }

    private IEnumerator LiftShuttle()
    {

        yield return new WaitForSeconds(2f);
        float elapsedTime = 0f;
        float lerpDuration = 5f;

        while (elapsedTime < lerpDuration)
        {
            float progress = elapsedTime / lerpDuration;

            // Smooth interpolation using Ease-In-Out
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);
            topPartMainChip.transform.position = Vector3.Lerp(topPartMainChip.transform.position, topPartTargetPos, smoothProgress);

            elapsedTime += Time.deltaTime; // Increment time
            yield return null; // Wait for next frame
        }


        Debug.Log("SplitTheChip Coroutine Finished");
    }


    //public void ResetObjectsToOrigin()
    //{
    //    StartCoroutine(ResetToOriginCoroutine());
    //}

    //private IEnumerator ResetToOriginCoroutine()
    //{
    //    float elapsedTime = 0f;

    //    // Save the current positions of the objects
    //    Vector3[] startPositions = new Vector3[placeholders.Length];
    //    for (int i = 0; i < placeholders.Length; i++)
    //    {
    //        startPositions[i] = placeholders[i].transform.position;
    //    }

    //    while (elapsedTime < lerpDuration)
    //    {
    //        float progress = elapsedTime / lerpDuration;

    //        for (int i = 0; i < placeholders.Length; i++)
    //        {
    //            // Lerp each object towards its original position
    //            placeholders[i].transform.position = Vector3.Lerp(startPositions[i], originalPosition, progress);
    //        }

    //        elapsedTime += Time.deltaTime;
    //        yield return null; // Wait until the next frame
    //    }

    //    // Ensure all objects are set exactly at their original positions
    //    for (int i = 0; i < placeholders.Length; i++)
    //    {
    //        placeholders[i].transform.position = originalPosition;
    //    }
    //}

}
