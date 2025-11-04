using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// First hit each hand is safer.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/LeftHandLuck", fileName = "LeftHandLuck")]
    public sealed class LeftHandLuckTrinketSO : TrinketSO
    {
        private bool usedThisHand;

        /// <inheritdoc />
        public override void OnHandStart(BlackjackEngine engine)
        {
            usedThisHand = false;
        }

        /// <inheritdoc />
        public override void OnAfterHit(BlackjackEngine engine)
        {
            if (usedThisHand)
            {
                return;
            }

            usedThisHand = true;
            Hand hand = engine.CurrentHand;
            if (hand.Cards.Count > 0)
            {
                CardInstance card = hand.Cards[hand.Cards.Count - 1];
                card.ValueOverride = Mathf.Max(1, card.ValueOverride - 1);
            }
        }
    }
}
