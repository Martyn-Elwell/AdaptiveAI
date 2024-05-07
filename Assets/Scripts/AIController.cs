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
    [SerializeField] public Actions savedPredictedAction;
    [SerializeField] public float correctPredictions = 0;
    [SerializeField] public float totalPredictions = 0;
    [SerializeField] public float playerAttackCount = 0;
    [SerializeField] private float playerAggresionTotal = 0;
    [SerializeField] private float playerAggresionScore = 0;
    [SerializeField] public float aggresionWeighting = 1;
    [SerializeField] private float confidenceScore = 0;
    [SerializeField] public float confidenceWeighting = 1;
    [SerializeField] private AIType algorithmType = AIType.Probabilistic;
    [SerializeField] private ResponseType counterType = ResponseType.Random;
    [SerializeField] public int[] initialAttack = new int[6];
    [SerializeField] public int[,] attackTable = new int[6,6];
    private bool lastAttackRecivedWasCombo = false;
    private int previousIndex = 0;

    [Header ("Debug Featrues")]
    public bool debugOverride = false;
    [Range(0, 5)] public int debugOverrideNum;
    public void Start()
    {

    }

    public void Update()
    {

        MovementHandler();
    }

    public void MovementHandler()
    {
        float distance = transform.position.z - player.transform.position.z;
        if (Mathf.Abs(distance - idealDistance) >= 0.2f)
        {
            animator.SetFloat("Direction", distance - idealDistance);
        }
        else
        {
            animator.SetFloat("Direction", 0f);
        }
    }


    public override void StartAction(Actions action)
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
        Actions predictedAction = new Actions();

        // Calls prediction algorithm if not stunned
        if (!stunned)
        {
            predictedAction = predictAction();
        }
        // If stunned, do not predict action
        // Set your action to stunned and disable the stun 
        else
        {
            stunned = false;
            predictedAction = actions[7];
        }
        currentAction = predictedAction;
        StartAction(predictedAction);
        return predictedAction;
    }

    public Actions punish()
    {
        Actions action = punishAction();
        //inCombat = false;
        currentAction = action;
        StartAction(action);
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
        Actions returnAction = actions[Random.Range(0, offenseAction.Count)];
        currentAction = returnAction;
        return returnAction;
    }

    public Actions predictAction()
    {
        Actions returnAction = actions[0];


        // Player is attack without a combo
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

        // Player is attacking with combo
        else if (player.timeSinceLastAttack < player.comboTime)
        {
            Debug.Log("Combo!");
            lastAttackRecivedWasCombo = true;

            if (attackTable.Length == 0)
            {
                returnAction = randomAction();
                return returnAction;
            }

            int[] row = new int[6];
            for (int i = 0; i < 6; i++)
            {
                try
                {
                    row[i] = attackTable[previousIndex, i];
                }
                catch { }
                
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
        savedPredictedAction = predictedAction;

        totalPredictions++;
        returnAction = SelectDefence(predictedAction);
        return returnAction;
    }

    public Actions SelectDefence(Actions predictedAction)
    {
        List<Actions> defences = new List<Actions>(predictedAction.defences);
        Actions selectedAction = defences[0];
        float closestDifference = Mathf.Abs(playerAggresionScore - selectedAction.aggresiveness);


        if (defences.Count == 0)
        {
            return predictedAction;
        }

        // Select defence randomly from predicted actions defences
        if (counterType == ResponseType.Random)
        {
            int randNum = Random.Range(0, defences.Count);
            selectedAction = defences[randNum];
        }
        // Select defence using weighted selction
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
        }
        return selectedAction;
    }

    public void CorrectPrediction()
    {
        correctPredictions++;
        float predictionPercent = correctPredictions / totalPredictions;
        UI.UpdateAccuracy(correctPredictions, predictionPercent);
    }
    public void InCorrectPrediction()
    {
        float predictionPercent = correctPredictions / totalPredictions;
        UI.UpdateAccuracy(correctPredictions, predictionPercent);
    }
    public void recordAction(Actions action)
    {
        // Non move set action for example stunned
        if (action.ID > 6) { return; }
        
        // Statistic tracking
        playerAttackCount++;
        playerAggresionTotal += action.aggresiveness;
        playerAggresionScore = playerAggresionTotal / playerAttackCount;

        // Record Non Combo Action
        if (lastAttackRecivedWasCombo == false)
        {
            initialAttack[action.ID] += 1;
            previousIndex = action.ID;
        }
        // Record Combo-ed action
        else if (lastAttackRecivedWasCombo == true)
        {
            attackTable[previousIndex,action.ID] += 1;
        }
    }



    public Actions randomAction()
    {
        Actions returnAction = actions[Random.Range(0, 5)];
        if (debugOverride) { returnAction = actions[debugOverrideNum]; }
        return returnAction;
    }




}
