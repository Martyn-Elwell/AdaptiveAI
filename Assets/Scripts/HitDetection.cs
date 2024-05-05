using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public PlayerController player;
    public AIController AI;
    public Actions playerAction = null;
    public Actions AIAction = null;

    public void IntiateHitDetection()
    {
        if (playerAction != null && AIAction != null)
        {
            float shortestTime = playerAction.impactTime;
            if (shortestTime > AIAction.impactTime)
            {
                shortestTime = AIAction.impactTime;
            }

            Invoke("RunHitDetection", shortestTime);
        }

    }

    public void InputAction(Controller user, Actions action)
    {

        if (user is PlayerController)
        {
            playerAction = action;
        }
        else if (user is AIController)
        {
            AIAction = action;
        }
        CancelInvoke("ResetActions");
        IntiateHitDetection();
    }


    public void RunHitDetection()
    {
        // Checks if both actions are null
        if (playerAction == null &&  AIAction == null) {return; }
        if (playerAction == null)
        { playerAction = player.actions[7]; }
        if (AIAction == null)
        { AIAction = AI.actions[7]; }

        // Players attack is within range
        float distance = Vector3.Distance
            (player.transform.position, AI.transform.position);
        
        // Records the player action for the AI
        AI.recordAction(playerAction);

        // Player in range AI not, player wins
        if (playerAction.range >= distance && AIAction.range < distance)
        {
            player.VisualFeedback(playerAction, true, AI);
        }

        // AI in range Player not, Ai wins
        if (AIAction.range >= distance && playerAction.range < distance)
        {
            AI.VisualFeedback(AI.currentAction, true, player);
        }

        // Parry Check
        if (playerAction == AIAction)
        {
            // Range Check
            if (playerAction.range >= distance)
            {
                Debug.Log("Parried");
                if (playerAction.type == Type.Defence) { return; }
                player.VisualFeedback(player.actions[6], true, AI);
                AI.VisualFeedback(AI.actions[6], true, player);
            }
            else { player.OutOfRange(); }
        }
        // Player counter check
        else if (playerAction.counters.Contains(AIAction))
        {
            // Range Check
            if (playerAction.range >= distance)
            {
                Debug.Log("Player counters with " + playerAction.name);
                player.VisualFeedback(playerAction, true, AI);
            }
            else { player.OutOfRange(); }
        }
        // AI counter check
        else if (playerAction.defences.Contains(AIAction))
        {
            // Range Check
            if (AIAction.range >= distance)
            {
                Debug.Log("AI counters with " + AIAction.name);
                AI.VisualFeedback(AIAction, true, player);
            }
        }
        //Neutral check
        else if (playerAction.neutral.Contains(AIAction))
        {
            // Defence check
            if (playerAction.type == Type.Defence)
            {
                return;
            }
            Debug.Log("Both attack");

            if (playerAction.range >= distance)
            {
                player.VisualFeedback(playerAction, true, AI);
            }
            if (AIAction.range >= distance)
            {
                AI.VisualFeedback(AIAction, true, player);
            }
        }

        if (AI.savedPredictedAction == playerAction)
        {
            AI.CorrectPrediction();
        }
        else
        {
            AI.InCorrectPrediction();
        }
        AIAction = null;
        playerAction = null;

    }

    public void ResetActions()
    {
        
    }
}
