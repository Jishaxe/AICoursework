using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Actions
{
    class MoveTowards: UtilityAction
    {
        public Vector3 target;

        public override void Execute(Agent agent, World world, float t) {
            Vector3 direction = target - agent.transform.position;

            world.StartCoroutine(MoveInDirection(agent, direction, t));
        }

        public override string ToString()
        {
            return "MoveTowards " + target.ToString();
        }
    }
}
