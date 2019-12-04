using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Scorers
{
    // Returns 1 if has flag, -1 if doesn't have flag
    public class HasFlag : Scorer
    {
        public override float Evaluate(Agent agent, World world)
        {
            return agent.hasFlag ? 1 : -1;
        }
    }
}
