using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Actions
{
    public class Action
    {
        public virtual void Execute(Agent agent, World world) { }

        public static void MoveInDirection(Agent agent, Vector3 direction)
        {
            direction.Normalize();
            direction *= agent.movementInATick;

            agent.transform.position = agent.transform.position + direction;
        }
    }
}
