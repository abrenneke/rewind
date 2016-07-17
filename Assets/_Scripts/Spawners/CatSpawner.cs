using Tiled2Unity;
using UnityEngine;

namespace Assets._Scripts.Spawners
{
    [UnityComponent]
    public class CatSpawner : MonoBehaviour
    {
        [UnityMessage]
        public void Start()
        {
            var instance = (GameObject)Instantiate(InGameObjectPrefabReference.Instance.CatPrefab, transform.position, Quaternion.identity);

            instance.transform.SetParent(transform.parent);

            var map = GetComponentInParent<TiledMap>();
            instance.GetComponent<SpriteDepthInMap>().AttachedMap = map;

            Destroy(gameObject);
        } 
    }
}