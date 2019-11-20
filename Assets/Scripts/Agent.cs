using Assets.Scripts.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Team
{
    RED, BLUE
}

public class Agent : MonoBehaviour
{
    public Team team;
    public string agentName;
    public float movementInATick = 1f;
    public Text nameText;
    public Text actionText;

    public UtilityAIModel model;
    public UtilityAction nextAction;

    public void Start()
    {
        nameText.text = agentName;
    }

    public void ChooseAction(World world)
    {
        EvaluatedActionWithScore[] evaluatedActions = model.EvaluateActions(this, world);
        
        nextAction = evaluatedActions[0].action;

        actionText.text = nextAction.ToString();
    }
}
