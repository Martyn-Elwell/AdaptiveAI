using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    public PlayerController player;
    private float idealDistance = 3f;
    public void Update()
    {
        float distance = transform.position.z - player.transform.position.z;
        if (Mathf.Abs(distance - idealDistance) >= 0.2f ) { animator.SetFloat("Direction", distance - idealDistance); }
        else { animator.SetFloat("Direction", 0f) ; }
        
    }

    public void warn()
    {
        Actions predictedAction = predictAction();

        startAction(predictedAction);
    }

    public Actions predictAction()
    {
        Actions returnAction = actions[Random.Range(0, 5)];
        return returnAction;
    }
}
