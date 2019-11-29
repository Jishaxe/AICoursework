using Assets.Scripts;
using Assets.Scripts.Actions;
using Assets.Scripts.Scorers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Holds a Scorer and a way to transform the Scorer result. For now, it is just multiplied by a scalar.
[Serializable]
public class ScorerAndTransformer: ScriptableObject {
    public Scorer scorer;
    public float multiplier = 1f;

    // Evaluate the Scorer then multiply the result by the multiplier
    public float EvaluateAndTransform(Agent agent, World world)
    {
        //aa
        return scorer.Evaluate(agent, world) * multiplier;
    }
}

[Serializable]
public class ActionAndScorers: ScriptableObject
{
    public UtilityAction action;

    // All scorers and their multiplier
    public List<ScorerAndTransformer> scorers = new List<ScorerAndTransformer>();

    // Evaluates and averages all the score results after they're transformed
    public float Evaluate(Agent agent, World world)
    {
        float sum = 0;

        foreach (ScorerAndTransformer scorer in scorers)
        {
            sum += scorer.EvaluateAndTransform(agent, world);
        }

        return sum / scorers.Count;
    }
}


public class EvaluatedActionWithScore: ScriptableObject
{
    public UtilityAction action;
    public float score;
}


[CreateAssetMenu(fileName = "New UtilityAIModel", menuName = "Utility AI Model")]
[Serializable]
public class UtilityAIModel : ScriptableObject
{
    public static Type[] actionTypes =
    {
        typeof(DoNothing),
        typeof(MoveTowards)
    };

    public static Type[] scorerTypes =
    {
        typeof(HasFlag),
        typeof(DistanceToFlag)
    };


    public List<ActionAndScorers> actions = new List<ActionAndScorers>();

    public EvaluatedActionWithScore[] EvaluateActions(Agent agent, World world)
    {
        List<EvaluatedActionWithScore> result = new List<EvaluatedActionWithScore>();

        foreach (ActionAndScorers action in actions)
        {
            float score = action.Evaluate(agent, world);
            EvaluatedActionWithScore evaled = new EvaluatedActionWithScore();
            evaled.score = score;
            evaled.action = action.action;
            result.Add(evaled);
        }

        result = result.OrderBy(o => o.score).ToList();
        return result.ToArray();
    }
}
