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

    public Action nextAction;

    public void Start()
    {
        nameText.text = agentName;
    }

    public void ChooseAction(World world)
    {
        MoveTowards action = new MoveTowards();
        action.target = new Vector3(0, 0, 15);
        nextAction = action;

        actionText.text = nextAction.ToString();
    }
}
