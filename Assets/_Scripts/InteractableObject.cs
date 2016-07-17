using Tiled2Unity;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class InteractableObject : MonoBehaviour
    {
        public string InteractionName;

        private GameObject overlayChild;

        public string AfterInteraction;

        public void ShowCanInteract()
        {
            overlayChild.SetActive(true);
            InteractionsDisplay.Instance.SetHighlightInteract(true);
        }

        public void HideCanInteract()
        {
            overlayChild.SetActive(false);
            InteractionsDisplay.Instance.SetHighlightInteract(false);
        }

        [UnityMessage]
        public void OnDestroy()
        {
            HideCanInteract();
        }

        [UnityMessage]
        public void Start()
        {
            overlayChild = new GameObject("Overlay");

            overlayChild.transform.localPosition = new Vector3(0, 0, -1f);

            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            var meshFilter = GetComponentInChildren<MeshFilter>();
            
            overlayChild.transform.SetParent(meshRenderer.gameObject.transform, false);

            var newMeshFilder = overlayChild.AddComponent<MeshFilter>();
            newMeshFilder.mesh = meshFilter.mesh;

            var newMeshRenderer = overlayChild.AddComponent<MeshRenderer>();
            newMeshRenderer.sortingLayerName = meshRenderer.sortingLayerName;
            newMeshRenderer.sortingOrder = meshRenderer.sortingOrder;

            newMeshRenderer.material = DynamicMaterialDatabase.Instance.GetMaterialForTexture(meshRenderer.material.mainTexture);

            HideCanInteract();
        }
    }
}