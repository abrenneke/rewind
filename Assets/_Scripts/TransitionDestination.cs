using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class TransitionDestination : MonoBehaviour
    {
        [AssignedInUnity]
        public string Name;

        [UnityMessage]
        public void Awake()
        {
            MapController.Instance.RegisterTransitionPoint(this);
        }
    }
}