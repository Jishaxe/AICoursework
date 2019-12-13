using UnityEngine;

namespace Assets.Scripts.Actions
{
    class MoveTowardsAndGetFlag : UtilityAction
    {
        public override void Execute(Agent agent, World world, float t)
        {
            Vector3 direction = world.flag.transform.position - agent.transform.position;

            // tries to get the flag if it's near enough
            agent.GetFlag(world);

            if (!agent.hasFlag) agent.MoveInDirection(world, direction, t);
        }
    }
}
