using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    PlayerController player;
    public void Update()
    {

    }

    public void warn()
    {
        Actions predictedAction = predictAction();

        startAcion(predictedAction);
    }

    public Actions predictAction()
    {
        Actions returnAction = actions[Random.Range(0, 5)];
        return returnAction;
    }
}
