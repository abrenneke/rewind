using System;
using System.Collections.Generic;
using Assets._Scripts.AfterInteractions;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class AfterInteractionDatabase : MonoBehaviour
    {
        public static AfterInteractionDatabase Instance { get; private set; }

        private IDictionary<string, AfterInteraction> afterInteractions;
        private IList<string> triggeredAfterInteractions;

        [UnityMessage]
        public void Awake()
        {
            Instance = this;

            afterInteractions = new Dictionary<string, AfterInteraction>();
            triggeredAfterInteractions = new List<string>();

            foreach (var afterInteraction in GetComponentsInChildren<AfterInteraction>())
            {
                afterInteractions[afterInteraction.Name] = afterInteraction;
            }
        }

        public static AfterInteraction Get(string afterInteractionName)
        {
            AfterInteraction interaction;
            
            if (!Instance.afterInteractions.TryGetValue(afterInteractionName, out interaction))
                throw new InvalidOperationException("Couldn't find after interaction " + afterInteractionName);

            return interaction;
        }

        public static void Trigger(InteractableObject interactableObject)
        {
            if (interactableObject.AfterInteraction == null)
                return;

            if (Instance.triggeredAfterInteractions.Contains(interactableObject.AfterInteraction))
                return;

            var afterInteraction = Get(interactableObject.AfterInteraction);

            afterInteraction.Trigger();

            if(afterInteraction.AllowMultiple == false)
                Instance.triggeredAfterInteractions.Add(afterInteraction.Name);

            if (afterInteraction.DisablesInteraction)
                Destroy(interactableObject);
        }
    }
}