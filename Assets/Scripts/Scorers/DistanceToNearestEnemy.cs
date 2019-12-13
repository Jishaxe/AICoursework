using UnityEngine;

namespace Assets.Scripts.Scorers
{
    public class DistanceToNearestEnemy : Scorer
    {
        const int minDistance = 10;
        const int maxDistance = 30;

        public override float Evaluate(Agent agent, World world)
        {
            Agent nearestEnemy = world.GetNearestEnemyTo(agent);

            Vector3 direction = nearestEnemy.transform.position - agent.transform.position;

            float normalizedDistance = (direction.magnitude - minDistance) / (maxDistance - minDistance);
            return normalizedDistance;
        }
    }
}
