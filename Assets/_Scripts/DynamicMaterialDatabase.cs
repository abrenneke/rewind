using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class DynamicMaterialDatabase : MonoBehaviour
    {
        public static DynamicMaterialDatabase Instance { get; private set; }

        [AssignedInUnity, Range(0, 1)]
        public float TintAlpha;

        private IDictionary<string, Material> createdMaterials;

        [UnityMessage]
        public void Awake()
        {
            Instance = this;
            createdMaterials = new Dictionary<string, Material>();
        }

        public Material GetMaterialForTexture(Texture texture)
        {
            Material material;
            if (createdMaterials.TryGetValue(texture.name, out material))
                return material;

            material = CreateMaterial(texture);
            createdMaterials[texture.name] = material;

            return material;
        }

        private Material CreateMaterial(Texture texture)
        {
            var material = new Material(Shader.Find("Custom/White Overlay"));

            material.SetFloat("Cut off", 0.1f);
            material.SetFloat("Pixel snap", 1.0f);

            material.mainTexture = texture;
            material.SetTexture("Tiled Texture", texture);

            var alphaColor = new Color(1, 1, 1, TintAlpha);
            material.SetColor("Tint", alphaColor);
            material.color = alphaColor;

            return material;
        }
    }
}