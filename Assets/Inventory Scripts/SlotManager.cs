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

    public Vector3 descriptionOffset;
    public GameObject descriptionBox;
    private GameObject instantiatedDescription;

    private GameObject artefactInfo;
    private InventoryManager inventoryManager;

    private void Awake() {
        inventoryManager = GetComponentInParent<InventoryManager>();
        if (inventoryManager != null) artefactInfo = inventoryManager.artefactInfo;
    }

    public void AddToSlot(ArtefactManager artefactManager) {
        artefact = artefactManager.artefact;
        if (artefact == null) return;

        sprite.gameObject.SetActive(true);
        sprite.sprite = artefact.sprite;

        if (inventoryManager == null) highlight.SetActive(true);
    }

    public void RemoveFromSlot() {
        highlight.SetActive(false);
        if (artefact == null) return;
        if (instantiatedDescription != null) { 
            Destroy(instantiatedDescription.gameObject);
        }

        sprite.sprite = null;
        sprite.gameObject.SetActive(false);
        artefact = null;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (artefact == null) return;
        highlight.SetActive(true);
        
        instantiatedDescription = Instantiate(descriptionBox, transform.position + descriptionOffset, Quaternion.identity, transform);
        instantiatedDescription.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = artefact.artefactName;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (artefact == null) return;
        if (inventoryManager != null) highlight.SetActive(false);

        Destroy(instantiatedDescription);
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (inventoryManager == null) return;
        if (instantiatedDescription != null) {
            highlight.SetActive(false);
            Destroy(instantiatedDescription.gameObject);
        }

        inventoryManager.heldInventory.SetHeld(this);
        RemoveFromSlot();
    }
}
