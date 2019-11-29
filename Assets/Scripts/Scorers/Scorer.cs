using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Scorer: ScriptableObject
    {
        public virtual float Evaluate(Agent agent, World world) { throw new NotImplementedException();  }
    }
}
