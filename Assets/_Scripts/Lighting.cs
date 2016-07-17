using DG.Tweening;
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

        private float lastLightingLevel;

        [UnityMessage]
        public void Awake()
        {
            lightingMaterial = new Material(Shader.Find("Hidden/Lighting"));

            lastLightingLevel = 1.0f;
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
                lightingMaterial.SetFloat("_Factor", lastLightingLevel);
            }
        }

        public void TurnOffLighting()
        {
            DOTween.To(() => lastLightingLevel, x =>
            {
                lastLightingLevel = x;
                lightingMaterial.SetFloat("_Factor", lastLightingLevel);
            }, 0.0f, 1.0f);
        }
    }
}