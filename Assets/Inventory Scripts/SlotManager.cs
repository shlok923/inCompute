using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlotManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    public Artefact artefact;
    public GameObject highlight;
    public Image sprite;
    private GameObject instantiatedDescription;

    public Vector3 descriptionBoxOffset;
    public GameObject descriptionBox;

    public void AddToSlot(ArtefactManager artefactManager) {
        artefact = artefactManager.artefact;
        sprite.sprite = artefact.sprite;
    }

    public void RemoveFromSlot() {
        if (artefact == null) return;
        if (instantiatedDescription != null) { 
            highlight.SetActive(false);
            Destroy(instantiatedDescription.gameObject);
        }

        artefact = null;
        sprite.sprite = null;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (artefact == null) return;
        highlight.SetActive(true);
        
        instantiatedDescription = Instantiate(descriptionBox, descriptionBoxOffset, Quaternion.identity, transform);
        instantiatedDescription.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = artefact.artefactName;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (artefact == null) return;
        highlight.SetActive(false);

        Destroy(instantiatedDescription);
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (artefact == null) return;
        if (instantiatedDescription != null) {
            highlight.SetActive(false);
            Destroy(instantiatedDescription.gameObject);
        }

        GetComponentInParent<InventoryManager>().CanOpen(false);

    }
}
