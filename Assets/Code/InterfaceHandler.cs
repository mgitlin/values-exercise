/**
 * InterfaceHandler.cs
 * by Matthew Gitlin
 * References and functionality for main interface buttons
 * 
**/

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceHandler : MonoBehaviour {

    [DllImport("__Internal")]
    private static extern void openWindow(string url);

    //Singleton
    private static InterfaceHandler interfaceHandler = null;
    public static InterfaceHandler instance
    {
        get
        {
            if (interfaceHandler == null)
            {
                interfaceHandler = (InterfaceHandler)FindObjectOfType(typeof(InterfaceHandler));
                if (FindObjectsOfType(typeof(InterfaceHandler)).Length > 1) Debug.Log("More than one InterfaceHandler instance in the scene!");
            }
            return interfaceHandler;
        }
    }

    [Header("Modals")]
    public InstructionsModal instructionsModal;
    public GameCompleteModal gameCompleteModal;
    [Header("Buttons")]
    public Button undoButton;
    public Button instructionsButton;
    public Button websiteButton;
    public Button reopenGameCompleteModal;

	public void InstructionsModal()
    {
        instructionsModal.RefreshInstructions();
        instructionsModal.GetComponent<Animator>().Play("OpenModal");
    }

    public void GameCompleteModal()
    {
        gameCompleteModal.GetComponent<Animator>().Play("OpenModal");
    }

    public void Website()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            openWindow("http://www.solution-focusedcoaching.com");
            return;
        }
    }
    
    public void UndoButton()
    {
        undoButton.interactable = false;
    }

}
