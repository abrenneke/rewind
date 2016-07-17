using System.Collections.Generic;
using Tiled2Unity;
using UnityEngine;

namespace Assets._Scripts.Editor.Importers
{
    [CustomTiledImporter]
    public class InteractableObject : ICustomTiledImporter
    {
        public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> customProperties)
        {
            if (customProperties.ContainsKey("interaction"))
            {
                var interaction = gameObject.AddComponent<_Scripts.InteractableObject>();
                interaction.InteractionName = customProperties["interaction"];

                if (customProperties.ContainsKey("after-interaction"))
                {
                    interaction.AfterInteraction = customProperties["after-interaction"];
                }
            }
        }

        public void CustomizePrefab(GameObject prefab)
        {
        }
    }
}