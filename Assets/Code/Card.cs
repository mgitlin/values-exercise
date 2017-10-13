/**
 * Card.cs
 * by Matthew Gitlin
 * based on Unity Drag and Drop Tutorial from Quill18
 * Functionality and logic for card objects
 * 
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string value;
    [HideInInspector]
    public bool inZone;
    Text t;
    CanvasGroup cg;
    public Vector3 originalPosition;
    public bool allowed = true;

    public void InitValues()
    {
        cg = GetComponent<CanvasGroup>();
        t = GetComponentInChildren<Text>();
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    // Update cards value and text compent to something new
    public void ChangeValue(string newValue) {
        this.value = newValue;
        this.t.text = newValue;
    }

    // Perform certain action when dragging of card initiates
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        cg.blocksRaycasts = false;
    }

    // Perform certain action while card is being dragged
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    // Perform certain action once dragging of card ends
    public void OnEndDrag(PointerEventData eventData)
    {
        this.inZone = false;
        this.transform.position = originalPosition;
        cg.blocksRaycasts = true;
        GameHandler.instance.UpdateDeckCount();
    }

    // Show card and allow it to be interacted with
    public void AllowUse()
    {
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        cg.interactable = true;
        allowed = true;
    }

    // Hide card and disallow it to be interacted with
    public void DisableUse()
    {
        cg.alpha = 0;
        cg.blocksRaycasts = false;
        cg.interactable = false;
        allowed = false;
    }

}
