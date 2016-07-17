using System.Collections.Generic;
using Assets._Scripts.Spawners;
using JetBrains.Annotations;
using Tiled2Unity;
using UnityEngine;

namespace Assets._Scripts.Editor.Importers
{
    [CustomTiledImporter, UsedImplicitly]
    public class Cats : ICustomTiledImporter
    {
        public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> customProperties)
        {
            
        }

        public void CustomizePrefab(GameObject prefab)
        {
            var catLayer = prefab.transform.FindChild("Cats");

            if (catLayer == null)
                return;

            foreach (Transform catObject in catLayer)
            {
                var spawner = catObject.gameObject.AddComponent<CatSpawner>();
                spawner.Map = prefab.GetComponent<TiledMap>();

                catObject.name = "Cat Spawner";
            }
        }
    }
}