using System.Collections.Generic;
using Assets._Scripts.Spawners;
using JetBrains.Annotations;
using Tiled2Unity;
using UnityEngine;

namespace Assets._Scripts.Editor.Importers
{
    [CustomTiledImporter, UsedImplicitly]
    public class Trash : ICustomTiledImporter
    {
        public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> customProperties)
        {
            
        }

        public void CustomizePrefab(GameObject prefab)
        {
            var trashLayer = prefab.transform.FindChild("Trash");

            if (trashLayer == null)
                return;

            foreach (Transform trashObject in trashLayer)
            {
                trashObject.gameObject.AddComponent<TrashSpawner>();
                trashObject.name = "Trash Spawner";
            }
        }
    }
}