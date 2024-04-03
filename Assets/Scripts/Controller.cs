using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using TMPro;

public class Controller : MonoBehaviour
{
    public Animator animator;
    protected Vector3 intialPosition;
    public List<Actions> actions;
    public Actions currentAction;
    public GameObject mark;
    [SerializeField] private List<ParticleSystem> particles;
    public GameObject textPrefab;
    [SerializeField] protected Color color;

    protected bool inCombat = false;
    protected bool stunned = false;

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
                    opponenet.hurt();
                }
                break;
            // Heavy
            case 1:
                if (sucess)
                {
                    GameObject txt = Instantiate(textPrefab, transform.position + Vector3.up * 2.5f, Quaternion.Euler(0f, 270f, 0f), transform);
                    txt.GetComponent<TextMeshPro>().text = action.Description;
                    txt.GetComponent<TextMeshPro>().color = color;
                    opponenet.hurt();
                }
                break;
            // Block
            case 2:
                if (sucess)
                {
                    GameObject txt = Instantiate(textPrefab, transform.position + Vector3.up * 2.5f, Quaternion.Euler(0f, 270f, 0f), transform);
                    txt.GetComponent<TextMeshPro>().text = action.Description;
                    txt.GetComponent<TextMeshPro>().color = color;
                    opponenet.sparks();
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
                if (sucess)
                {
                    GameObject txt = Instantiate(textPrefab, transform.position + Vector3.up * 2.5f, Quaternion.Euler(0f, 270f, 0f), transform); ;
                    txt.GetComponent<TextMeshPro>().text = action.Description;
                    txt.GetComponent<TextMeshPro>().color = color;
                    opponenet.hurt();
                }
                break;
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


    public void hurt()
    {
        particles[0].Play();
        animator.SetTrigger("Hit");
    }

    public void stun()
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
}
