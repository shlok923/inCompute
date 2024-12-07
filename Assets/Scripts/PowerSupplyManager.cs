using System.Collections.Generic;
using UnityEngine;

public class PowerSupplyManager : MonoBehaviour
{
    public Material glow;
    public Material unglow;

    public List<DialInteractor> dials; // List of dials
    private List<List<int>> assignedWires; // List of wires assigned to dials
    private List<GameObject> wires; // List of wire objects

    public List<int> requiredSequence; // Order of dial indices to configure
    public List<int> requiredStates;  // Required states for dials in the sequence

    [SerializeField] private PickupObjects psFile;

    private int currentSequenceIndex = 0; // Tracks progress in the sequence

    private void Awake()
    {
        wires = new List<GameObject>();
        assignedWires = new List<List<int>>();
        ExtractWires();
        AssignWires();
    }

    public void OnDialConfigurationChanged(DialInteractor dial)
    {
        int dialIndex = dials.IndexOf(dial);

        // Check if this dial is the next one in the sequence
        if (dialIndex == requiredSequence[currentSequenceIndex])
        {
            // Check if the dial is in the correct state
            if (dial.hasCorrectConfiguration())
            {
                Debug.Log($"Correct configuration! Dial {dialIndex}, Sequence Index: {currentSequenceIndex + 1}");
                currentSequenceIndex++;

                // Light up the wires for this dial
                SetWiresState(assignedWires[dialIndex], glow);

                // Check if the puzzle is solved
                if (currentSequenceIndex >= requiredSequence.Count)
                {
                    Debug.Log("Puzzle Solved!");
                    ActivateAllWires();
                    psFile.gameObject.SetActive(true);
                }
            }
            else
            {
                //Debug.Log($"Dial {dialIndex} is in the wrong state. Expected: {requiredStates[currentSequenceIndex]}, Got: {dial.GetState()}");
                // Do not reset, just wait for the correct state
            }
        }
        else
        {
            Debug.Log($"Sequence broken! Interacted Dial: {dialIndex}, Expected Dial: {requiredSequence[currentSequenceIndex]}");
            ResetSequence();
        }
    }

    private void ResetSequence()
    {
        Debug.Log("Resetting sequence...");

        foreach (var dial in dials)
        {
            dial.ResetToOriginalAngle();
        }

        currentSequenceIndex = 0;

        // Turn off all wires
        foreach (var wireList in assignedWires)
        {
            SetWiresState(wireList, unglow);
        }
    }

    private void ActivateAllWires()
    {
        foreach (var wireList in assignedWires)
        {
            SetWiresState(wireList, glow);
        }
    }

    private void SetWiresState(List<int> wiresToLight, Material state)
    {
        for (int i = 0; i < wiresToLight.Count; i++)
        {
            wires[wiresToLight[i]].GetComponent<MeshRenderer>().material = state;
        }
    }

    public void AssignWires()
    {
        List<int> wireIndices = new List<int>();
        for (int i = 0; i < wires.Count; i++) wireIndices.Add(i);
        ShuffleList(wireIndices);

        int wiresPerDial = wires.Count / dials.Count;
        for (int i = 0; i < dials.Count; i++)
        {
            List<int> currentWires = new List<int>();
            for (int j = 0; j < wiresPerDial; j++) currentWires.Add(wireIndices[j + i * wiresPerDial]);
            assignedWires.Add(currentWires);
        }
    }

    private void ShuffleList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void ExtractWires()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Wire"))
            {
                wires.Add(transform.GetChild(i).gameObject);
            }
        }
    }
}
