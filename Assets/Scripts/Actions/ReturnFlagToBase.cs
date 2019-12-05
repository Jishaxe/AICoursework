using UnityEngine;

namespace Assets.Scripts.Actions
{
    class ReturnFlagToBase : UtilityAction
    {
        public override void Execute(Agent agent, World world, float t)
        {
            Vector3 direction = agent.thisBase.transform.position - agent.transform.position;

            // tries to drop the flag at base if it's near enough
            agent.DropFlagAtBase(world);

            agent.MoveInDirection(world, direction, t);
        }

        public override string ToString()
        {
            return "ReturnFlagToBase";
        }
    }
}
