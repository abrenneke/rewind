using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class QuitButton : MonoBehaviour
    {
        [CalledFromUnity]
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}