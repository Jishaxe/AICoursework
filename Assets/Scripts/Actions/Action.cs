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

        // Lerps in a given direction across executionTime, also facing the agent in that direction
        public static IEnumerator MoveInDirection(Agent agent, Vector3 direction, float executionTime)
        {
            if (direction.magnitude > 1f)
            {
                direction.Normalize();
                direction *= agent.movementInATick;
            }

            Vector3 startPosition = agent.transform.position;
            Vector3 endPosition = startPosition + direction;

            Quaternion startRotation = agent.transform.rotation;
            Quaternion endRotation = Quaternion.LookRotation(direction);

            float time = 0f;

            while (time < executionTime)
            {
                Vector3 lerpedPosition = Vector3.Lerp(startPosition, endPosition, time / executionTime);
                Quaternion lerpedRotation = Quaternion.Lerp(startRotation, endRotation, time / executionTime);

                agent.transform.position = lerpedPosition;
                agent.transform.rotation = lerpedRotation;
                time += Time.deltaTime;

                yield return null;
            }

            agent.transform.position = endPosition;
        }
    }
}
