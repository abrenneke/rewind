using System.Collections.Generic;
using JetBrains.Annotations;
using Tiled2Unity;
using UnityEngine;

namespace Assets._Scripts.Editor.Importers
{
    [CustomTiledImporter, UsedImplicitly]
    public class StartPosition : ICustomTiledImporter
    {
        public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> customProperties)
        {
            if (customProperties.ContainsKey("start"))
            {
                gameObject.AddComponent<_Scripts.StartPosition>();

                gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;

                var collider = gameObject.GetComponentInChildren<Collider2D>();
                if (collider != null)
                    collider.enabled = false;
            }
        }

        public void CustomizePrefab(GameObject prefab)
        {

        }
    }
}