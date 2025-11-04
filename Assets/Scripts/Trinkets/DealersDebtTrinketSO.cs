using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Awards chips when the dealer shows an ace.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/DealersDebt", fileName = "DealersDebt")]
    public sealed class DealersDebtTrinketSO : TrinketSO
    {
        /// <inheritdoc />
        public override void OnInitialDeal(BlackjackEngine engine)
        {
            CardInstance? up = engine.GetDealerUpCard();
            if (up != null && up.Card.IsAce)
            {
                engine.Resources.Chips += 1;
            }
        }
    }
}
