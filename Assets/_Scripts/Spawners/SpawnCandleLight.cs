using Tiled2Unity;
using UnityEngine;

namespace Assets._Scripts.Spawners
{
    [UnityComponent]
    public class SpawnCandleLight : MonoBehaviour
    {
        [UnityMessage]
        public void Start()
        {
            var instance = Instantiate(InGameObjectPrefabReference.Instance.CandleLightPrefab);

            instance.transform.SetParent(transform.parent, false);
        }
    }
}