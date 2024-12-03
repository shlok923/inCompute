using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardImplementation : MonoBehaviour {
    public Card card;

    public Image graphic;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public void UpdateStats() {
        graphic.sprite = card.sprite;
        title.text = card.title;
        description.text = card.description;
    }
}
