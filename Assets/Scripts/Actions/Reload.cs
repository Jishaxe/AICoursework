using UnityEngine;

namespace Assets.Scripts.Actions
{
    class Reload : UtilityAction
    {
        public override void Execute(Agent agent, World world, float t)
        {
            agent.Reload();
        }
    }
}
