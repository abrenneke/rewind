using UnityEngine;

namespace Assets._Scripts.AI.Mouse
{
    public class Attacking : AIState
    {
        private MouseAI MouseAI {get { return (MouseAI)AIBase; } }
        
        public override void Update()
        {
            var distanceToStart = MouseAI.transform.position.DistanceTo(MouseAI.StartPosition);

            if (distanceToStart >= MouseAI.IdleRange + MouseAI.AttackRange)
            {
                MouseAI.SetState<IdleRoaming>();
                return;
            }
        }

        public override void FixedUpdate()
        {
            MouseAI.MoveTo(Player.Instance.transform.position, MouseAI.AttackSpeed);
        }
    }
}