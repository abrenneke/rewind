using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent, RequireComponent(typeof(Camera))]
    public class LightingCamera : MonoBehaviour
    {
        [UnityMessage]
        public void Start()
        {
            var thisCamera = GetComponent<Camera>();

            if (thisCamera.targetTexture != null)
                thisCamera.targetTexture.Release();

            thisCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.Default);
        }
    }
}