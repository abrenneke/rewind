using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Scripts.AI.Cat;
using JetBrains.Annotations;
using Tiled2Unity;
using UnityEngine;

namespace Assets._Scripts
{
    [Serializable, UsedImplicitly]
    public struct MapInfo
    {
        [AssignedInUnity]
        public string Name;

        [AssignedInUnity]
        public GameObject DirtyMapPrefab;

        [AssignedInUnity]
        public GameObject CleanMapPrefab;
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

        private MapInfo lastMap;

        private IDictionary<string, TransitionDestination> destinations;

        private IList<string> cleanedMaps;

        private IList<GameObject> thingsToCleanInCurrentRoom;

        [UnityMessage]
        public void Awake()
        {
            destinations = new Dictionary<string, TransitionDestination>();
            cleanedMaps = new List<string>();
            thingsToCleanInCurrentRoom = new List<GameObject>();
            Instance = this;
        }

        [UnityMessage]
        public void Start()
        {
            ChangeMap(StartingMapName);

            var startPosition = CurrentMap.GetComponentInChildren<StartPosition>();
            if (startPosition == null)
                throw new InvalidOperationException("Missing starting position.");

            Player.Instance.transform.position = startPosition.gameObject.transform.position;
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

            var mapInfo = AllMaps.FirstOrDefault(x => x.Name == mapName);

            if (mapInfo.Name == null)
                throw new InvalidOperationException("Couldn't find map " + mapName);

            lastMap = mapInfo;

            var targetMap = mapInfo.DirtyMapPrefab;
            if (cleanedMaps.Contains(mapInfo.Name) && mapInfo.CleanMapPrefab != null)
            {
                targetMap = mapInfo.CleanMapPrefab;
            }
            
            var mapInstance = (GameObject)Instantiate(targetMap, Vector3.zero, Quaternion.identity);

            CurrentMap = mapInstance;

            Player.Instance.GetComponentInChildren<SpriteDepthInMap>().AttachedMap = CurrentMap.GetComponent<TiledMap>();

            thingsToCleanInCurrentRoom.Clear();
        }

        private void UnloadMap()
        {
            Destroy(CurrentMap);

            destinations.Clear();

            CurrentMap = null;
        }

        public void SetCleaned()
        {
            cleanedMaps.Add(lastMap.Name);
            ChangeMap(lastMap.Name);

            //TODO spawn location
        }

        public void RegisterThingToClean(GameObject thing)
        {
            thingsToCleanInCurrentRoom.Add(thing);
        }

        public void ThingCleaned(GameObject thing)
        {
            thingsToCleanInCurrentRoom.Remove(thing);

            if (thingsToCleanInCurrentRoom.Count == 0)
            {
                SetCleaned();
            }
        }
    }
}