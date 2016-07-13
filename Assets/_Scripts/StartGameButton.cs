using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets._Scripts
{
    [UnityComponent]
    public class StartGameButton : MonoBehaviour
    {
        [CalledFromUnity]
        public void Play()
        {
            SceneManager.LoadScene("Game");
        } 
    }
}