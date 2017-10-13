/**
 * GameHandler.cs
 * by Matthew Gitlin
 * Handles game initialization and logic
 * 
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}

public class GameHandler : MonoBehaviour {

    //Singleton
    private static GameHandler gameHandler = null;
    public static GameHandler instance
    {
        get
        {
            if (gameHandler == null)
            {
                gameHandler = (GameHandler)FindObjectOfType(typeof(GameHandler));
                if (FindObjectsOfType(typeof(GameHandler)).Length > 1) Debug.Log("More than one GameHandler instance in the scene!");
            }
            return gameHandler;
        }
    }

    string[] cardNames = new string[] { "Achievement",
"Ambition",
"Authenticity",
"Autonomy",
"Charity",
"Citizenship ",
"Commitment",
"Community",
"Compassion",
"Consistency",
"Contentment",
"Contribution",
"Conviction",
"Cooperation",
"Courage",
"Courtesy",
"Creativity",
"Curiosity",
"Determination",
"Dignity",
"Discipline",
"Diversity",
"Learning",
"Empathy",
"Excellence",
"Excitement",
"Expertise",
"Exploration",
"Fairness",
"Faith",
"Family",
"Flexibility",
"Frugality",
"Generosity",
"Gratitude",
"Growth",
"Harmony",
"Health",
"Honesty",
"Honor",
"Hospitality",
"Humility",
"Individuality",
"Influence",
"Innovation",
"Integrity",
"Intelligence",
"Intuition",
"Joy",
"Justice",
"Kindness",
"Knowledge",
"Logic",
"Love",
"Loyalty",
"Moderation",
"Nerve",
"Optimism",
"Organization",
"Originality",
"Passion",
"Perseverance",
"Philanthropy",
"Poise",
"Positivity",
"Practicality",
"Precision",
"Preeminence",
"Professionalism",
"Prudence",
"Punctuality",
"Recognition",
"Reliability",
"Reputation",
"Resilience",
"Respect",
"Responsibility",
"Restraint",
"Security",
"Self-control",
"Self-reliance",
"Self-respect",
"Sensitivity",
"Serenity",
"Service",
"Simplicity",
"Sincerity",
"Spirituality",
"Stability",
"Status",
"Structure",
"Success",
"Tolerance",
"Tradition",
"Tranquility",
"Transparency",
"Trust",
"Unity",
"Virtue",
"Wisdom",
};
    [Range(16, 100)]
    public int deckSize = 100;
    public int completeThreshhold = 10;

    int discarded;
    [Header("Object References:")]
    public Card cardPrefab;
    public GameObject deck;
    public DropZone keepZone, discardZone;
    public Transform hand;
    public Text statusText;

    Card[] cardsInHand = new Card[8];
    Card[] finalCards = new Card[8];
    List<string> originalDeck = new List<string>();
    List<string> keepDeck = new List<string>();
    [HideInInspector]
    public bool started = false;
    
    string lastCardMovedValue = "";
    string lastCardDrawnValue = "";
    enum LASTCARDSTATUS { KEPT, DISCARDED};
    LASTCARDSTATUS lastCardStatus;
    int lastCardSibIdx;
    bool roundComplete = false;
    bool gameOver = false;
    int cardsInPlay = 0;
    
    // Access number of cards kept
    public int KeepCount()
    {
        return keepDeck.Count;
    }

    // Access number of cards discarded
    public int DiscardCount()
    {
        return discarded;
    }

    void Start()
    {
        for (int i = 0; i < deckSize; i++)
        {
            originalDeck.Add(cardNames[i].ToLower());
        }
        
        originalDeck.Shuffle();
        cardsInPlay = originalDeck.Count;
        for (int i = 0; i < 8; i++)
        {
            Card c = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity) as Card;
            c.transform.SetParent(hand);
            c.InitValues();
            cardsInHand[i] = c;
            DrawCard(originalDeck[i], i, cardsInHand);
        }
        UpdateDeckCount();
        statusText.text = "Begin exercise.";
    }

    // Add a new value from the to the hand in the same position as the card last moved
    bool DrawCard(string value, int index, Card[] location)
    {
        if (originalDeck.Count > 0)
        {
            location[index].ChangeValue(value);
            location[index].AllowUse();
            lastCardDrawnValue = value;
            if (originalDeck.Contains(value))
            {
                originalDeck.Remove(value);
            }
            UpdateDeckCount();
            return true;
        }
        else
        {
            Debug.Log("No cards left in deck to draw.");
            lastCardDrawnValue = "";
            location[index].ChangeValue("");
            location[index].DisableUse();
            return false;
        }
    }

    // Handle all function when keeping a card 
    public void KeepCard(Card c)
    {
        if (c.allowed)
        {
            keepDeck.Add(c.value);
            lastCardStatus = GameHandler.LASTCARDSTATUS.KEPT;
            lastCardMovedValue = c.value;
            lastCardSibIdx = c.transform.GetSiblingIndex();
            string newValue = "";
            if (originalDeck.Count > 0)
            {
                newValue = originalDeck[0];
            }
            DrawCard(newValue, lastCardSibIdx, cardsInHand);
            InterfaceHandler.instance.undoButton.interactable = true;
        }
    }

    // Handle all function related to discarding a card
    public void DiscardCard(Card c)
    {
        if (c.allowed)
        {
            discarded++;
            lastCardStatus = GameHandler.LASTCARDSTATUS.DISCARDED;
            lastCardMovedValue = c.value;
            lastCardSibIdx = c.transform.GetSiblingIndex();
            string newValue = "";
            if (originalDeck.Count > 0)
            {
                newValue = originalDeck[0];
            }
            DrawCard(newValue, lastCardSibIdx, cardsInHand);
            InterfaceHandler.instance.undoButton.interactable = true;
        }
    }

    // Undo the last move performed by the user
    public void UndoLastMove()
    {
        Debug.Log("Undid: " + lastCardMovedValue);

        if (lastCardStatus == GameHandler.LASTCARDSTATUS.KEPT)              // If card being undone was kept
        {                                                                   // remove last kept card from list
            keepDeck.Remove(lastCardMovedValue);
        }
        else if (lastCardStatus == GameHandler.LASTCARDSTATUS.DISCARDED)    // If card being undone was discarded
        {                                                                   // subtract one card from discard count
            discarded -= 1;
        }
        string newValue = "";
        if (originalDeck.Count == 0)
        {
            newValue = lastCardMovedValue;
        }
        else
        {
            newValue = lastCardDrawnValue;
        }
        originalDeck.Insert(0, newValue);                                   // Add the card last drawn back into the deck
        DrawCard(lastCardMovedValue, lastCardSibIdx, cardsInHand);          // to be redrawn
        UpdateDeckCount();
    }

    // Update count text on deck to show proper amount
    public void UpdateDeckCount ()
    {
        deck.GetComponentInChildren<Text>().text = "Deck\n"+originalDeck.Count+" card"+((originalDeck.Count == 1) ? " ": "s ")+"\nremaining\nin the\n deck";
        keepZone.RefreshText();
        discardZone.RefreshText();
    }

    private void Update()
    {
        if (!gameOver)
        {
            
            if ((keepDeck.Count + discarded) == cardsInPlay && originalDeck.Count == 0)
            {
                roundComplete = true;
            }
            if (roundComplete)
            {
                if (keepDeck.Count <= completeThreshhold)
                {
                    gameOver = true;
                    Debug.Log("Exercise complete");
                    statusText.text = "Exercise complete.";
                    statusText.GetComponent<Animator>().Play("TextFadeIn");
                    originalDeck = new List<string>(keepDeck);
                    originalDeck.Shuffle();
                    cardsInPlay = originalDeck.Count;
                    for (int i = 0; i < keepDeck.Count; i++)
                    {
                        Card c = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity) as Card;
                        c.transform.SetParent(InterfaceHandler.instance.gameCompleteModal.cardArea);
                        c.InitValues();
                        finalCards[i] = c;
                        DrawCard(keepDeck[i], i, finalCards);
                        finalCards[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
                    }
                    discarded = 0;
                    InterfaceHandler.instance.undoButton.gameObject.SetActive(false);
                    InterfaceHandler.instance.GameCompleteModal();
                    InterfaceHandler.instance.reopenGameCompleteModal.gameObject.SetActive(true);
                    UpdateDeckCount();
                }
                else
                {
                    Debug.Log("Round complete");
                    statusText.text = "Round complete. Please continue for the remaining cards.";
                    statusText.GetComponent<Animator>().Play("TextFadeInOut");
                    originalDeck = new List<string>(keepDeck);
                    originalDeck.Shuffle();
                    cardsInPlay = originalDeck.Count;
                    for (int i = 0; i < 8; i++)
                    {
                        DrawCard(keepDeck[i], i, cardsInHand);
                    }
                    keepDeck.Clear();
                    discarded = 0;
                    
                    UpdateDeckCount();
                    roundComplete = false;
                }
                InterfaceHandler.instance.UndoButton();
            }
        }
    }


}
