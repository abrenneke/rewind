using System.Collections.Generic;
using Assets._Scripts.Spawners;
using JetBrains.Annotations;
using Tiled2Unity;
using UnityEngine;

namespace Assets._Scripts.Editor.Importers
{
    [CustomTiledImporter, UsedImplicitly]
    public class Mice : ICustomTiledImporter
    {
        public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> customProperties)
        {
        }

        public void CustomizePrefab(GameObject prefab)
        {
            var miceLayer = prefab.transform.FindChild("Mice");

            if (miceLayer == null)
                return;

            foreach (Transform mouseObject in miceLayer)
            {
                mouseObject.gameObject.AddComponent<MouseSpawner>();
                mouseObject.name = "Mouse Spawner";
            }
        }
    }
}