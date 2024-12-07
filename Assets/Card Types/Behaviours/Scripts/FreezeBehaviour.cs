using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FreezeBehaviour", menuName = "Card Behaviours/Freeze")]
public class FreezeBehaviour : CardBehaviour {
    private MazeGenerator mazeGenerator;

    public void SetMaze(MazeGenerator mazeGen) {
        mazeGenerator = mazeGen;
    }

    public override bool canUse() {
        return mazeGenerator.gameObject.activeSelf;
    }

    public override void Activate() {
        mazeGenerator.canTransition = false;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.freezeCardUse);
    }

    public override void Regenerate() {
        Debug.Log("Card of Freeze of can't be regenerated!");
    }
}
