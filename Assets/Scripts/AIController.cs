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
    [Header("Player")]
    public PlayerController player;
    private float idealDistance = 3f;

    [Header("AlgorithmData")]
    [SerializeField] private float timeSinceLastAttack = 100f;
    [SerializeField] private float comboTime = 2f;
    [SerializeField] private AIType algorithmType = AIType.Probabilistic;
    [SerializeField] private int[] initialAttack = new int[6];
    [SerializeField] private int[][] attackTable = new int[6][];
    private bool lastAttackRecivedWasCombo = false;
    private int previousIndex = 0;

    [Header ("Debug Featrues")]
    public bool debugOverride = false;
    [Range(0, 5)] public int debugOverrideNum;
    public void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            attackTable[i] = new int[6];
        }
    }
    public void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        float distance = transform.position.z - player.transform.position.z;
        if (Mathf.Abs(distance - idealDistance) >= 0.2f ) { animator.SetFloat("Direction", distance - idealDistance); }
        else { animator.SetFloat("Direction", 0f) ; }
        
    }

    public override void startAction(Actions action)
    {
        currentAction = action;
        animator.SetTrigger(action.name);
        inCombat = true;
        Invoke("EndCombat", 1f);
        detector.InputAction(this, action);
        //StartCoroutine(returnCoroutine(2f));
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
        Actions returnAction = actions[0];
        if (timeSinceLastAttack >= comboTime)
        {
            lastAttackRecivedWasCombo = false;

            returnAction = SelectFromRow(initialAttack);

        }
        else if (timeSinceLastAttack < comboTime)
        {
            lastAttackRecivedWasCombo = true;
            int[] row = new int[6];
            for (int i = 0; i < attackTable[0].Length; i++)
            {
                try
                {
                    row[i] = attackTable[previousIndex][i];
                }
                catch
                {
                    Debug.LogError("Previous attack index: " + previousIndex);
                    Debug.LogError("Previous attack was: " + player.actions[previousIndex]);
                    Debug.LogError("i: " + i);
                }
            }
            returnAction = SelectFromRow(row);
        }

        timeSinceLastAttack = 0f;

        return returnAction;
    }

    private Actions SelectFromRow(int[] Row)
    {
        Actions returnAction = actions[0];

        int total = 0;
        int highestCount = 0;
        int maxCount = 0;

        // Finds out total entries in that row
        for (int i = 0; i < Row.Length; i++)
        {
            total += Row[i];
            if (Row[i] > maxCount)
            {
                highestCount = i;
                maxCount = Row[i];
            }
        }
        // Selects the highest value 
        if (algorithmType == AIType.Deterministic)
        {
            returnAction = actions[highestCount];
        }
        // Selects the random value using 
        else
        {

            int randNum = Random.Range(0, total);
            int num = 0;
            for (int i = 0; i < Row.Length; i++)
            {
                num += Row[i];
                if (num >= randNum)
                {
                    returnAction = actions[i];
                }
            }

        }

        return returnAction;
    }

    public void recordAction(Actions action)
    {


        if (lastAttackRecivedWasCombo == false)
        {
            initialAttack[action.ID] += 1;
            previousIndex = action.ID;
        }
        else if (lastAttackRecivedWasCombo == true)
        {
            attackTable[previousIndex][action.ID] += 1;
        }
    }

    public Actions randomAction()
    {
        Actions returnAction = actions[Random.Range(0, 5)];
        if (debugOverride) { returnAction = actions[debugOverrideNum]; }
        return returnAction;
    }


}
