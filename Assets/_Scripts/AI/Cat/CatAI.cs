using FMODUnity;
using UnityEngine;

namespace Assets._Scripts.AI.Cat
{
    [UnityComponent]
    public class CatAI : AIBase, ICanBeHitWithBroom
    {
        [AssignedInUnity, Range(0, 50)]
        public float DetectionRadius = 5;

        [AssignedInUnity, Range(0, 50)]
        public float MaxPounceDistance;

        [AssignedInUnity, Range(0, 30)]
        public float PounceSpeed;

        [AssignedInUnity, Range(0, 30)]
        public float ReturnSpeed;

        [AssignedInUnity]
        public int DamageDealt = 1;

        [EventRef]
        public string CatAngry;

        [EventRef]
        public string CatDying;

        [EventRef]
        public string CatHit;

        public Vector2 StartPosition { get; set; }

        [UnityMessage]
        public void Start()
        {
            SetState<Idle>();
        }

        protected override void SetUpStates()
        {
            AddState<Idle>();
            AddState<Pouncing>();
            AddState<Returning>();
        }

        protected override void UnityOnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") == false)
                return;

            var unitVectorToPlayer = transform.position.UnitVectorTo(collision.gameObject.transform.position);

            Player.Instance.TakeDamageAndPush(DamageDealt, unitVectorToPlayer);

            if (CurrentState is Pouncing)
                SetState<Returning>();
        }

        public void IsHitWithBroom()
        {
            RuntimeManager.PlayOneShot(CatDying);

            Destroy(gameObject);
        }
    }
}