using UnityEngine;

namespace Assets._Scripts.AI.Cat
{
    public class Returning : AIState
    {
        private CatAI CatAI { get { return (CatAI)AIBase; } }
        
        public override void Update()
        {
            if (CatAI.HasReached(CatAI.StartPosition))
            {
                CatAI.transform.position = new Vector3(CatAI.StartPosition.x, CatAI.StartPosition.y, CatAI.transform.position.z);
                StopMoving();
                CatAI.SetState<Idle>();
            }
        }

        public override void FixedUpdate()
        {
            CatAI.MoveTo(CatAI.StartPosition, CatAI.ReturnSpeed);
        }
    }
}