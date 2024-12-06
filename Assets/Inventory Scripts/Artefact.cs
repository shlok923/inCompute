using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Artefact", menuName = "Inventory Holdable/Artefacts")]

public class Artefact : ScriptableObject {
    public enum Type {
        hint,
        usable,
        broken
    }

    public string artefactName;
    public Mesh model;
    public Sprite sprite;
    public Sprite infoSprite;
    public Type type;
}
