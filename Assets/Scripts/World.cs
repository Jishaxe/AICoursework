using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    // List of agents
    public List<Agent> agents;

    // Current tick amount
    public int tick = 0;

    // How long it takes to execute the actions
    public float actionExecutionTime = 1f;

    // Time left before we can go to the next step
    public float timeLeftToExecute = 0f;

    // If this is true, continually go next
    public bool playing = false;


    // Bases and flag
    public GameObject redBase;
    public GameObject blueBase;
    public GameObject flag;

    // The agent that currently has the flag
    public Agent agentWithFlag = null;

    public Vector3 flagResetPosition;

    // Start is called before the first frame update
    void Start()
    {
        flagResetPosition = flag.transform.position;
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Agent"))
        {
            Agent agent = gameObject.GetComponent<Agent>();
            agent.thisBase = GetBase(agent.team);
            agents.Add(agent);
        }
    }

    public void ResetFlag()
    {
        if (agentWithFlag != null)
        {
            agentWithFlag.hasFlag = false;
            flag.transform.SetParent(null);
            agentWithFlag = null;
        }

        flag.transform.position = flagResetPosition;
    }

    public void Update()
    {
        if (timeLeftToExecute > 0) timeLeftToExecute -= Time.deltaTime;

        if (playing && timeLeftToExecute <= 0) Next();
    }

    public void Next()
    {
        // Don't go next if the actions are still executing
        if (timeLeftToExecute > 0) return;
        timeLeftToExecute = actionExecutionTime;

        tick++;
        ChooseActions();
        ExecuteActions();
    }


    public Agent GetNearestEnemyTo(Agent agent)
    {
        Agent result = null;
        float closestDistance = Mathf.Infinity;

        foreach (Agent possible in agents)
        {
            if (possible.team == agent.team) continue; // skip members of same team
            if (possible.health <= 0) continue; // ignore dead agents

            if ((possible.transform.position - agent.transform.position).magnitude < closestDistance)
            {
                result = possible;
                closestDistance = (possible.transform.position - agent.transform.position).magnitude;
            }
        }

        return result;
    }

    public GameObject GetBase(Team team)
    {
        if (team == Team.RED) return redBase;
        if (team == Team.BLUE) return blueBase;
        return null;
    }

    public void ChooseActions()
    {
        foreach (Agent agent in agents)
        {
            if (agent.health <= 0) continue;
            agent.ChooseAction(this);
        }
    }

    public void ExecuteActions()
    {
        foreach (Agent agent in agents)
        {
            if (agent.health <= 0) continue;
            agent.nextAction.Execute(agent, this, actionExecutionTime);
        }
    }
}
