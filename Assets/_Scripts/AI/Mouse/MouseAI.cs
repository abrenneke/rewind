using GAF.Core;
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

        [AssignedInUnity]
        public GAFMovieClip Horizontal;

        [AssignedInUnity]
        public GAFMovieClip Down;

        [AssignedInUnity]
        public GAFMovieClip Up;

        public Vector2 StartPosition { get; set; }

        [UnityMessage]
        public void Start()
        {
            MapController.Instance.RegisterThingToClean(gameObject);
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

        protected override void PostUpdate()
        {
            CheckSpriteDirection();
        }

        public void IsHitWithBroom()
        {
            MapController.Instance.ThingCleaned(gameObject);

            Destroy(gameObject);
        }

        private void CheckSpriteDirection()
        {
            var movementDirection = new Vector3().DirectionToDegrees(LastDesiredVelocity);

            if (movementDirection < 0)
                movementDirection += 360;

            if (movementDirection > 360)
                movementDirection -= 360;

            var sideActive = false;
            var frontActive = false;
            var backActive = false;
            bool? flipSide = null;

            if (movementDirection >= 315 || movementDirection < 45)
            {
                // Right
                sideActive = true;
                flipSide = false;
            }
            else if (movementDirection >= 45 && movementDirection < 135)
            {
                // Up
                backActive = true;
            }
            else if (movementDirection >= 135 && movementDirection < 225)
            {
                // Left
                sideActive = true;
                flipSide = true;
            }
            else if (movementDirection >= 225 && movementDirection < 315)
            {
                // Down
                frontActive = true;
            }

            if (Horizontal.gameObject.activeSelf != sideActive)
                Horizontal.gameObject.SetActive(sideActive);

            if (Down.gameObject.activeSelf != frontActive)
                Down.gameObject.SetActive(frontActive);

            if (Up.gameObject.activeSelf != backActive)
                Up.gameObject.SetActive(backActive);

            if (flipSide.HasValue)
            {
                var scale = Horizontal.transform.localScale;
                if (flipSide.Value && Horizontal.transform.localScale.x > 0)
                {
                    Horizontal.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
                }
                else if (!flipSide.Value && Horizontal.transform.localScale.x < 0)
                {
                    Horizontal.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
                }
            }

            if (LastDesiredVelocity.IsZero())
            {
                Horizontal.stop();
                Down.stop();
                Up.stop();
            }
            else
            {
                if (Horizontal.isPlaying() == false)
                    Horizontal.play();
                if (Down.isPlaying() == false)
                    Down.play();
                if (Up.isPlaying() == false)
                    Up.play();
            }
        }

        [UnityMessage]
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<BroomCollision>() != null)
                IsHitWithBroom();
        }
    }
}