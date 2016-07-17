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

        private InteractionOverlay currentInteraction;
        private InteractableObject lastInteractionObject;

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

            lastInteractionObject = obj;

            GameStateController.Instance.SetState(GameState.InInteraction);

            var interactionInfo = InteractionDatabase.Instance.GetInteraction(obj.InteractionName);

            var interactionInstance = Instantiate(InteractionPrefab);
            interactionInstance.transform.SetParent(Canvas.transform, false);

            var overlay = interactionInstance.GetComponent<InteractionOverlay>();

            overlay.Text.text = interactionInfo.InteractionText;

            currentInteraction = overlay;

            currentInteraction.Show();
            Delay.TemporarilySetBool(x => canClose = x, 0.5f, false);

            //TODO image
        }

        [UnityMessage]
        public void Update()
        {
            if (currentInteraction == null || canClose == false)
                return;

            if (!Input.GetButtonDown("Submit"))
                return;

            Delay.TemporarilySetBool(x => CanInteract = x, 0.5f, false);
            Delay.After(currentInteraction.Hide(), WhenInteractionClosed);
        }

        private void WhenInteractionClosed()
        {
            currentInteraction = null;
            GameStateController.Instance.SetState(GameState.InGame);
            
            AfterInteractionDatabase.Trigger(lastInteractionObject);
        }
    }
}