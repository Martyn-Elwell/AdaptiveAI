using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{ 
    public AIController AI;


    public void Update()
    {
        /*if (!inCombat)
        {
            inputHandler();
        }
        else if (inCombat)
        {
            returnToItialPosition();
        }*/
        inputHandler();

    }

    public void inputHandler()
    {
        Gamepad gamepad = Gamepad.current;

        float horizontalInput = Input.GetAxis("Horizontal");
        animator.SetFloat("Direction", horizontalInput);
        if (stunned) { return; }
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Debug.Log("A is pressed!");
            // Dodge
            attack(3);
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Debug.Log("B is pressed!");
            // Heavy
            attack(1);

        }
        if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            //Debug.Log("X is pressed!");
            // Light
            attack(0);

        }
        if (Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            //Debug.Log("Y is pressed!");
            // Stun
            attack(4);
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton4) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            //Debug.Log("LB is pressed!");
            // Block
            attack(2);
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.Alpha6))
        {
            //Debug.Log("RB is pressed!");
            // Ranged
            attack(5);

        }
        //if (gamepad.leftTrigger.wasPressedThisFrame || Input.GetKeyDown(KeyCode.Alpha7)) {Debug.Log("LT is pressed!");}
        //if (gamepad.rightTrigger.wasPressedThisFrame || Input.GetKeyDown(KeyCode.Alpha8)) {Debug.Log("RT is pressed!");}
    }

    public void attack(int id)
    {
        startAction(actions[id]);
        Actions aiAction = AI.warn();
        float shortestTime = actions[id].impactTime;
        if (shortestTime > aiAction.impactTime)
        {
            shortestTime = aiAction.impactTime;
        }
        Invoke("HitDetection", shortestTime);
    }

    public void HitDetection()
    {
        Debug.Log("Ai uses " + AI.currentAction.name);

        float distance = Vector3.Distance(transform.position, AI.transform.position);
        //Players attack is within range
        AI.recordAction(currentAction);

        // Player in range AI not
        if (currentAction.range >= distance && AI.currentAction.range < distance)
        {
            VisualFeedback(currentAction, true, AI);
        }

        // AI in range Player not
        if (AI.currentAction.range >= distance && currentAction.range < distance)
        {
            AI.VisualFeedback(AI.currentAction, true, this);
        }

        if (currentAction == AI.currentAction)
        {
            if (currentAction.range >= distance)
            {
                Debug.Log("Parried");
                if (currentAction.type == Type.Defence) { return; }
                VisualFeedback(actions[6], true, AI);
                AI.VisualFeedback(actions[6], true, this);
            }
            else
            {
                GameObject txt = Instantiate(textPrefab, transform.position + Vector3.up * 2.5f, Quaternion.Euler(0f, 270f, 0f), transform);
                txt.GetComponent<TextMeshPro>().text = "Out of Range";
            }
        }
        else if (currentAction.counters.Contains(AI.currentAction))
        {
            if (currentAction.range >= distance)
            {
                Debug.Log("Player counters");
                VisualFeedback(currentAction, true, AI);
            }
            else
            {
                GameObject txt = Instantiate(textPrefab, transform.position + Vector3.up * 2.5f, Quaternion.Euler(0f, 270f, 0f), transform);
                txt.GetComponent<TextMeshPro>().text = "Out of Range";
            }
        }
        else if (currentAction.defences.Contains(AI.currentAction))
        {
            if (AI.currentAction.range >= distance)
            {
                Debug.Log("AI counters");
                AI.VisualFeedback(AI.currentAction, true, this);
            }
            else
            {
                GameObject txt = Instantiate(textPrefab, AI.transform.position + Vector3.up * 2.5f, Quaternion.Euler(0f, 270f, 0f), transform);
                txt.GetComponent<TextMeshPro>().text = "Out of Range";
            }

        }
        else if (currentAction.neutral.Contains(AI.currentAction))
        {
            if (currentAction.type == Type.Defence)
            {
                return;
            }
            Debug.Log("Both attack");

            if (currentAction.range >= distance)
            {
                VisualFeedback(currentAction, true, AI);
            }
            if (AI.currentAction.range >= distance)
            {
                AI.VisualFeedback(AI.currentAction, true, this);
            }
        }
    }

    public override void stun()
    {
        particles[2].Play();
        stunned = true;
        Invoke("unstun", 1.5f);
        attack(7); // Stunned
    }

    public void unstun()
    {
        stunned = false;
    }
}
