using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Scorers
{
    // returns percentage of bullets left
    public class AmmoLeft : Scorer
    {
        public override float Evaluate(Agent agent, World world)
        {
            return (float)agent.bullets / (float)agent.maxBullets; 
        }
    }
}
