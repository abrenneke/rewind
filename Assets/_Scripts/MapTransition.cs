using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class MapTransition : MonoBehaviour
    {
        [AssignedInUnity]
        public string ToMap;

        [AssignedInUnity]
        public string ToName;

        [AssignedInUnity]
        public bool IsDoor;
    }
}