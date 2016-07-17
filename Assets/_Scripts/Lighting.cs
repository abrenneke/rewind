using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class Lighting : MonoBehaviour
    {
        [AssignedInUnity]
        public Camera LightingCamera;
        
        private Material lightingMaterial;

        private Texture lastLightingTex;

        [UnityMessage]
        public void Awake()
        {
            lightingMaterial = new Material(Shader.Find("Hidden/Lighting"));
        }

        [UnityMessage]
        public void OnRenderImage(RenderTexture source, RenderTexture target)
        {
            Graphics.Blit(source, target, lightingMaterial);
        }

        [UnityMessage]
        public void Update()
        {
            if (lastLightingTex != LightingCamera.targetTexture && LightingCamera.targetTexture != null)
            {
                lastLightingTex = LightingCamera.targetTexture;
                lightingMaterial.SetTexture("_LightingTex", lastLightingTex);
            }
        }
    }
}