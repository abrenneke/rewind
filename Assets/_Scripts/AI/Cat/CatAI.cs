﻿using UnityEngine;

namespace Assets._Scripts.AI.Cat
{
    [UnityComponent]
    public class CatAI : AIBase
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
        }
    }
}