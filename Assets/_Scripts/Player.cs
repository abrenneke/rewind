using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent, RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [AssignedInUnity, Range(0, 1)]
        public float MoveSpeed = 5;

        private Vector2 desiredMovement;
        private new Rigidbody2D rigidbody;

        [UnityMessage]
        public void Awake()
        {
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
    }
}
