using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts
{
    [UnityComponent]
    public class InteractionsDisplay : MonoBehaviour
    {
        public static InteractionsDisplay Instance { get; private set; }

        [AssignedInUnity]
        public GameObject PillsArea;

        [AssignedInUnity]
        public Text PillsCounter;

        [AssignedInUnity]
        public GameObject BroomArea;

        [AssignedInUnity]
        public GameObject InteractArea;

        [UnityMessage]
        public void Awake()
        {
            Instance = this;

            SetBroomEnabled(false);
            SetPillsEnabled(false);
        }

        [UnityMessage]
        public void Start()
        {
            Player.Instance.PillsChanged += PillsChanged;

            PillsChanged(0);
        }

        private void PillsChanged(int numPills)
        {
            PillsCounter.text = numPills.ToString();

            SetPillsEnabled(numPills > 0);
        }

        public void SetPillsEnabled(bool enabled)
        {
            PillsArea.SetActive(enabled);
        }

        public void SetBroomEnabled(bool enabled)
        {
            BroomArea.SetActive(enabled);
        }

        public void SetHighlightInteract(bool highlight)
        {
            // Dunno
            if (this == null || InteractArea == null)
                return;

            var image = InteractArea.GetComponent<Image>();
            if (highlight)
            {
                image.color = Color.white.WithAlpha(image.color.a);
            }
            else
            {
                image.color = Color.black.WithAlpha(image.color.a);
            }
        }
    }
}