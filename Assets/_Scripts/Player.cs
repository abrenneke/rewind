using System;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent, RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }

        [AssignedInUnity, Range(0, 1)]
        public float MoveSpeed = 5;

        private Vector2 desiredMovement;
        private new Rigidbody2D rigidbody;

        [UnityMessage]
        public void Awake()
        {
            Instance = this;
            rigidbody = GetComponent<Rigidbody2D>();
        }

        [UnityMessage]
        public void Update()
        {
            var rawMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (rawMovement.sqrMagnitude > 1)
                rawMovement.Normalize();

            desiredMovement = rawMovement * MoveSpeed;

            if (rigidbody.velocity.sqrMagnitude > 0)
                rigidbody.velocity = new Vector2();
        }

        [UnityMessage]
        public void FixedUpdate()
        {
            if (desiredMovement.IsZero())
                return;

            var fixedMovement = desiredMovement * Time.fixedDeltaTime;
            var newPosition = transform.position + (Vector3)fixedMovement;

            rigidbody.MovePosition(newPosition);
        }

        [UnityMessage]
        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Transition"))
            {
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
    }
}
