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


    // The red base - go back here with the flag
    public GameObject redBase;
    public GameObject flag;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Agent")) agents.Add(gameObject.GetComponent<Agent>());
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

    public void ChooseActions()
    {
        foreach (Agent agent in agents)
        {
            agent.ChooseAction(this);
        }
    }

    public void ExecuteActions()
    {
        foreach (Agent agent in agents)
        {
            agent.nextAction.Execute(agent, this, actionExecutionTime);
        }
    }
}
