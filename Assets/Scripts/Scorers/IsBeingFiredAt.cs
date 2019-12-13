using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Scorers
{
    // returns percentage of bullets left
    public class IsBeingFiredAt : Scorer
    {
        public override float Evaluate(Agent agent, World world)
        {
            return agent.isBeingFiredAt ? 1 : -1;
        }
    }
}
