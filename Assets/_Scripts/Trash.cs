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

        public void IsHitWithBroom()
        {
            Destroy(gameObject);
        }
    }
}