using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorerUIElement : MonoBehaviour
{
    public Text scorerName;
    public Text score;


    public void Setup(EvaluatedScorerWithScore scorer)
    {
        scorerName.text = scorer.scorer.scorer.GetType().Name;
        score.text = scorer.score.ToString();
    }
}
