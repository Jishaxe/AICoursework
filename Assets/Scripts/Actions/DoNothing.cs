﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Actions
{
    [Serializable]
    public class DoNothing : UtilityAction
    {
        public override void Execute(Agent agent, World world, float executionTime)
        {
            throw new NotImplementedException();
        }
    }
}
