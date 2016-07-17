using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts
{
    [UnityComponent]
    public class HealOverlay : MonoBehaviour
    {
        public static HealOverlay Instance { get; private set; }

        [AssignedInUnity]
        public float Duration;

        [AssignedInUnity]
        public float FullAlpha = 0.23f;
        
        private Image image;

        [UnityMessage]
        public void Awake()
        {
            Instance = this;
        }

        [UnityMessage]
        public void Start()
        {
            image = GetComponent<Image>();
            image.color = image.color.WithAlpha(0);
        }

        public void Show()
        {
            image.color = image.color.WithAlpha(FullAlpha);
            image.DOFade(0, Duration);
        }
    }
}