using UnityEngine;

namespace Assets._Scripts.AI
{
    public abstract class AIState
    {
        public AIBase AIBase { get; set; }

        public bool IsActive { get; set; }

        protected Vector2 DesiredVelocity { get; set; }

        public virtual void Init()
        {
            
        }

        public virtual void Enter()
        {
            
        }

        public virtual void Exit()
        {
            
        }

        public virtual void Update()
        {
            
        }

        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            
        }

        public virtual void OnCollisionExit2D(Collision2D collision)
        {
            
        }

        public virtual bool AllowStateChangeFrom()
        {
            return true;
        }

        public virtual void FixedUpdate()
        {
            AIBase.Move(DesiredVelocity);
        }

        protected void StopMoving()
        {
            DesiredVelocity = Vector2.zero;
        }
    }
}