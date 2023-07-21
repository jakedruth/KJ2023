#pragma warning disable IDE0180

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerHandler : MonoBehaviour
{
    [Header("Scene Dependencies")]
    public Transform handHolder;
    public Transform deckHolder;
    public Transform discardHolder;
    public TMPro.TMP_Text deckCountText;
    public TMPro.TMP_Text discardCountText;

    [Header("Values")]
    public float handSpacing;
    public int maxHandSize;

    // [Header("Card data")]
    const int BOARD_SIZE = 3 * 4; // board size is 12 - 3 rows x 4 columns
    protected List<Card> deck;
    protected List<Card> discard;
    protected List<Card> hand;
    protected List<BoardSlot> board;

    public void Awake()
    {
        deck = new List<Card>();
        discard = new List<Card>();
        hand = new List<Card>();
        board = new List<BoardSlot>();

        // Currently a debug thing
        CreateDefaultDeck();
        discard.Clear();
        hand.Clear();
        board.Clear();
    }

    public void Start()
    {
        // starting hand size
        for (int i = 0; i < 3; i++)
        {
            DrawCard();
        }
        deckCountText.text = $"Count: {deck.Count}";
        discardCountText.text = $"Count: {discard.Count}";
    }

    public void ShuffleDeck()
    {
        // Fisher-Yates shuffle https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Card card = deck[k];

            deck[k] = deck[n];
            deck[n] = card;
        }
    }

    [ContextMenu("Draw Card")]
    public void DrawCard()
    {
        if (deck.Count == 0 || hand.Count >= maxHandSize)
            return;

        Card card = deck[0];
        hand.Add(card);
        deck.RemoveAt(0);

        CardHandler prefab = Resources.Load<CardHandler>("Prefabs/CardHandler");
        CardHandler instance = Instantiate(prefab, deckHolder.transform.position + Vector3.up * 0.2f, handHolder.rotation, handHolder);
        instance.SetCard(card);
        instance.Owner = this;

        AdjustHand();
        deckCountText.text = $"Count: {deck.Count}";

        return;
    }

    public void AdjustHand()
    {
        float xPos = (-handSpacing * (handHolder.childCount - 1)) * 0.5f;

        for (int i = 0; i < handHolder.childCount; i++)
        {
            handHolder.GetChild(i).DOLocalMove(Vector3.right * xPos, 0.5f);
            xPos += handSpacing;
        }
    }

    public bool PlayCard(int handIndex, int boardIndex)
    {
        if (handIndex < 0 || handIndex >= hand.Count)
            return false;

        Card card = hand[handIndex];
        if (card is CreatureCard cc)
        {
            // TODO: Can a new creature replace an old creature all ready on the board?
            // check to see if the boardIndex is open
            bool valid = true;
            for (int i = 0; i < board.Count; i++)
            {
                BoardSlot slot = board[i];
                if (slot.boardIndex != boardIndex)
                    continue;

                valid = false;
                break;
            }

            if (valid)
            {
                cc.PlayCard();
                board.Add(new BoardSlot(boardIndex, cc));
                hand.RemoveAt(handIndex);
                return true;
            }
            return false;
        }

        return true;
    }

    public bool DiscardCardOnBoard(int boardIndex)
    {
        for (int i = 0; i < board.Count; i++)
        {
            if (board[i].boardIndex == boardIndex)
            {
                discard.Add(board[i].card);
                board.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    [ContextMenu("Debug-Discard first board card")]
    protected void DiscardFirstBoardCard()
    {
        if (board.Count == 0)
            return;

        DiscardCardOnBoard(board[0].boardIndex);
    }

    [ContextMenu("Debug-Play first")]
    protected void DebugPlayCard()
    {
        if (hand.Count == 0)
            return;

        int boardIndex = 0;
        for (int i = 0; i < board.Count; i++)
        {
            if (board[i].boardIndex == boardIndex)
            {
                boardIndex++;
                i = 0;
            }
        }

        if (boardIndex < BOARD_SIZE)
            PlayCard(0, boardIndex);
    }

    [ContextMenu("Debug-Create default deck")]
    protected void CreateDefaultDeck()
    {
        deck.Clear();
        // Add one of each creature
        deck.Add(new CreatureCard(Resources.Load<Creature_SO>("Cards/Creatures/CreatureCard_01")));
        deck.Add(new CreatureCard(Resources.Load<Creature_SO>("Cards/Creatures/CreatureCard_02")));
        deck.Add(new CreatureCard(Resources.Load<Creature_SO>("Cards/Creatures/CreatureCard_03")));
        deck.Add(new CreatureCard(Resources.Load<Creature_SO>("Cards/Creatures/CreatureCard_04")));
        deck.Add(new CreatureCard(Resources.Load<Creature_SO>("Cards/Creatures/CreatureCard_05")));
        deck.Add(new CreatureCard(Resources.Load<Creature_SO>("Cards/Creatures/CreatureCard_06")));

        // Add a few Attack cards
        deck.Add(new AttackCard(Resources.Load<Attack_SO>("Cards/ActionCards/MeleeAttack")));
        deck.Add(new AttackCard(Resources.Load<Attack_SO>("Cards/ActionCards/MeleeAttack")));
        deck.Add(new AttackCard(Resources.Load<Attack_SO>("Cards/ActionCards/MeleeAttack")));
        deck.Add(new AttackCard(Resources.Load<Attack_SO>("Cards/ActionCards/MeleeAttack")));
        deck.Add(new AttackCard(Resources.Load<Attack_SO>("Cards/ActionCards/MeleeAttack")));

        deck.Add(new AttackCard(Resources.Load<Attack_SO>("Cards/ActionCards/RangedAttack")));
        deck.Add(new AttackCard(Resources.Load<Attack_SO>("Cards/ActionCards/RangedAttack")));
        deck.Add(new AttackCard(Resources.Load<Attack_SO>("Cards/ActionCards/RangedAttack")));
        deck.Add(new AttackCard(Resources.Load<Attack_SO>("Cards/ActionCards/RangedAttack")));
        deck.Add(new AttackCard(Resources.Load<Attack_SO>("Cards/ActionCards/RangedAttack")));

        // Shuffle the deck
        ShuffleDeck();
    }
}

[System.Serializable]
public struct BoardSlot
{
    public int boardIndex;
    public Card card;

    public BoardSlot(int index, Card card)
    {
        boardIndex = index;
        this.card = card;
    }
}

#pragma warning restore IDE0180