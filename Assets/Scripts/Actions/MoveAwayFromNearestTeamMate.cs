using UnityEngine;

namespace Assets.Scripts.Actions
{
    class MoveAwayFromNearestTeamMate : UtilityAction
    {
        public override void Execute(Agent agent, World world, float t)
        {
            Agent nearestTeamMate = world.GetNearestTeamMateTo(agent);

            Vector3 direction = agent.transform.position - nearestTeamMate.transform.position;

            agent.MoveInDirection(world, direction, t);
        }
    }
}
