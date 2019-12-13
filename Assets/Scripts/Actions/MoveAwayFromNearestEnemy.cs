using UnityEngine;

namespace Assets.Scripts.Actions
{
    class MoveAwayFromNearestEnemy : UtilityAction
    {
        public override void Execute(Agent agent, World world, float t)
        {
            Agent nearestEnemy = world.GetNearestEnemyTo(agent);

            Vector3 direction = agent.transform.position - nearestEnemy.transform.position;

            agent.MoveInDirection(world, direction, t);
        }
    }
}
