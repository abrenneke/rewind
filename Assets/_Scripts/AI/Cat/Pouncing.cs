using FMODUnity;
using UnityEngine;

namespace Assets._Scripts.AI.Cat
{
    public class Pouncing : AIState
    {
        private CatAI CatAI {get { return (CatAI)AIBase; } }

        public override void Enter()
        {
            RuntimeManager.PlayOneShot(CatAI.CatAngry);
        }

        public override void Update()
        {
            var distanceToStart = CatAI.transform.position.DistanceTo(CatAI.StartPosition);

            if (distanceToStart >= CatAI.MaxPounceDistance)
            {
                StopMoving();
                CatAI.SetState<Returning>();
            }
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //TODO damage here?
                StopMoving();
                CatAI.SetState<Returning>();
            }
        }

        public override void FixedUpdate()
        {
            CatAI.MoveTo(Player.Instance.transform.position, CatAI.PounceSpeed);
        }
    }
}