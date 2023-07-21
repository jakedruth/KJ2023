using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGameHandler : MonoBehaviour
{
    public enum PHASE_ORDER
    {
        START_TURN,
        DRAW_CARD,
        SELECT_CARD_TO_PLAY_FROM_HAND,
        PLAY_CARD,
        PLAYED_CARD_TAKES_EFFECT,
        END_TURN,
    }


    public PlayerHandler playerHuman;
    public PlayerHandler playerAI;

    public bool humanCurrentPlayer = true;
    public PHASE_ORDER currentPhase;

    void Awake()
    {
        currentPhase = PHASE_ORDER.START_TURN;
    }

    public void HandleCurrentPhase()
    {

    }

    public void GoToNextPhase()
    {
        if (currentPhase != PHASE_ORDER.END_TURN)
        {
            currentPhase++;
            humanCurrentPlayer = !humanCurrentPlayer;
        }
        else
            currentPhase = PHASE_ORDER.START_TURN;

        switch (currentPhase)
        {
            case PHASE_ORDER.START_TURN:
                break;
            case PHASE_ORDER.DRAW_CARD:
                break;
            case PHASE_ORDER.SELECT_CARD_TO_PLAY_FROM_HAND:
                break;
            case PHASE_ORDER.PLAY_CARD:
                break;
            case PHASE_ORDER.PLAYED_CARD_TAKES_EFFECT:
                break;
            case PHASE_ORDER.END_TURN:
                break;
            default:
                break;
        }
    }
}
