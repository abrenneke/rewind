using UnityEngine;

namespace Assets._Scripts.AfterInteractions
{
    public class GiveCandle : AfterInteraction
    {
        public override void Trigger()
        {
            Camera.main.GetComponent<Lighting>().TurnOffLighting();
        }
    }
}