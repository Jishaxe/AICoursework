using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Actions
{
    public class UtilityAction: ScriptableObject
    {
        public virtual void Execute(Agent agent, World world, float executionTime) { throw new NotImplementedException(); }
    }
}
