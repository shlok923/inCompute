using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorBehaviour", menuName = "Card Behaviours/Color")]
public class ColorBehaviour : CardBehaviour {
    // Solving Blue 
    private List<Mirror> mirrorStateOne;
    // 10, 66
    private List<Mirror> mirrorStateTwo;
    // 60, 12, 51, 74
    // 60, 12, 10, 66, 51, 74

    private List<PlaceObjects> crystalPlaceholders;
    private GameObject GPUManager;

    public void SetMirrors(List<Mirror> stateOne, List<Mirror> stateTwo, List<PlaceObjects> crystalPlaces, GameObject gpuManager) {
        mirrorStateOne = stateOne;
        mirrorStateTwo = stateTwo;
        crystalPlaceholders = crystalPlaces;
        GPUManager = gpuManager;
    }

    public override bool canUse() {
        for (int i = 0; i  < crystalPlaceholders.Count; i++) {
            if (!crystalPlaceholders[i].IsObjectPlaced()) return false;
        }

        return GPUManager.activeSelf;
    }

    public override void Activate() {
        for (int i = 0; i < mirrorStateOne.Count; i++) {
            mirrorStateOne[i].canInteract = false;
            mirrorStateOne[i].ToggleUpDown();
            Debug.Log(i);
        }

        for (int i = 0; i < mirrorStateTwo.Count; i++) {
            mirrorStateTwo[i].canInteract = false;
            mirrorStateTwo[i].ToggleUpDown();
            mirrorStateTwo[i].ToggleRotationState();
            Debug.Log(i);
        }
    }

    public override void Regenerate() {
        Debug.Log("Card of Color can't be regenerated!");
    }
}
