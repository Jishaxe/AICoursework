using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public List<Agent> agents;
    public int tick = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Agent")) agents.Add(gameObject.GetComponent<Agent>());
    }

    public void Next()
    {
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
            agent.nextAction.Execute(agent, this);
        }
    }
}
