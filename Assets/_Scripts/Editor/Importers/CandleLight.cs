using System.Collections.Generic;
using Assets._Scripts.Spawners;
using JetBrains.Annotations;
using Tiled2Unity;
using UnityEngine;

namespace Assets._Scripts.Editor.Importers
{
    [CustomTiledImporter, UsedImplicitly]
    public class CandleLight : ICustomTiledImporter
    {
        public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> customProperties)
        {
            if (customProperties.ContainsKey("after-interaction") && customProperties["after-interaction"] == "give-candle")
            {
                gameObject.AddComponent<SpawnCandleLight>();
            }
        }

        public void CustomizePrefab(GameObject prefab)
        {
        }
    }
}