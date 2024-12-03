using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Inventory Holdable/Cards")]
public class Card : ScriptableObject {
    public string title;
    public int ID;
    public CardBehaviour behaviour;
    public Sprite sprite;
    public string description;
}
