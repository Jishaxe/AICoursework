using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUIElement : MonoBehaviour
{
    public Text actionText;
    public Text totalScore;

    public Transform scorerContainer;
    public GameObject scorerPrefab;

    public void Setup(EvaluatedActionWithScore action)
    {
        actionText.text = action.action.GetType().Name;
        totalScore.text = action.score.ToString();

        foreach (EvaluatedScorerWithScore scorer in action.scorers)
        {
            GameObject scorerUI = Instantiate(scorerPrefab, scorerContainer);
            scorerUI.GetComponent<ScorerUIElement>().Setup(scorer);
        }
    }
}
