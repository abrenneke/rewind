using UnityEngine;

namespace Assets._Scripts.AfterInteractions
{
    [UnityComponent]
    public abstract class AfterInteraction : MonoBehaviour
    {
        [AssignedInUnity]
        public string Name;

        [AssignedInUnity]
        public bool DisablesInteraction;

        [AssignedInUnity]
        public bool ReturnsOnRespawn;

        [AssignedInUnity]
        public bool AllowMultiple;

        public abstract void Trigger();
    }
}