/**
 * InstructionsModal.cs
 * by Matthew Gitlin
 * Functionality and information for instructions modal and its buttons
 * 
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsModal : MonoBehaviour {

    string[] pages = new string[]
    {
        "Welcome to the exercise!",
        "This exercise is played kind of like the game Solitaire. Once you start the game, you’ll see 8 words laid out on the screen.  If you have a strong response to a word, you’ll click on it with your mouse and drag it to the left and place it in the KEEP pile.  If a word isn’t affecting you at all, then drag it to the right, to the DISCARD pile. Once you move a card to a pile, a new card takes its place. You'll continue this sorting until you have put all 100 cards into one of the two piles. That's the end of Round 1. If you accidentally move a card to the wrong pile, you can hit the UNDO button to start again.  Carefully move the card into the correct pile.",
        "Once you are done sorting all 100 cards in Round 1, the computer will reshuffle the cards in the KEEP pile and present those to you again.  This is where it gets a little harder!  If every word is a value, then none of them are values. You need to eliminate at least half of the cards to the DISCARD pile. Once you’ve gone through all the cards in your KEEP pile, the computer again reshuffles what remains and presents them to you again. This continues until you only have 8 or fewer cards left.",
        "The final 8 or fewer words are your core values.  Take another look at them and say each one out loud.  Write them down, take a screen capture of them, or use your phone to take a photo of the screen.  But it’s important to acknowledge them in some way. Click on the \"Screenshot\" button to open a savable screen capture in a new browser window. You will be able to save a copy of the image to your computer by right-clicking on the screenshot and saving it. Please also email the image to me at drew@solution-focusedcoaching.com. Keep a copy of this image so we can start working with your values at your next session.",
        "Most of all, have FUN with this time of discovery.  Don’t think too much about it and certainly don’t stress over it!  This is simply a game to help us get closer to what is most important to you.  If at the end of the game, you don’t like the cards remaining or they don’t feel like a good representation of your core values, that’s okay.  We’ll scrap it and try another way to identify your core values."
    };

    public Text title;
    public Text pageContent;
    public Text pageNumber;
    public Button nextButton;
    public Button prevButton;
    public Button closeButton;
    int i = 0;

    private void Start()
    {
        GetComponent<Animator>().Play("OpenModal");
        pageContent.text = pages[i];
        pageNumber.text = (i).ToString() + "/" + (pages.Length - 1);
    }

    public void RefreshInstructions()
    {
        i = 1;
        pageContent.text = pages[i];
        pageNumber.text = (i).ToString() + "/" + (pages.Length - 1);
        nextButton.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);
    }

    public void NextPage()
    {
        title.text = "Instructions";
        nextButton.GetComponentInChildren<Text>().text = "Next Page   >";
        pageContent.fontSize = 20;
        prevButton.gameObject.SetActive(true);
        i++;
        pageContent.text = pages[i];
        pageNumber.gameObject.SetActive(true);
        if (i == (pages.Length - 1))
        {
            nextButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(true);
        }
        pageNumber.text = (i).ToString() + "/" + (pages.Length - 1);
    }

    public void PrevPage()
    {
        nextButton.gameObject.SetActive(true);
        i--;
        pageContent.text = pages[i];
        closeButton.gameObject.SetActive(false);
        if (i == 1)
        {
            prevButton.gameObject.SetActive(false);
        }
        pageNumber.text = (i).ToString() + "/" + (pages.Length - 1);
    }

    public void Close()
    {
        GetComponent<Animator>().Play("CloseModal");
        GameHandler.instance.started = true;
    }
}
