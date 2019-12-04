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
    public delegate void EvaluationEvent(EvaluatedActionWithScore[] evaluatedActions);
    public event EvaluationEvent OnEvaluatedActions;
    public Team team;
    public string agentName;
    public GameObject thisBase;

    [Header("How many units this agent can move in one tick")]
    public float movementInATick = 1f;

    [Header("Text for the UI that pops up above tha player")]
    public Text nameText;
    public Text actionText;

    [Header("Model to use")]
    public UtilityAIModel model;
    public UtilityAction nextAction;
    public EvaluatedActionWithScore[] lastEvaluatedActions;

    [Header("How close this agent needs to be before they can pick up the flag")]
    public float closenessBeforePickingUpFlag;

    [Header("Where this agent holds the flag")]
    public Transform flagHoldPoint;

    public bool hasFlag = false;


    public void Start()
    {
        nameText.text = agentName;
    }




    // Picks up the flag if it's close enough to this agent
    // Sets the parent to this flagHoldPoint
    public void GetFlag(World world)
    {
        // Don't do anything if we already have the flag or another agent has it
        if (this.hasFlag) return;
        if (world.agentWithFlag != null) return;

        if ((world.flag.transform.position - this.transform.position).magnitude <= closenessBeforePickingUpFlag)
        {
            world.agentWithFlag = this;
            this.hasFlag = true;
            world.flag.transform.SetParent(flagHoldPoint, false);
        }
    }

    public void DropFlagAtBase(World world)
    {
        if (!this.hasFlag) return;

        if ((thisBase.transform.position - this.transform.position).magnitude <= closenessBeforePickingUpFlag)
        {
            world.ResetFlag();
        }
    }

    // Drops the flag at this position
    public void DropFlag(World world)
    {
        if (!this.hasFlag) return;

        this.hasFlag = false;
        world.agentWithFlag = null;
        world.flag.transform.SetParent(null, true);
        world.flag.transform.position = new Vector3(world.flag.transform.position.x, world.flagResetPosition.y, world.flag.transform.position.z);
    }

    public void ChooseAction(World world)
    {
        EvaluatedActionWithScore[] evaluatedActions = model.EvaluateActions(this, world);
        
        nextAction = evaluatedActions[0].action;

        actionText.text = nextAction.ToString();

        lastEvaluatedActions = evaluatedActions;

        OnEvaluatedActions?.Invoke(evaluatedActions);
    }
}
