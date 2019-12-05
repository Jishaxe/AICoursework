using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Scorers
{
    // Returns 1 if has ammo, -1 if doesn't have ammo
    public class HasAmmo : Scorer
    {
        public override float Evaluate(Agent agent, World world)
        {
            return agent.bullets > 0 ? 1 : -1;
        }
    }
}
