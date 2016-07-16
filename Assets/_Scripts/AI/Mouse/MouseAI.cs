using UnityEngine;

namespace Assets._Scripts.AI.Mouse
{
    [UnityComponent, RequireComponent(typeof(Rigidbody2D))]
    public class MouseAI : AIBase, ICanBeHitWithBroom
    {
        [AssignedInUnity]
        public float IdleSpeed;

        [AssignedInUnity]
        public float AttackSpeed;

        [AssignedInUnity]
        public float IdleRange;

        [AssignedInUnity]
        public float AttackRange;

        [AssignedInUnity]
        public float DetectionRange;

        [AssignedInUnity]
        public int DamageDealt;

        public Vector2 StartPosition { get; set; }

        [UnityMessage]
        public void Start()
        {
            StartPosition = transform.position;
            SetState<IdleRoaming>();
        }

        protected override void SetUpStates()
        {
            AddState<IdleRoaming>();
            AddState<Attacking>();
        }

        protected override void UnityOnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                SetState<IdleRoaming>();

                var unitVectorToPlayer = transform.position.UnitVectorTo(collision.gameObject.transform.position);

                Player.Instance.TakeDamageAndPush(DamageDealt, unitVectorToPlayer);
            }
        }

        public void IsHitWithBroom()
        {
            Destroy(gameObject);
        }
    }
}