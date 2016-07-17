using System;
using System.Linq;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using GAF.Core;
using JetBrains.Annotations;

namespace Assets._Scripts
{
    [UnityComponent, RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(NoVelocity))]
    public class Player : MonoBehaviour
    {
        public event Action<int> HealthChanged;

        public event Action<int> PillsChanged;

		[EventRef]
		public string SoundWalkEventName = "event:/footsteps_carpet";
        private EventInstance walkSound;

        [EventRef]
		public string SoundDoorOpenEventName = "event:/Door_Open";

        [EventRef, UsedImplicitly]
        public string SoundBroomSweep;

        [EventRef, UsedImplicitly]
        public string Hit;

        [EventRef, UsedImplicitly]
        public string Heal;
        
		public static Player Instance { get; private set; }

        [AssignedInUnity]
        public Transform Rotation;

        [AssignedInUnity, Range(0, 10)]
        public float MoveSpeed = 5;

        [AssignedInUnity]
        public int MaxHealth = 6;

        [AssignedInUnity]
        public int Health = 6;

        [AssignedInUnity]
        public int RecoilFrames;

        [AssignedInUnity]
        public float RecoilSpeed = 20;

        [AssignedInUnity, Range(0, 2)]
        public float AttackDelayTime = 1;

        [AssignedInUnity, Range(0, 5)]
        public float AttackRange = 1;

        public int NumPills;

        private Vector2 desiredMovement;
        private new Rigidbody2D rigidbody;

        private bool isRecoiling;
        private Vector2 recoilDirection;
        private int recoilFrameCounter;

        private bool canTransition = true;
        private bool canAttack = true;

        private Vector2 lastMovement;

        [AssignedInUnity]
        public GAFMovieClip SideWalk;

        [AssignedInUnity]
        public GAFMovieClip FrontWalk;

        [AssignedInUnity]
        public GAFMovieClip BackWalk;

        [UnityMessage]
        public void Awake()
        {
            Instance = this;
            rigidbody = GetComponent<Rigidbody2D>();

            walkSound = RuntimeManager.CreateInstance(SoundWalkEventName);
        }

        [UnityMessage]
        public void Update()
        {
            if (IsPaused)
            {
                desiredMovement = new Vector2();
                CheckAudio();
                return;
            }


            if (isRecoiling)
            {
                Recoil();
            }
            else
            {
                MoveFromPlayerMovement();
                CheckAttack();
                CheckUsePill();
            }

            CheckAudio();
        }

        private void CheckUsePill()
        {
            if (Input.GetButtonDown("Action2") == false)
                return;

            if (NumPills <= 0)
                return;

            UsePill();
        }

        private void CheckAttack()
        {
            if (canAttack == false || Input.GetButtonDown("Action1") == false)
                return;

            Delay.TemporarilySetBool(x => canAttack = x, AttackDelayTime, false);

            // TODO play animation

            SweepAttack();

            RuntimeManager.PlayOneShot(SoundBroomSweep);
        }

        private void SweepAttack()
        {
            const float degrees = 90;
            const int count = 10;
            const float increment = degrees / count;

            var facingDirection = transform.position.DirectionToDegrees(lastMovement);

            var angle = -(degrees / 2.0f);
            for(var i = 0; i < count; angle += increment, i++)
            {
                var raycastAngle = facingDirection + angle;
                var raycastVector = new Vector2(Mathf.Cos(raycastAngle), Mathf.Sin(raycastAngle));

                var raycastResults = Physics2D.RaycastAll(transform.position, raycastVector.normalized, AttackRange);

                raycastResults = raycastResults.Where(x => x.collider.gameObject != gameObject && x.collider.GetComponentInParent<Player>() == null).ToArray();
                
                if (raycastResults.Any())
                {
                    var result = raycastResults.First();
                    var canBeHit = result.collider.gameObject.GetInterfaceComponent<ICanBeHitWithBroom>();
                    if (canBeHit != null)
                    {
                        canBeHit.IsHitWithBroom();
                        break;
                    }
                }
            }
        }

        private bool IsPaused {get { return GameStateController.Instance.CurrentState != GameState.InGame; } }

        private void Recoil()
        {
            if (recoilFrameCounter >= RecoilFrames)
            {
                isRecoiling = false;
                return;
            }

            desiredMovement = recoilDirection * RecoilSpeed;
        }

        private void MoveFromPlayerMovement()
        {
            var rawMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (rawMovement.sqrMagnitude > 1)
                rawMovement.Normalize();

            desiredMovement = rawMovement * MoveSpeed;

            CheckSpriteDirection();
        }

        private void CheckSpriteDirection()
        {
            var movementDirection = new Vector3().DirectionToDegrees(lastMovement);

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

            if (SideWalk.gameObject.activeSelf != sideActive)
                SideWalk.gameObject.SetActive(sideActive);

            if (FrontWalk.gameObject.activeSelf != frontActive)
                FrontWalk.gameObject.SetActive(frontActive);

            if (BackWalk.gameObject.activeSelf != backActive)
                BackWalk.gameObject.SetActive(backActive);

            if (flipSide.HasValue)
            {
                var scale = SideWalk.transform.localScale;
                if (flipSide.Value && SideWalk.transform.localScale.x > 0)
                {
                    SideWalk.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
                }
                else if(!flipSide.Value && SideWalk.transform.localScale.x < 0)
                {
                    SideWalk.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
                }
            }

            if (desiredMovement.IsZero())
            {
                SideWalk.stop();
                FrontWalk.stop();
                BackWalk.stop();
            }
            else
            {
                if (SideWalk.isPlaying() == false)
                    SideWalk.play();
                if (FrontWalk.isPlaying() == false)
                    FrontWalk.play();
                if (BackWalk.isPlaying() == false)
                    BackWalk.play();
            }
        }

        private void CheckAudio()
        {
            if (desiredMovement.IsZero())
            {
                walkSound.setParameterValue("Loop", 0.0f);
            }
            else
            {
                PLAYBACK_STATE walkSoundState;
                walkSound.getPlaybackState(out walkSoundState);
                if (walkSoundState != PLAYBACK_STATE.PLAYING)
                {
                    walkSound.setParameterValue("Loop", 1.0f);
                    walkSound.start();
                }

                Rotation.transform.rotation = Quaternion.AngleAxis(new Vector3().DirectionToDegrees(desiredMovement), Vector3.forward);
            }
        }

        [UnityMessage]
        public void FixedUpdate()
        {
			if (desiredMovement.IsZero() || IsPaused)
				return;

            lastMovement = desiredMovement;
            
            var fixedMovement = desiredMovement * Time.fixedDeltaTime;
            var newPosition = transform.position + (Vector3)fixedMovement;

            rigidbody.MovePosition(newPosition);

            if (isRecoiling)
                recoilFrameCounter++;
        }

        [UnityMessage]
        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Transition"))
            {
                TransitionToAnotherMap(collider);
            }
        }

        private void TransitionToAnotherMap(Collider2D collider)
        {
            if (canTransition == false)
                return;

            RuntimeManager.PlayOneShot(SoundDoorOpenEventName, transform.position);
            EnterTransition(collider.gameObject.GetComponentInParent<MapTransition>());

            Delay.TemporarilySetBool(x => canTransition = x, 1, false);
        }

        private void EnterTransition(MapTransition transitionObject)
        {
            if (transitionObject == null)
                throw new InvalidOperationException("Missing transition object.");

            MapController.Instance.ChangeMap(transitionObject.ToMap);

            Delay.Frame(() =>
            {
                var destination = MapController.Instance.GetTransitionDestination(transitionObject.ToName);

                if (destination == null)
                    throw new InvalidOperationException("Couldn't find transition point " + transitionObject.ToName);

                transform.position = destination.transform.position;
            });
        }

        public void TakeDamage(int damageAmount)
        {
            Health -= damageAmount;

            RuntimeManager.PlayOneShot(Hit);

            if (HealthChanged != null)
                HealthChanged(Health);
        }

        public void TakeDamageAndPush(int damageAmount, Vector2 vectorToPush)
        {
            recoilDirection = vectorToPush.normalized;
            isRecoiling = true;
            recoilFrameCounter = 0;

            TakeDamage(damageAmount);
        }

        public void AddPill()
        {
            NumPills++;

            if (PillsChanged != null)
                PillsChanged(NumPills);
        }

        public void UsePill()
        {
            NumPills--;
            if (PillsChanged != null)
                PillsChanged(NumPills);

            Health = Mathf.Min(MaxHealth, Health + 3);
            
            if (HealthChanged != null)
                HealthChanged(Health);

            HealOverlay.Instance.Show();

            RuntimeManager.PlayOneShot(Heal);
        }
    }
}
