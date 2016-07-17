using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public abstract class AfterInteraction : MonoBehaviour
    {
        [AssignedInUnity]
        public string Name;

        public abstract void Trigger();
    }
}