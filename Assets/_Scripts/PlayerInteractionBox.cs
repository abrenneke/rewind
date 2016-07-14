using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class PlayerInteractionBox : MonoBehaviour
    {
        [AssignedInUnity]
        public Player Player;

        private new BoxCollider2D collider;

        [UnityMessage]
        public void Start()
        {
            collider = GetComponent<BoxCollider2D>();

            Physics2D.IgnoreCollision(collider, Player.GetComponent<Collider2D>());
        }

        [UnityMessage]
        public void OnTriggerEnter2D(Collider2D other)
        {
            var interactable = other.gameObject.GetInterfaceComponent<IInteractable>();
            if (interactable == null)
                return;


        }
    }
}