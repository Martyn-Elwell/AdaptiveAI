using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIType
{
    Deterministic = 0,
    Probabilistic = 1,
}

public class AIController : Controller
{
    public PlayerController player;
    private float idealDistance = 3f;

    [Header("AlgorithmData")]
    public float timeSinceLastAttack = 0f;
    [SerializeField] private float comboTime = 2f;
    private int[] initialAttacks = new int[6];
    public int[,] attackTable = new int[6, 6];
    public bool lastAttackRecivedWasCombo = false;
    public int previousActionIndex = 0;

    [Header ("Override")]
    public bool debugOverride = false;
    [Range(0, 5)]
    public int debugOverrideNum;
    public void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        float distance = transform.position.z - player.transform.position.z;
        if (Mathf.Abs(distance - idealDistance) >= 0.2f ) { animator.SetFloat("Direction", distance - idealDistance); }
        else { animator.SetFloat("Direction", 0f) ; }
        
    }

    public void invokeWarn(float time)
    {
        Invoke("warn", time);
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

    public void punish()
    {
        Actions predictedAction = predictAction();
        currentAction = predictedAction;
        startAction(predictedAction);
        wait(1f);
    }

    private IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
        player.HitDetection();
    }

    public Actions predictAction()
    {
        if (timeSinceLastAttack >= comboTime)
        {
            lastAttackRecivedWasCombo = false;
            
        }
        else if (timeSinceLastAttack < comboTime)
        {
            lastAttackRecivedWasCombo = true;

        }


        Actions returnAction = actions[Random.Range(0, 5)];
        if (debugOverride) { returnAction = actions[debugOverrideNum]; }
        return returnAction;
    }

    public void recordAction(Actions action)
    {/*
        if (lastAttackRecivedWasCombo == false)
        {
            initialAttacks[action.ID] += 1;
            previousActionIndex = action.ID;
        }
        else if (lastAttackRecivedWasCombo == true)
        {
            attackTable[previousActionIndex, action.ID] += 1;
        }*/
    }


}
