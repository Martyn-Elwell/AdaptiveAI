using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    [Header("References")]
    public HitDetection detector;
    public UIController UI;

    [Header("Actions")]
    public List<Actions> actions;
    public Actions currentAction;

    [Header("Visuals")]
    public Animator animator;
    public GameObject knifePrefab;
    [SerializeField] protected List<ParticleSystem> particles;
    public GameObject textPrefab;
    [SerializeField] protected Color color;

    [Header("Character Data")]
    [SerializeField] protected bool inCombat = false;
    [SerializeField] protected bool stunned = false;
    [SerializeField] protected float health = 20;
    [SerializeField] protected float maxHealth = 20;
    [SerializeField] protected int score = 0;


    void Start()
    {
        animator.fireEvents = false;
    }

    public virtual void startAction(Actions action)
    {
        if (inCombat ) { return; }
        currentAction = action;
        animator.SetTrigger(action.name);
        inCombat = true;
        Invoke("EndCombat", 0.8f);
        detector.InputAction(this, action);
        //StartCoroutine(returnCoroutine(2f));
    }

    protected void EndCombat()
    {
        inCombat = false;
    }

    public void VisualFeedback(Actions action, bool sucess, Controller opponenet)
    {
        switch (action.ID)
        {
            // Light
            case 0:
                if (sucess)
                {
                    GameObject txt = Instantiate(textPrefab, transform.position + Vector3.up * 2.5f, Quaternion.Euler(0f, 270f, 0f), transform);
                    txt.GetComponent<TextMeshPro>().text = action.Description;
                    txt.GetComponent<TextMeshPro>().color = color;
                    opponenet.hurt(action.damage, this);
                }
                break;
            // Heavy
            case 1:
                if (sucess)
                {
                    GameObject txt = Instantiate(textPrefab, transform.position + Vector3.up * 2.5f, Quaternion.Euler(0f, 270f, 0f), transform);
                    txt.GetComponent<TextMeshPro>().text = action.Description;
                    txt.GetComponent<TextMeshPro>().color = color;
                    opponenet.hurt(action.damage, this);
                }
                break;
            // Block
            case 2:
                if (sucess)
                {
                    GameObject txt = Instantiate(textPrefab, transform.position + Vector3.up * 2.5f, Quaternion.Euler(0f, 270f, 0f), transform);
                    txt.GetComponent<TextMeshPro>().text = action.Description;
                    txt.GetComponent<TextMeshPro>().color = color;
                    shieldClash();
                }
                break;
            // Dodge
            case 3:
                if (sucess)
                {
                    GameObject txt = Instantiate(textPrefab, transform.position + Vector3.up * 2.5f, Quaternion.Euler(0f, 270f, 0f), transform);
                    txt.GetComponent<TextMeshPro>().text = action.Description;
                    txt.GetComponent<TextMeshPro>().color = color;
                    //opponenet.hurt();
                }
                break;
            // Stun
            case 4:
                if (sucess)
                {
                    GameObject txt = Instantiate(textPrefab, transform.position + Vector3.up * 2.5f, Quaternion.Euler(0f, 270f, 0f), transform);
                    txt.GetComponent<TextMeshPro>().text = action.Description;
                    txt.GetComponent<TextMeshPro>().color = color;
                    opponenet.stun();
                }
                break;
            // Ranged
            case 5:
                GameObject knife = Instantiate(knifePrefab, transform.position + Vector3.up * 2f, Quaternion.Euler(0f, 0f, 90f));
                if (sucess)
                {
                    GameObject txt = Instantiate(textPrefab, transform.position + Vector3.up * 2.5f, Quaternion.Euler(0f, 270f, 0f), transform); ;
                    txt.GetComponent<TextMeshPro>().text = action.Description;
                    txt.GetComponent<TextMeshPro>().color = color;
                    StartCoroutine(delayHurt(0.3f, opponenet, action.damage));
                }
                break;
            // Parried
            case 6:
                if (sucess)
                {
                    GameObject txt = Instantiate(textPrefab, transform.position + Vector3.up * 2.5f, Quaternion.Euler(0f, 270f, 0f), transform); ;
                    txt.GetComponent<TextMeshPro>().text = action.Description;
                    txt.GetComponent<TextMeshPro>().color = color;
                    sparks();
                }
                break;
        }
    }


    public void hurt(int damage, Controller opponent)
    {
        particles[0].Play();
        animator.SetTrigger("Hit");
        health -= damage;
        UI.UpdateHealth(this, health);
        if (health <= 0)
        {
            Debug.Log(gameObject.name + " is dead");
            opponent.winRound();
            health = maxHealth;
            UI.UpdateHealth(this, health);
        }
    }

    public void OutOfRange()
    {
        GameObject txt = Instantiate(textPrefab, transform.position + Vector3.up * 2.5f, Quaternion.Euler(0f, 270f, 0f), transform);
        txt.GetComponent<TextMeshPro>().text = "Out of Range";
    }

    public virtual void stun()
    {
        animator.SetTrigger("Stunned");
        currentAction = actions[7]; // Stunned
        particles[2].Play();
        stunned = true;
    }

    public void sparks()
    {
        particles[1].Play();
    }
    
    public void shieldClash()
    {
        particles[3].Play();
        animator.SetTrigger("SuccesfulBlock");
    }


    public IEnumerator delayHurt(float time, Controller character, int damage)
    {
        yield return new WaitForSeconds(time);
        character.hurt(damage, this);
    }
    public void winRound()
    {
        score += 1;
        UI.UpdateScore(this, score);
    }
}
