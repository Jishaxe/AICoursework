using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIScript))]
public class AgentSelector : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    SelectableAgent selectedAgent = null;
    SelectableAgent hoveredAgent = null;

    public delegate void AgentSelectedEvent(Agent agent);
    public event AgentSelectedEvent OnAgentSelected;
    public event AgentSelectedEvent OnAgentDeselected;

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // If an agent was hovered over
        if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Agent"))
        {
            SelectableAgent agent = hit.collider.gameObject.GetComponent<SelectableAgent>();

            agent.Hover();
            if (hoveredAgent != agent)
            {
                if (hoveredAgent != null) hoveredAgent.Unhover();
                hoveredAgent = agent;
            }

            // If an agent is clicked
            if (Input.GetMouseButtonUp(0))
            {
                // If this agent is not selected, deselect the previous agent (if exists) and select the new one
                if (!agent.isSelected)
                {
                    if (selectedAgent != null)
                    {
                        selectedAgent.Deselect();
                        OnAgentDeselected?.Invoke(selectedAgent.agent);
                    }

                    agent.Select();
                    OnAgentSelected?.Invoke(agent.agent);
                    selectedAgent = agent;
                }
                else // Agent is already selected and we clicked it, so deselect it
                {
                    selectedAgent = null;
                    agent.Deselect();
                    OnAgentDeselected?.Invoke(agent.agent);
                    agent.Hover();
                }
            }
        }
        else
        {
            if (hoveredAgent != null)
            {
                hoveredAgent.Unhover();
                hoveredAgent = null;
            }

            // If mouse was clicked somewhere on the screen and there is an agent selected, deselect the agent
            if (Input.GetMouseButtonUp(0))
            {
                if (selectedAgent != null)
                {
                    selectedAgent.Deselect();
                    OnAgentDeselected?.Invoke(selectedAgent.agent);
                    selectedAgent = null;
                }
            }
        }
    }
}
