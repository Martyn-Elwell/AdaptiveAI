using System.Collections;
using System.Collections.Generic;
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

        if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("A is pressed!");
            // Dodge
            startAcion(actions[3]);
            AI.warn();
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("B is pressed!");
            // Heavy
            startAcion(actions[1]);
            AI.warn();

        }
        if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("X is pressed!");
            // Light
            startAcion(actions[0]);
            AI.warn();

        }
        if (Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Y is pressed!");
            // Stun
            startAcion(actions[4]);
            AI.warn();

        }
        if (Input.GetKeyDown(KeyCode.JoystickButton4) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("LB is pressed!");
            // Block
            startAcion(actions[2]);
            AI.warn();
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("RB is pressed!");
            // Ranged
            startAcion(actions[5]);
            AI.warn();

        }
        //if (gamepad.leftTrigger.wasPressedThisFrame || Input.GetKeyDown(KeyCode.Alpha7)) {Debug.Log("LT is pressed!");}
        //if (gamepad.rightTrigger.wasPressedThisFrame || Input.GetKeyDown(KeyCode.Alpha8)) {Debug.Log("RT is pressed!");}
    }
}
