using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class AfterInteractionDatabase : MonoBehaviour
    {
        public static AfterInteractionDatabase Instance { get; private set; }

        private IDictionary<string, AfterInteraction> afterInteractions;

        [UnityMessage]
        public void Awake()
        {
            Instance = this;

            afterInteractions = new Dictionary<string, AfterInteraction>();

            foreach (var afterInteraction in GetComponents<AfterInteraction>())
            {
                afterInteractions[afterInteraction.Name] = afterInteraction;
            }
        }

        public static void Trigger(string afterInteractionName)
        {
            AfterInteraction interaction;
            if (!Instance.afterInteractions.TryGetValue(afterInteractionName, out interaction))
                throw new InvalidOperationException("Couldn't find after interaction " + afterInteractionName);

            interaction.Trigger();
        }
    }
}