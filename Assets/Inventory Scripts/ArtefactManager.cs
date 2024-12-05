using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ArtefactManager : MonoBehaviour {
    public Artefact artefact;
    public GameObject model;

    private GameObject highlight;

    public States state = States.unpicked;

    public enum States {
        unpicked,
        idle,
        hover,
        info,
        play
    }

    private void Awake() {
        UpdateStats();
    }

    public void UpdateStats() {
        if (artefact == null) return;
        model.GetComponent<MeshFilter>().mesh = artefact.model;
    }

    public void isPicked(Transform slotHighlight) {
        state = States.idle;
    }
}
