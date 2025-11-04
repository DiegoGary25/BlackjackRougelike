using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Grants a bonus peek when hitting into a soft dealer.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/SoftStep", fileName = "SoftStep")]
    public sealed class SoftStepTrinketSO : TrinketSO
    {
        private bool usedThisHand;

        /// <inheritdoc />
        public override void OnHandStart(BlackjackEngine engine)
        {
            usedThisHand = false;
        }

        /// <inheritdoc />
        public override void OnBeforeHit(BlackjackEngine engine)
        {
            if (usedThisHand)
            {
                return;
            }

            CardInstance? up = engine.GetDealerUpCard();
            if (up != null && up.Card.IsAce)
            {
                engine.RequestPeek();
                usedThisHand = true;
            }
        }
    }
}
