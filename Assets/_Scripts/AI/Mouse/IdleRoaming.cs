using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Scripts.AI.Mouse
{
    public class IdleRoaming : AIState
    {
        private MouseAI MouseAI {get { return (MouseAI)AIBase; } }

        private Vector2? targetPosition;

        private Coroutine newPositionCoroutine;

        public override void Update()
        {
            if (targetPosition == null)
            {
                GetNewIdleTargetPosition();
            }

            if (targetPosition == null)
                throw new InvalidOperationException("Error");

            if (MouseAI.HasReached(targetPosition.Value))
            {
                targetPosition = null;
            }

            var distanceToCenter = MouseAI.transform.position.DistanceTo(MouseAI.StartPosition);

            // Only attack if within idle range
            if (distanceToCenter <= MouseAI.IdleRange)
            {
                var distanceToPlayer = MouseAI.transform.position.DistanceTo(Player.Instance.transform.position);

                if (distanceToPlayer <= MouseAI.DetectionRange)
                {
                    MouseAI.SetState<Attacking>();
                }
            }
        }

        private IEnumerator GetNewPositionAfter3Seconds()
        {
            yield return new WaitForSeconds(3);
            targetPosition = null;
        }

        private void GetNewIdleTargetPosition()
        {
            var angle = Random.Range(0, 360);
            var distance = Random.value * MouseAI.IdleRange;

            var vector = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

            targetPosition = MouseAI.StartPosition + vector;

            if (newPositionCoroutine != null)
                MouseAI.StopCoroutine(newPositionCoroutine);

            // To prevent getting stuck.
            newPositionCoroutine = MouseAI.StartCoroutine(GetNewPositionAfter3Seconds());
        }

        public override void FixedUpdate()
        {
            if (targetPosition != null)
            {
                MouseAI.MoveTo(targetPosition.Value, MouseAI.IdleSpeed);
            }
        }
    }
}