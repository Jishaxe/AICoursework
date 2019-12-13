namespace Assets.Scripts.Scorers
{
    // Returns 1 if has ammo, -1 if doesn't have ammo
    public class EnemyHasFlag : Scorer
    {
        public override float Evaluate(Agent agent, World world)
        {
            return world.agentWithFlag != null && world.agentWithFlag.team != agent.team ? 1 : -1;
        }
    }
}
