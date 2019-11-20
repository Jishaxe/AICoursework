using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public abstract class Scorer: ScriptableObject
    {
        public abstract float Evaluate(Agent agent, World world);
    }
}
