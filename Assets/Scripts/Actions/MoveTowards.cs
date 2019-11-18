using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Actions
{
    class MoveTowards: Action
    {
        public Vector3 target;

        public override void Execute(Agent agent, World world) {
            Vector3 direction = target - agent.transform.position;

            MoveInDirection(agent, direction);
        }

        public override string ToString()
        {
            return "MoveTowards " + target.ToString();
        }
    }
}
