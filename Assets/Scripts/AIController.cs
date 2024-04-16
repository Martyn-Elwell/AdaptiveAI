using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum AIType
{
    Deterministic = 0,
    Probabilistic = 1,
}
public enum ResponseType
{
    Random = 0,
    Weighted = 1,
}

public class AIController : Controller
{
    [Header("Player")]
    public PlayerController player;
    private float idealDistance = 3f;

    [Header("AlgorithmData")]
    [SerializeField] public float playerAttackCount = 0;
    [SerializeField] private float playerAggresionTotal = 0;
    [SerializeField] private float playerAggresionScore = 0;
    [SerializeField] public float aggresionWeighting = 1;
    [SerializeField] private float confidenceScore = 0;
    [SerializeField] public float confidenceWeighting = 1;
    [SerializeField] private AIType algorithmType = AIType.Probabilistic;
    [SerializeField] private ResponseType counterType = ResponseType.Random;
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

    public Actions punish()
    {
        Actions action = actions[1];
        //inCombat = false;
        currentAction = action;
        startAction(action);
        return action;
    }

    private IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
        player.HitDetection();
    }

    public Actions punishAction()
    {
        List<Actions> offenseAction = new List<Actions>();
        offenseAction.Add(actions[0]);
        offenseAction.Add(actions[1]);
        offenseAction.Add(actions[4]);
        offenseAction.Add(actions[5]);
        Actions returnAction = actions[Random.Range(0, offenseAction.Count)];
        currentAction = returnAction;
        return returnAction;
    }

    public Actions predictAction()
    {
        
        Actions returnAction = actions[0];
        if (player.timeSinceLastAttack >= player.comboTime)
        {
            Debug.Log("First Attack!");
            lastAttackRecivedWasCombo = false;
            if (initialAttack.Length == 0)
            {
                returnAction = randomAction();
                return returnAction;
            }
            returnAction = SelectFromRow(initialAttack);


        }
        else if (player.timeSinceLastAttack < player.comboTime)
        {
            Debug.Log("Combo!");
            lastAttackRecivedWasCombo = true;
            int[] row = new int[6];
            for (int i = 0; i < attackTable[0].Length; i++)
            {
                try
                {
                    if (attackTable[previousIndex].Length == 0)
                    {
                        returnAction = randomAction();
                        return returnAction;
                    }

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
        player.timeSinceLastAttack = 0f;

        return returnAction;
    }

    private Actions SelectFromRow(int[] Row)
    {
        
        Actions predictedAction = actions[0];
        Actions returnAction;
        int total = 0;
        int highestCount = 0;
        int maxCount = 0;
        total = Row.Sum();

        // Finds out total entries in that row
        for (int i = 0; i < Row.Length; i++)
        {
            if (Row[i] > maxCount)
            {
                highestCount = i;
                maxCount = Row[i];
                if (total != 0)
                {
                    confidenceScore = Row[i] / total;
                }
            }
        }
        // Selects the highest value 
        if (algorithmType == AIType.Deterministic)
        {
            Debug.Log("Picking " + actions[highestCount].name + " with " + maxCount + " uses.");
            predictedAction = actions[highestCount];
        }
        // Selects the random value using 
        else
        {
            int randNum = Random.Range(0, total);
            int cumulativeSum = 0;
            
            for (int i = 0; i < Row.Length; i++)
            {
                cumulativeSum += Row[i];
                if (randNum < cumulativeSum)
                {
                    predictedAction = actions[i];

                    if (total != 0)
                    {
                        confidenceScore = Row[i] / total;
                    }
                    break;
                    
                }
            }

        }
        
        returnAction = SelectDefence(predictedAction);
        return returnAction;
    }

    public Actions SelectDefence(Actions predictedAction)
    {
        Debug.LogWarning("AI PREDICTS " + predictedAction.name);
        List<Actions> defences = new List<Actions>(predictedAction.defences);
        Actions selectedAction = defences[0];
        float closestDifference = Mathf.Abs(playerAggresionScore - selectedAction.aggresiveness);
        foreach (Actions action in predictedAction.defences)
        {
            Debug.Log("Counters are: " + action.name);
        }
        if (defences.Count == 0)
        {
            return predictedAction;
        }


        if (counterType == ResponseType.Random)
        {
            int randNum = Random.Range(0, defences.Count);
            selectedAction = defences[randNum];
        }
        else if (counterType == ResponseType.Weighted)
        {
            for (int i = 1; i < defences.Count; i++)
            {
                // Act more aggresively if confident
                float difference = Mathf.Abs((playerAggresionScore /*+ confidenceWeighting * confidenceWeighting * 2*/) - defences[i].aggresiveness);

                // Check if the current Move has a closer aggression value
                if (difference < closestDifference)
                {
                    selectedAction = defences[i];
                    closestDifference = difference;
                }
            }
            Debug.LogWarning("selected Action aggression is " + selectedAction.aggresiveness);
            Debug.LogWarning("aggresion score is  " + playerAggresionScore);


        }
        return selectedAction;
    }

    public void recordAction(Actions action)
    {
        if (action.ID > 6) { return; }
        playerAttackCount++;
        playerAggresionTotal += action.aggresiveness;
        playerAggresionScore = playerAggresionTotal / playerAttackCount;

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
