using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts
{
    [UnityComponent]
    public class DamageOverlay : MonoBehaviour
    {
        [AssignedInUnity]
        public float Duration;

        [AssignedInUnity]
        public float FullAlpha = 0.23f;

        private int previousHealth;
        private Image image;

        [UnityMessage]
        public void Start()
        {
            image = GetComponent<Image>();
            image.color = image.color.WithAlpha(0);

            previousHealth = Player.Instance.Health;

            Player.Instance.HealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(int newHealth)
        {
            if (newHealth < previousHealth)
            {
                image.color = image.color.WithAlpha(FullAlpha);
                image.DOFade(0, Duration);
            }

            previousHealth = newHealth;
        }
    }
}