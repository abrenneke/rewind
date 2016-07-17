﻿using System.Collections.Generic;
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

            foreach (Transform mouseObject in miceLayer)
            {
                var spawner = mouseObject.gameObject.AddComponent<MouseSpawner>();

                spawner.Map = prefab.GetComponent<TiledMap>();
                spawner.name = "Mouse Spawner";
            }
        }
    }
}