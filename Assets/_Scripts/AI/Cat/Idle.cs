namespace Assets._Scripts.AI.Cat
{
    public class Idle : AIState
    {
        private CatAI CatAI { get { return (CatAI)AIBase; } }

        public override void Enter()
        {
            CatAI.StartPosition = CatAI.transform.position;
        }

        public override void Update()
        {
            var distanceToPlayer = CatAI.transform.position.DistanceTo(Player.Instance.transform.position);

            if (distanceToPlayer < CatAI.DetectionRadius)
            {
                CatAI.SetState<Pouncing>();
            }

            //TODO idle animation?
        }
    }
}