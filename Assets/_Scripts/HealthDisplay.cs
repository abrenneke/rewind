using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class HealthDisplay : MonoBehaviour
    {
        [AssignedInUnity]
        public GameObject HeartPrefab;

        [AssignedInUnity]
        public GameObject HalfHeartPrefab;

        [AssignedInUnity]
        public Transform Container;

        private int lastHealthAmount;

        private IList<GameObject> instances;

        [UnityMessage]
        public void Awake()
        {
            instances = new List<GameObject>();
        }

        [UnityMessage]
        public void Update()
        {
            if(lastHealthAmount != Player.Instance.Health)
                RecalculateHeartDisplay();

            lastHealthAmount = Player.Instance.Health;
        }

        private void RecalculateHeartDisplay()
        {
            foreach (var instance in instances)
            {
                Destroy(instance);
            }

            instances.Clear();

            var remainingHealth = Player.Instance.Health;

            while (remainingHealth >= 2)
            {
                instances.Add(Instantiate(HeartPrefab));
                remainingHealth -= 2;
            }

            while (remainingHealth >= 1)
            {
                instances.Add(Instantiate(HalfHeartPrefab));
                remainingHealth--;
            }

            foreach (var instance in instances)
            {
                instance.transform.SetParent(Container, false);
            }
        }
    }
}