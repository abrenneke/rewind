using System;
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
            if (overlayChild == null)
                throw new InvalidOperationException("No overlay");

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

            if (GetComponentInChildren<MeshRenderer>() != null)
            {
                SetUpMeshRendererOverlay();
            }
            else
            {
                SetUpSpriteRendererOverlay();
            }

            HideCanInteract();
        }

        private void SetUpSpriteRendererOverlay()
        {
            var spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            overlayChild.transform.SetParent(spriteRenderer.gameObject.transform, false);

            var newSpriteRenderer = overlayChild.AddComponent<SpriteRenderer>();

            newSpriteRenderer.sprite = spriteRenderer.sprite;
            newSpriteRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
            newSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder;

            newSpriteRenderer.material = DynamicMaterialDatabase.Instance.GetMaterialForTexture(spriteRenderer.sprite.texture);
        }

        private void SetUpMeshRendererOverlay()
        {
            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            var meshFilter = GetComponentInChildren<MeshFilter>();

            overlayChild.transform.SetParent(meshRenderer.gameObject.transform, false);

            var newMeshFilder = overlayChild.AddComponent<MeshFilter>();
            newMeshFilder.mesh = meshFilter.mesh;

            var newMeshRenderer = overlayChild.AddComponent<MeshRenderer>();
            newMeshRenderer.sortingLayerName = meshRenderer.sortingLayerName;
            newMeshRenderer.sortingOrder = meshRenderer.sortingOrder;

            newMeshRenderer.material = DynamicMaterialDatabase.Instance.GetMaterialForTexture(meshRenderer.material.mainTexture);
        }
    }
}