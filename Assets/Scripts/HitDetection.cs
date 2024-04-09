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
        IntiateHitDetection();
    }


    public void RunHitDetection()
    {
        if (playerAction == null || AIAction == null) { return; }

        float distance = Vector3.Distance(player.transform.position, AI.transform.position);
        //Players attack is within range
        AI.recordAction(playerAction);

        // Player in range AI not
        if (playerAction.range >= distance && AIAction.range < distance)
        {
            player.VisualFeedback(playerAction, true, AI);
        }

        // AI in range Player not
        if (AIAction.range >= distance && playerAction.range < distance)
        {
            AI.VisualFeedback(AI.currentAction, true, player);
        }

        if (playerAction == AIAction)
        {
            if (playerAction.range >= distance)
            {
                Debug.Log("Parried");
                if (playerAction.type == Type.Defence) { return; }
                player.VisualFeedback(player.actions[6], true, AI);
                AI.VisualFeedback(AI.actions[6], true, player);
            }
            else
            {
                player.OutOfRange();
            }
        }
        else if (playerAction.counters.Contains(AIAction))
        {
            if (playerAction.range >= distance)
            {
                Debug.Log("Player counters");
                player.VisualFeedback(playerAction, true, AI);
            }
            else
            {
                player.OutOfRange();
            }
        }
        else if (playerAction.defences.Contains(AIAction))
        {
            if (AIAction.range >= distance)
            {
                Debug.Log("AI counters");
                AI.VisualFeedback(AIAction, true, player);
            }
            else
            {
                
            }

        }
        else if (playerAction.neutral.Contains(AIAction))
        {
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

        AIAction = null;
        playerAction = null;

    }
}
