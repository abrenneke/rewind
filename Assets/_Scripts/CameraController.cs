using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent, RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [UnityMessage]
        public void Start()
        {
            transform.position = new Vector3(0, 0, -10);
            transform.SetParent(Player.Instance.transform, false);
        } 
    }
}