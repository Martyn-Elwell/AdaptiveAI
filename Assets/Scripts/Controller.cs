using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Animator animator;
    protected Vector3 intialPosition;
    public List<Actions> actions;
    public Actions currentAction;
    public GameObject mark;
    [SerializeField] private List<ParticleSystem> particles;

    protected bool inCombat = false;

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

    public void hurt()
    {
        particles[0].Play();
    }

    public void collide()
    {
        particles[1].Play();
    }
}
