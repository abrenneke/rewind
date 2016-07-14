using System.Collections;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class InteractionController : MonoBehaviour
    {
        public static InteractionController Instance { get; private set; }

        public bool CanInteract { get; private set; }

        [AssignedInUnity]
        public GameObject InteractionPrefab;

        [AssignedInUnity]
        public Canvas Canvas;

        private GameObject currentInteraction;

        private bool canClose;

        [UnityMessage]
        public void Awake()
        {
            Instance = this;
            CanInteract = true;
            canClose = true;
        }

        public void InteractWith(InteractableObject obj)
        {
            if (CanInteract == false || GameStateController.Instance.CurrentState != GameState.InGame)
                return;

            GameStateController.Instance.SetState(GameState.InInteraction);

            var interactionInfo = InteractionDatabase.Instance.GetInteraction(obj.InteractionName);

            var interactionInstance = Instantiate(InteractionPrefab);
            interactionInstance.transform.SetParent(Canvas.transform, false);

            var overlay = interactionInstance.GetComponent<InteractionOverlay>();

            overlay.Text.text = interactionInfo.InteractionText;

            currentInteraction = interactionInstance;

            StartCoroutine(CantCloseForShortTime());

            //TODO image
        }

        [UnityMessage]
        public void Update()
        {
            if (currentInteraction == null || canClose == false)
                return;

            if (Input.GetButtonDown("Submit"))
            {
                Destroy(currentInteraction);
                currentInteraction = null;

                StartCoroutine(CantInteractForShortTime());

                GameStateController.Instance.SetState(GameState.InGame);
            }
        }

        private IEnumerator CantInteractForShortTime()
        {
            CanInteract = false;
            yield return new WaitForSeconds(0.5f);
            CanInteract = true;
        }

        private IEnumerator CantCloseForShortTime()
        {
            canClose = false;
            yield return new WaitForSeconds(0.5f);
            canClose = true;
        }
    }
}