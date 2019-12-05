using UnityEngine;

namespace Assets.Scripts.Actions
{
    class GetToCover : UtilityAction
    {
        public override void Execute(Agent agent, World world, float t)
        {
            if (agent.isInCover) return;
            Cover cover = world.GetNearestCoverTo(agent);
            if (cover == null) return;

            Vector3 direction = cover.coverPoint.transform.position - agent.transform.position;

            // tries to drop the flag at base if it's near enough
            agent.EnterCover(world, cover);

            if (!agent.isInCover) agent.MoveInDirection(world, direction, t);
        }

        public override string ToString()
        {
            return "GetToCover";
        }
    }
}
