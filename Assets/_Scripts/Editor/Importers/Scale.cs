using System;
using System.Collections.Generic;
using Tiled2Unity;
using UnityEngine;

namespace Assets._Scripts.Editor.Importers
{
    [CustomTiledImporter]
    public class Scale : ICustomTiledImporter
    {
        public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> customProperties)
        {
            if (customProperties.ContainsKey("scale"))
            {
                var scale = Convert.ToSingle(customProperties["scale"]);

                gameObject.transform.GetChild(0).gameObject.transform.localScale = new Vector3(scale, scale, scale);
            }
        }

        public void CustomizePrefab(GameObject prefab)
        {
        }
    }
}