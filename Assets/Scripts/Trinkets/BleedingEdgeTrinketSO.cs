using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Doubles become riskier but more rewarding.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/BleedingEdge", fileName = "BleedingEdge")]
    public sealed class BleedingEdgeTrinketSO : TrinketSO
    {
        private bool doubledThisHand;

        /// <inheritdoc />
        public override void OnHandStart(BlackjackEngine engine)
        {
            doubledThisHand = false;
        }

        /// <inheritdoc />
        public override void OnDouble(BlackjackEngine engine)
        {
            doubledThisHand = true;
        }

        /// <inheritdoc />
        public override void OnWin(BlackjackEngine engine)
        {
            if (doubledThisHand)
            {
                engine.Resources.Favor += 1;
            }
        }

        /// <inheritdoc />
        public override void OnLoss(BlackjackEngine engine)
        {
            if (doubledThisHand)
            {
                engine.Resources.HitPoints = Mathf.Max(0, engine.Resources.HitPoints - 1);
            }
        }
    }
}
