using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent, RequireComponent(typeof(Rigidbody2D))]
    public class NoVelocity : MonoBehaviour
    {
        private new Rigidbody2D rigidbody;

        [UnityMessage]
        public void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        [UnityMessage]
        public void Update()
        {
            if (rigidbody.velocity.sqrMagnitude > 0)
                rigidbody.velocity = Vector2.zero;
        }
    }
}