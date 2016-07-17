using UnityEngine;

namespace Assets._Scripts
{
    [UnityComponent]
    public class Trash : InteractableObject, ICanBeHitWithBroom
    {
        [UnityMessage]
        public void Awake()
        {
            InteractionName = "trash";
        }
        
        protected override void AfterStart()
        {
            MapController.Instance.RegisterThingToClean(gameObject);
        }

        public void IsHitWithBroom()
        {
            MapController.Instance.ThingCleaned(gameObject);
            Destroy(gameObject);
        }

        [UnityMessage]
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<BroomCollision>() != null)
                IsHitWithBroom();
        }
    }
}