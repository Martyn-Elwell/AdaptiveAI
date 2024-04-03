using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    public PlayerController player;
    private float idealDistance = 3f;
    [Header ("Override")]
    public bool debugOverride = false;
    [Range(0, 5)]
    public int debugOverrideNum;
    public void Update()
    {
        float distance = transform.position.z - player.transform.position.z;
        if (Mathf.Abs(distance - idealDistance) >= 0.2f ) { animator.SetFloat("Direction", distance - idealDistance); }
        else { animator.SetFloat("Direction", 0f) ; }
        
    }

    public Actions warn()
    {
        
        Actions predictedAction = predictAction();
        if (stunned)
        {
            stunned = false;
            predictedAction = actions[7];
            Debug.Log("Stun is reset");
        }
        currentAction = predictedAction;
        startAction(predictedAction);
        return predictedAction;
    }

    public Actions predictAction()
    {
        Actions returnAction = actions[Random.Range(0, 5)];
        if (debugOverride) { returnAction = actions[debugOverrideNum]; }
        return returnAction;
    }
}
