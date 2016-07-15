using System;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

namespace Assets._Scripts
{
    [UnityComponent, RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        public event Action<int> HealthChanged;

		[EventRef]
		public string SoundWalkEventName = "event:/footsteps_carpet";
        private EventInstance walkSound;

        [EventRef]
		public string SoundDoorOpenEventName = "event:/Door_Open";
        
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

        private Vector2 desiredMovement;
        private new Rigidbody2D rigidbody;

        private bool isRecoiling;
        private Vector2 recoilDirection;
        private int recoilFrameCounter;

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
            if (GameStateController.Instance.CurrentState != GameState.InGame)
            {
                desiredMovement = new Vector2();
            }
            else if (isRecoiling)
            {
                Recoil();
            }
            else
            {
                MoveFromPlayerMovement();
            }

            CheckAudio();
        }

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
        }

        private void CheckAudio()
        {
            if (desiredMovement.IsZero())
            {
                walkSound.stop(STOP_MODE.ALLOWFADEOUT);
            }
            else
            {
                PLAYBACK_STATE walkSoundState;
                walkSound.getPlaybackState(out walkSoundState);
                if (walkSoundState != PLAYBACK_STATE.PLAYING)
                    walkSound.start();

                Rotation.transform.rotation = Quaternion.AngleAxis(new Vector3().DirectionToDegrees(desiredMovement), Vector3.forward);
            }
        }

        [UnityMessage]
        public void FixedUpdate()
        {
			if (desiredMovement.IsZero())
				return;
            
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
				RuntimeManager.PlayOneShot(SoundDoorOpenEventName, transform.position);
				EnterTransition(collider.gameObject.GetComponentInParent<MapTransition>());
            }
        }

        private void EnterTransition(MapTransition transitionObject)
        {
            if (transitionObject == null)
                throw new InvalidOperationException("Missing transition object.");

            MapController.Instance.ChangeMap(transitionObject.ToMap);

            var destination = MapController.Instance.GetTransitionDestination(transitionObject.ToName);

            if (destination == null)
                throw new InvalidOperationException("Couldn't find transition point " + transitionObject.ToName);

            transform.position = destination.transform.position;
        }

        public void TakeDamage(int damageAmount)
        {
            Health -= damageAmount;

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
    }
}
