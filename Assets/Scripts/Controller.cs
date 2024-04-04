using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public Animator animator;
    protected Vector3 intialPosition;
    public List<Actions> actions;
    public Actions currentAction;
    public GameObject mark;
    public GameObject knifePrefab;
    [SerializeField] protected List<ParticleSystem> particles;
    public GameObject textPrefab;
    [SerializeField] protected Color color;

    protected bool inCombat = false;
    protected bool stunned = false;
    [SerializeField] protected float health = 20;
    [SerializeField] protected Slider healthbar;

    void Start()
    {
        intialPosition = transform.position;
        animator.fireEvents = false;
    }

    public void startAction(Actions action)
    {
        currentAction = action;
        animator.SetTrigger(action.name);
        inCombat = true;
        //StartCoroutine(returnCoroutine(2f));
    }

    protected IEnumerator returnCoroutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        transform.position = mark.transform.position;
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
                    opponenet.hurt(action.damage);
                }
                break;
            // Heavy
            case 1:
                if (sucess)
                {
                    GameObject txt = Instantiate(textPrefab, transform.position + Vector3.up * 2.5f, Quaternion.Euler(0f, 270f, 0f), transform);
                    txt.GetComponent<TextMeshPro>().text = action.Description;
                    txt.GetComponent<TextMeshPro>().color = color;
                    opponenet.hurt(action.damage);
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


    public void hurt(int damage)
    {
        particles[0].Play();
        animator.SetTrigger("Hit");
        health -= damage;
        healthbar.value = health;
        if (health <= 0)
        {
            Debug.Log(gameObject.name + " is dead");
        }
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
        Debug.Log(animator.gameObject.name);
        animator.SetTrigger("SuccesfulBlock");
    }


    public IEnumerator delayHurt(float time, Controller character, int damage)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("hurtignof0");
        character.hurt(damage);
    }
}
