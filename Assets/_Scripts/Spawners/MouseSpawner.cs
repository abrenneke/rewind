using Tiled2Unity;
using UnityEngine;

namespace Assets._Scripts.Spawners
{
    [UnityComponent]
    public class MouseSpawner : MonoBehaviour
    {
        public TiledMap Map { get; set; }

        [UnityMessage]
        public void Start()
        {
            var instance = (GameObject)Instantiate(InGameObjectPrefabReference.Instance.MousePrefab, transform.position, Quaternion.identity);

            instance.transform.SetParent(transform.parent);

            Destroy(gameObject);
        }
    }
}