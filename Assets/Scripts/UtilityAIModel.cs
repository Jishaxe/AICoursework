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
public class ScorerAndTransformer
{
    public Scorer scorer;
    public float multiplier = 1f;
    public bool negate = false;

    // Evaluate the Scorer then multiply the result by the multiplier
    public float EvaluateAndTransform(Agent agent, World world)
    {
        float result = scorer.Evaluate(agent, world) * multiplier;
        if (negate) result *= -1;
        return result;
    }
}

[Serializable]
public class ActionAndScorers
{
    public UtilityAction action;

    // All scorers and their multiplier
    public List<ScorerAndTransformer> scorers = new List<ScorerAndTransformer>();

    // Evaluates and averages all the score results after they're transformed
    public EvaluatedActionWithScore Evaluate(Agent agent, World world)
    {
        float sum = 0;

        List<EvaluatedScorerWithScore> evaledScorers = new List<EvaluatedScorerWithScore>();

        foreach (ScorerAndTransformer scorer in scorers)
        {
            EvaluatedScorerWithScore scorerResult = new EvaluatedScorerWithScore();
            scorerResult.scorer = scorer;
            scorerResult.score = scorer.EvaluateAndTransform(agent, world);
            sum += scorerResult.score;
            evaledScorers.Add(scorerResult);
        }

        EvaluatedActionWithScore result = new EvaluatedActionWithScore();
        result.action = action;
        result.score = sum / scorers.Count;
        result.scorers = evaledScorers.ToArray();

        return result;
    }
}

public class EvaluatedScorerWithScore
{
    public ScorerAndTransformer scorer;
    public float score;
}

public class EvaluatedActionWithScore
{
    public UtilityAction action;
    public EvaluatedScorerWithScore[] scorers;
    public float score;
}


[CreateAssetMenu(fileName = "New UtilityAIModel", menuName = "Utility AI Model")]
public class UtilityAIModel : ScriptableObject
{
    public static Type[] actionTypes =
    {
        typeof(DoNothing),
        typeof(MoveTowardsAndGetFlag),
        typeof(ReturnFlagToBase),
        typeof(ShootAtNearestEnemy)
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
            EvaluatedActionWithScore evaled = action.Evaluate(agent, world);
            result.Add(evaled);
        }

        result = result.OrderByDescending(o => o.score).ToList();
        return result.ToArray();
    }
}
