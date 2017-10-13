/**
 * DropZone.cs
 * by Matthew Gitlin
 * based on Unity Drag and Drop Tutorial from Quill18
 * Functionality and logic for areas that can have cards dropped onto
 * 
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    public enum Function { KEEP, DISCARD };
    public Function function;

    // Perform certain action when mouse pointer enters DropZone
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Card c = eventData.pointerDrag.GetComponent<Card>();
            if (c != null)
            {
                c.inZone = true;
            }
        }
    }

    // Perform certain action when mouse pointer exits DropZone
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Card c = eventData.pointerDrag.GetComponent<Card>();
            if (c != null)
            {
                c.inZone = false;
            }
        }
    }

    // Perform certain action once mouse button is released while over DropZone
    public void OnDrop(PointerEventData eventData)
    {
        Card c = eventData.pointerDrag.GetComponent<Card>();
        if (c.inZone == true) {
            if (function == Function.KEEP)
            {
                GameHandler.instance.KeepCard(c);
            }
            else
            {
                GameHandler.instance.DiscardCard(c);
            }
            RefreshText();
        }
    }

    // Refresh text component on a DropZone according to its function
    public void RefreshText()
    {
        GameHandler gh = GameHandler.instance;

        Text t = GetComponentInChildren<Text>();
        if (function == Function.KEEP)
        {
            t.text = "Keeping\n" + gh.KeepCount().ToString() + " card" + ((gh.KeepCount() != 1) ? "s " : "");
        }
        else
        {
            t.text = "Discarding\n" + gh.DiscardCount().ToString() + " card" + ((gh.DiscardCount() != 1) ? "s " : "");
        }
    }
}
