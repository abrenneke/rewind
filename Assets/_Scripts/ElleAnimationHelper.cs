using GAF.Core;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class ElleAnimationHelper : MonoBehaviour
    {
        public void StopAnimation(string childName)
        {
            transform.FindChild(childName).GetComponent<GAFMovieClip>().stop();
        }

        public void StartAnimation(string childName)
        {
            var movieClip = transform.FindChild(childName).GetComponent<GAFMovieClip>();
            if (movieClip.isPlaying() == false)
                movieClip.play();
        }
    }
}