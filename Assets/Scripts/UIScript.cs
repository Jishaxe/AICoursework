using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AgentSelector))]
public class UIScript : MonoBehaviour
{
    public Text tickText;
    public World world;
    public Text playPauseText;
    AgentSelector agentSelector;
    public Transform actionList;
    public GameObject actionUIPrefab;

    public Text agentName;
    // Start is called before the first frame update
    void Start()
    {
        agentSelector = GetComponent<AgentSelector>();
        agentSelector.OnAgentSelected += OnAgentSelected;
        agentSelector.OnAgentDeselected += OnAgentDeselected;
    }

    public void OnAgentDeselected(Agent agent)
    {
        agent.OnEvaluatedActions -= UpdateAgentDisplay;
        ClearAgentDisplay();
    }

    public void OnAgentSelected(Agent agent)
    {
        agent.OnEvaluatedActions += UpdateAgentDisplay;
        SetupAgentDisplay(agent);
    }

    public void SetupAgentDisplay(Agent agent)
    {
        agentName.text = "Agent " + agent.agentName;

        if (agent.lastEvaluatedActions != null) UpdateAgentDisplay(agent.lastEvaluatedActions);
    }

    public void UpdateAgentDisplay(EvaluatedActionWithScore[] evaluatedActions)
    {
        // clear the current action list
        foreach (Transform child in actionList) Destroy(child.gameObject);

        foreach (EvaluatedActionWithScore action in evaluatedActions)
        {
            GameObject actionUI = Instantiate(actionUIPrefab, actionList);
            actionUI.GetComponent<ActionUIElement>().Setup(action);
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(actionList.GetComponent<RectTransform>());
    }

    public void ClearAgentDisplay()
    {
        agentName.text = "No agent selected";
        // clear the current action list
        foreach (Transform child in actionList) Destroy(child.gameObject);
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

    public void RestartButtonClicked()
    {
        SceneManager.LoadScene("Main");
    }

    // Update is called once per frame
    void Update()
    {
        tickText.text = "Tick: " + world.tick;
    }
}
