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
    [Header("AI")]
    public AIController AI;
    public float timeSinceLastAttack = 100f;
    public float comboTime = 2f;


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

        timeSinceLastAttack += Time.deltaTime;

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
    }

    public void HitDetection()
    {
        
    }

    public override void stun(int damage, Controller opponent)
    {

        health -= damage;
        UI.UpdateHealth(this, health);
        if (health <= 0)
        {
            Debug.Log(gameObject.name + " is dead");
            opponent.winRound();
            health = maxHealth;
            UI.UpdateHealth(this, health);
        }

        particles[2].Play();
        stunned = true;
        Invoke("unstun", 1.5f);
        inCombat = false;
        attack(7); // Stunned
        
    }

    public void unstun()
    {
        stunned = false;
    }
}
