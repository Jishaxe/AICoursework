namespace Assets.Scripts.Scorers
{
    // Returns 1 if has ammo, -1 if doesn't have ammo
    public class IsInCover : Scorer
    {
        public override float Evaluate(Agent agent, World world)
        {
            return agent.isInCover ? 1 : -1;
        }
    }
}
