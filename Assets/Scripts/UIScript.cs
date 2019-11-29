using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AgentSelector))]
public class UIScript : MonoBehaviour
{
    public Text tickText;
    public World world;
    public Text playPauseText;
    AgentSelector agentSelector;

    public Text agentName;
    // Start is called before the first frame update
    void Start()
    {
        agentSelector = GetComponent<AgentSelector>();
        agentSelector.OnAgentSelected += OnAgentSelected;
    }

    public void OnAgentSelected(Agent agent)
    {
        ClearAgentDisplay();
        SetupAgentDisplay(agent);
    }

    public void SetupAgentDisplay(Agent agent)
    {
        if (agent == null) return;
        agentName.text = "Agent " + agent.agentName;
    }

    public void ClearAgentDisplay()
    {
        agentName.text = "No agent selected";
    }

    public void PlayPauseClicked()
    {
        if (!world.playing)
        {
            playPauseText.text = "Pause";
            world.playing = true;
        } else
        {
            playPauseText.text = "Play";
            world.playing = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        tickText.text = "Tick: " + world.tick;
    }
}
