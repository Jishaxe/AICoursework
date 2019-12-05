using UnityEngine;

namespace Assets.Scripts.Actions
{
    class ShootAtNearestEnemy : UtilityAction
    {
        public override void Execute(Agent agent, World world, float t)
        {
            Agent target = world.GetNearestEnemyTo(agent);
            agent.FireAtAgent(world, target, t);
        }

        public override string ToString()
        {
            return "ShootAtNearestEnemy";
        }
    }
}
