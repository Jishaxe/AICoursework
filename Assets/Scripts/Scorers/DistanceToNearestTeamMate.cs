using UnityEngine;

namespace Assets.Scripts.Scorers
{
    public class DistanceToNearestTeamMate : Scorer
    {
        const int minDistance = 10;
        const int maxDistance = 30;

        public override float Evaluate(Agent agent, World world)
        {
            Agent nearestTeamMate = world.GetNearestTeamMateTo(agent);

            Vector3 direction = nearestTeamMate.transform.position - agent.transform.position;

            float normalizedDistance = (direction.magnitude - minDistance) / (maxDistance - minDistance);
            return normalizedDistance;
        }
    }
}
