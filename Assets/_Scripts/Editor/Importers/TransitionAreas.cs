using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Tiled2Unity;
using UnityEngine;

namespace Assets._Scripts.Editor.Importers
{
    [CustomTiledImporter, UsedImplicitly]
    public class TransitionAreas : ICustomTiledImporter
    {
        public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> customProperties)
        {
            if (customProperties.ContainsKey("from") || customProperties.ContainsKey("to"))
            {
                gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                gameObject.GetComponentInChildren<PolygonCollider2D>().isTrigger = true;

                if (customProperties.ContainsKey("from"))
                {
                    var from = customProperties["from"];

                    var transitionDestination = gameObject.AddComponent<TransitionDestination>();
                    transitionDestination.Name = from;

                    gameObject.name = "Transition destination " + from;
                }

                if (customProperties.ContainsKey("to"))
                {
                    var to = customProperties["to"].Split(':');

                    var toMap = to[0];
                    var toName = to[1];

                    var mapTransition = gameObject.AddComponent<MapTransition>();
                    mapTransition.ToMap = toMap;
                    mapTransition.ToName = toName;

                    gameObject.name = "Transition to " + customProperties["to"];
                    
                    foreach (var child in gameObject.GetComponentsInChildren<Transform>())
                        child.tag = "Transition";
                }
            }
            
        }

        public void CustomizePrefab(GameObject prefab)
        {
        }
    }
}