using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts
{
    [UnityComponent]
    public class InteractionOverlay : MonoBehaviour
    {
        [AssignedInUnity]
        public Text Text;

        [AssignedInUnity]
        public Image Image;
    }
}