using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class InGameObjectPrefabReference : MonoBehaviour
    {
        public static InGameObjectPrefabReference Instance { get; private set; }

        [AssignedInUnity]
        public GameObject TrashPrefab;

        [AssignedInUnity]
        public GameObject CatPrefab;

        [AssignedInUnity]
        public GameObject MousePrefab;

        [UnityMessage]
        public void Awake()
        {
            Instance = this;
        }
    }
}