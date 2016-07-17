using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEngine;
using YamlDotNet.RepresentationModel;

namespace Assets._Scripts
{
    [UsedImplicitly]
    public class InteractionInfo
    {
        public string InteractionText { get; set; }

        public string ImageName { get; set; }
    }

    [UnityComponent]
    public class InteractionDatabase : MonoBehaviour
    {
        public static InteractionDatabase Instance { get; private set; }

        private IDictionary<string, IDictionary<int, InteractionInfo>> interactions;

        private IDictionary<string, int> interactionCounts;

        [UnityMessage]
        public void Awake()
        {
            Instance = this;
            interactions = new Dictionary<string, IDictionary<int, InteractionInfo>>();
            interactionCounts = new Dictionary<string, int>();
        }

        [UnityMessage]
        public void Start()
        {
            var resources = Resources.LoadAll<TextAsset>("Interactions");

            foreach (var resource in resources)
            {
                try
                {
                    var reader = new StringReader(resource.text);
                    var yaml = new YamlStream();
                    yaml.Load(reader);

                    var root = (YamlMappingNode)yaml.Documents[0].RootNode;

                    foreach (var child in root.Children)
                    {
                        var keyName = ((YamlScalarNode)child.Key).Value;
                        var index = 1;

                        var match = Regex.Match(keyName, @"(.+)_(\d)$");

                        if (match.Success)
                        {
                            keyName = match.Groups[1].Value;
                            index = Convert.ToInt32(match.Groups[2].Value);
                        }

                        var childInfo = (YamlMappingNode)child.Value;
                        var interaction = new InteractionInfo
                        {
                            InteractionText = childInfo.Children.TryGetScalarValue("interaction-text"),
                            ImageName = childInfo.Children.TryGetScalarValue("image")
                        };

                        if (interactions.ContainsKey(keyName) == false)
                            interactions[keyName] = new Dictionary<int, InteractionInfo>();

                        interactions[keyName][index] = interaction;
                        ResetInteractionCount(keyName);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
        }

        public void ResetInteractionCount(string interactionName)
        {
            interactionCounts[interactionName] = 1;
        }

        public void ResetAllInteractionCounts()
        {
            foreach (var key in interactionCounts.Keys.ToList())
            {
                interactionCounts[key] = 1;
            }
        }

        public InteractionInfo GetInteraction(string interactionName)
        {
            IDictionary<int, InteractionInfo> allInteractions;
            if (!interactions.TryGetValue(interactionName, out allInteractions))
            {
                allInteractions = new Dictionary<int, InteractionInfo>
                {
                    { 1, new InteractionInfo { InteractionText = "This object has no text." } }
                };
            }

            if (allInteractions.ContainsKey(interactionCounts[interactionName]) == false)
            {
                // Reached end
                return allInteractions[interactionCounts[interactionName] - 1];
            }

            var interaction = allInteractions[interactionCounts[interactionName]];

            interactionCounts[interactionName]++;

            return interaction;
        }
    }
}