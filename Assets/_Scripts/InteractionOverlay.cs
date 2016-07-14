using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts
{
    [UnityComponent]
    public class InteractionOverlay : MonoBehaviour
    {
        [AssignedInUnity]
        public Text Text;

        [AssignedInUnity]
        public Image Image;

        [AssignedInUnity]
        public Image Backdrop;

        [AssignedInUnity]
        public RectTransform Container;

        [AssignedInUnity, Range(0, 3)]
        public float TimeToShowBackdrop = 1;

        private Color destinationColor;

        [UnityMessage]
        public void Awake()
        {
            destinationColor = Backdrop.color;
        }

        public void Show()
        {
            Backdrop.color = destinationColor.WithAlpha(0);;

            Backdrop.DOColor(destinationColor, TimeToShowBackdrop);
        }

        public IEnumerator Hide()
        {
            Container.gameObject.SetActive(false);

            Backdrop.DOColor(destinationColor.WithAlpha(0), TimeToShowBackdrop);
            yield return new WaitForSeconds(TimeToShowBackdrop);

            Destroy(gameObject);
        }
    }
}