using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Animator animator;
    protected Vector3 intialPosition;
    public List<Actions> actions;
    public GameObject mark;

    protected bool inCombat = false;

    void Start()
    {
        intialPosition = transform.position;
    }

    public void startAcion(Actions action)
    {
        animator.SetTrigger(action.name);
        inCombat = true;
        StartCoroutine(returnCoroutine(2f));
    }

    protected void returnToItialPosition()
    {
        //transform.position = Vector3.Slerp(transform.position, intialPosition, 5f / 1000);
        //animator.SetBool("Walk", true);
    }

    protected IEnumerator returnCoroutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        transform.position = mark.transform.position;
        inCombat = false;




        /*yield return new WaitForSeconds(delayTime);
        returning = true;
        yield return new WaitForSeconds(moveTime);
        returning = false;
        animator.SetBool("Walk", false);*/

    }
}
