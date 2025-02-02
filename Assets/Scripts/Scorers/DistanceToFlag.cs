﻿using UnityEngine;

namespace Assets.Scripts.Scorers
{
    public class DistanceToFlag : Scorer
    {
        const int minDistance = 0;
        const int maxDistance = 30;

        public override float Evaluate(Agent agent, World world)
        {
            GameObject flag = world.flag;

            Vector3 direction = flag.transform.position - agent.transform.position;

            float normalizedDistance = (direction.magnitude - minDistance) / (maxDistance - minDistance);

            return normalizedDistance;
        }
    }
}
