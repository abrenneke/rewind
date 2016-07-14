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
            }
        }

        public void CustomizePrefab(GameObject prefab)
        {
        }
    }
}