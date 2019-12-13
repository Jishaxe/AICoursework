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
    public bool zeroWhenNegative = false;
    public bool zeroWhenPositive = false;
    public bool disregardWhenNegative = false;
    public bool disregardWhenPositive = false;

    // Evaluate the Scorer then multiply the result by the multiplier
    public float EvaluateAndTransform(Agent agent, World world)
    {
        float result = scorer.Evaluate(agent, world);
        if (negate) result = 1 - result;


        result *= multiplier;
       
        if (zeroWhenNegative && result < 0) result = 0;
        if (zeroWhenPositive && result > 0) result = 0;

        if (disregardWhenNegative && result <= 0) result = -100;
        if (disregardWhenPositive && result > 0) result = -100;
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
        result.score = sum;// / scorers.Count;
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
    public static int oscilliationScore = 1;

    public static Type[] actionTypes =
    {
        typeof(DoNothing),
        typeof(MoveTowardsAndGetFlag),
        typeof(ReturnFlagToBase),
        typeof(ShootAtNearestEnemy),
        typeof(GetToCover),
        typeof(Reload),
        typeof(MoveAwayFromNearestEnemy),
        typeof(MoveAwayFromNearestTeamMate)
    };

    public static Type[] scorerTypes =
    {
        typeof(HasFlag),
        typeof(DistanceToFlag),
        typeof(HasAmmo),
        typeof(IsInCover),
        typeof(AmmoLeft),
        typeof(IsBeingFiredAt),
        typeof(DistanceToNearestEnemy),
        typeof(EnemyHasFlag),
        typeof(TeamMateHasFlag),
        typeof(DistanceToNearestCover),
        typeof(DistanceToNearestTeamMate)
    };


    public List<ActionAndScorers> actions = new List<ActionAndScorers>();

    public EvaluatedActionWithScore[] EvaluateActions(Agent agent, World world, string lastAction)
    {
        List<EvaluatedActionWithScore> result = new List<EvaluatedActionWithScore>();

        foreach (ActionAndScorers action in actions)
        {
            EvaluatedActionWithScore evaled = action.Evaluate(agent, world);
            if (evaled.action.GetType().Name == lastAction) evaled.score += oscilliationScore;
            result.Add(evaled);
        }

        result = result.OrderByDescending(o => o.score).ToList();
        return result.ToArray();
    }
}
