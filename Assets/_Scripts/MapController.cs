using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Tiled2Unity;
using UnityEngine;

namespace Assets._Scripts
{
    [Serializable, UsedImplicitly]
    public struct MapInfo
    {
        [AssignedInUnity]
        public GameObject MapPrefab;
    }

    [UnityComponent]
    public class MapController : MonoBehaviour
    {
        public static MapController Instance { get; private set; }

        public GameObject CurrentMap { get; private set; }

        [AssignedInUnity]
        public MapInfo[] AllMaps;

        [AssignedInUnity]
        public string StartingMapName;

        private IDictionary<string, TransitionDestination> destinations; 

        [UnityMessage]
        public void Awake()
        {
            destinations = new Dictionary<string, TransitionDestination>();
            Instance = this;
        }

        [UnityMessage]
        public void Start()
        {
            ChangeMap(StartingMapName);
        }

        public void RegisterTransitionPoint(TransitionDestination destination)
        {
            destinations[destination.Name] = destination;
        }

        public TransitionDestination GetTransitionDestination(string name)
        {
            TransitionDestination destination;
            destinations.TryGetValue(name, out destination);
            return destination;
        } 

        public void ChangeMap(string mapName)
        {
            if (CurrentMap != null)
                UnloadMap();

            var newMap = AllMaps.FirstOrDefault(x => x.MapPrefab.name == mapName);
            if (newMap.MapPrefab == null)
                throw new InvalidOperationException("Couldn't find map " + mapName);

            var mapInstance = (GameObject)Instantiate(newMap.MapPrefab, Vector3.zero, Quaternion.identity);

            CurrentMap = mapInstance;

            Player.Instance.GetComponent<SpriteDepthInMap>().AttachedMap = CurrentMap.GetComponent<TiledMap>();
        }

        private void UnloadMap()
        {
            Destroy(CurrentMap);

            destinations.Clear();

            CurrentMap = null;
        }
    }
}