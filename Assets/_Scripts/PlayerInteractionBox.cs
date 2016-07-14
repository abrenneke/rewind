using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class PlayerInteractionBox : MonoBehaviour
    {
        [AssignedInUnity]
        public Player Player;

        private new BoxCollider2D collider;

        private InteractableObject currentInteractableObject;

        [UnityMessage]
        public void Start()
        {
            collider = GetComponent<BoxCollider2D>();

            Physics2D.IgnoreCollision(collider, Player.GetComponent<Collider2D>());
        }

        [UnityMessage]
        public void OnTriggerEnter2D(Collider2D other)
        {
            var interactable = other.gameObject.GetComponentInParent<InteractableObject>();
            if (interactable == null || currentInteractableObject != null)
                return;

            currentInteractableObject = interactable;

            //TODO highlight object
        }

        [UnityMessage]
        public void OnTriggerExit2D(Collider2D other)
        {
            var interactable = other.gameObject.GetComponentInParent<InteractableObject>();
            if (interactable == null)
                return;

            if (interactable == currentInteractableObject)
            {
                currentInteractableObject = null;
            }
        }

        [UnityMessage]
        public void Update()
        {
            if (currentInteractableObject == null)
                return;

            if (Input.GetButtonDown("Submit"))
            {
                InteractionController.Instance.InteractWith(currentInteractableObject);
            }
        }
    }
}