using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Scripts.AI
{
    [UnityComponent, RequireComponent(typeof(Rigidbody2D))]
    public abstract class AIBase : MonoBehaviour
    {
        public AIState CurrentState { get; private set; }

        public AIState PreviousState { get; private set; }

        public Vector2 LastDesiredVelocity { get; private set; }

        private IList<AIState> states;

        private new Rigidbody2D rigidbody;

        [UnityMessage]
        public void Awake()
        {
            states = new List<AIState>();
            rigidbody = GetComponent<Rigidbody2D>();

            SetUpStates();
            PostAwake();
        }

        [UnityMessage]
        public void Update()
        {
            if (CurrentState != null)
                CurrentState.Update();

            PostUpdate();
        }

        protected virtual void PostUpdate()
        {
            
        }

        [UnityMessage]
        public void FixedUpdate()
        {
            if (CurrentState != null)
                CurrentState.FixedUpdate();

            if (rigidbody.velocity.sqrMagnitude > 0)
                rigidbody.velocity = Vector2.zero;

            PostFixedUpdate();
        }

        protected virtual void PostFixedUpdate()
        {
            
        }

        protected virtual void PostAwake()
        {
        }

        [UnityMessage]
        public void OnCollisionEnter2D(Collision2D collision)
        {
            UnityOnCollisionEnter2D(collision);

            if (CurrentState != null)
                CurrentState.OnCollisionEnter2D(collision);
        }

        [UnityMessage]
        public void OnCollisionExit2D(Collision2D collision)
        {
            UnityOnCollisionExit2D(collision);

            if (CurrentState != null)
                CurrentState.OnCollisionExit2D(collision);
        }

        protected virtual void UnityOnCollisionEnter2D(Collision2D collision)
        {

        }

        protected virtual void UnityOnCollisionExit2D(Collision2D collision)
        {
            
        }

        protected abstract void SetUpStates();

        protected void AddState<T>() where T : AIState, new()
        {
            var state = new T { AIBase = this };

            states.Add(state);
            state.Init();
        }

        public void SetState<T>() where T : AIState
        {
            var newState = GetState<T>();
            SetState(newState);
        }

        public void SetState(AIState newState)
        {
            if (CurrentState == newState)
                return;

            if (CurrentState != null)
            {
                if (CurrentState.AllowStateChangeFrom() == false)
                    return;

                CurrentState.Exit();
                CurrentState.IsActive = false;
            }

            PreviousState = CurrentState;
            CurrentState = newState;

            CurrentState.IsActive = true;
            CurrentState.Enter();
        }

        public T GetState<T>() where T : AIState
        {
            return states.OfType<T>().FirstOrDefault();
        }

        public void Move(Vector2 velocity)
        {
            LastDesiredVelocity = velocity;

            if (velocity.IsZero())
                return;

            var movement = velocity * Time.deltaTime;

            rigidbody.MovePosition(transform.position + (Vector3)movement);
        }

        public void MoveTo(Vector3 position, float speed)
        {
            var distanceToStart = transform.position.DistanceTo(position);

            var unitToStartingPosition = transform.position.UnitVectorTo(position);

            var toStartVelocity = (unitToStartingPosition * distanceToStart) / Time.deltaTime;
            var fullVelocity = unitToStartingPosition * speed;

            Move(toStartVelocity.sqrMagnitude < fullVelocity.sqrMagnitude ? toStartVelocity : fullVelocity);
        }

        public bool HasReached(Vector3 position)
        {
            return transform.position.DistanceTo(position) < 0.1f;
        }
    }
}