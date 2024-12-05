using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSupplyManager : MonoBehaviour {
    public Material glow;
    public Material unglow;

    public List<DialInteractor> dials;
    private List<List<int>> assignedWires;
    private List<GameObject> wires;

    private void Awake() {
        wires = new List<GameObject>();
        assignedWires = new List<List<int>>();
        ExtractWires();
        AssignWires();
    }

    private void Update() {
        for (int i = 0; i < dials.Count; i++) {
            if (dials[i].hasCorrectConfiguration()) SetWiresState(assignedWires[i], glow);
            else SetWiresState(assignedWires[i], unglow);
        }
    }

    private void SetWiresState(List<int> wiresToLight, Material state) {
        for (int i = 0; i < wiresToLight.Count; i++) {
            wires[wiresToLight[i]].GetComponent<MeshRenderer>().material = state;
        }
    }

    public void AssignWires() {
        List<int> wireIndices = new List<int>();
        for (int i = 0; i < wires.Count; i++) wireIndices.Add(i);
        ShuffleList(wireIndices);

        int wiresPerDial = wires.Count / dials.Count;
        for (int i = 0; i < dials.Count; i++) {
            List<int> currentWires = new List<int>();
            for (int j = 0; j < wiresPerDial; j++) currentWires.Add(wireIndices[j + i * wiresPerDial]);
            assignedWires.Add(currentWires);
        }
    }

    private void ShuffleList(List<int> list) {
        for (int i = list.Count - 1; i > 0; i--) {
            int randomIndex = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void ExtractWires() {
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).CompareTag("Wire")) {
                wires.Add(transform.GetChild(i).gameObject);
            }
        }
    }
}
