using UnityEngine;

namespace Assets.Scripts.Scorers
{
    public class DistanceToNearestCover : Scorer
    {
        const int minDistance = 0;
        const int maxDistance = 30;

        public override float Evaluate(Agent agent, World world)
        {
            Cover nearestCover = world.GetNearestCoverTo(agent);
            if (nearestCover == null) return 1;

            Vector3 direction = nearestCover.transform.position - agent.transform.position;

            float normalizedDistance = (direction.magnitude - minDistance) / (maxDistance - minDistance);
            return normalizedDistance;
        }
    }
}
