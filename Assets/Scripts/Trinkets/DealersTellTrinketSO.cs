using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Grants a free peek when the dealer shows a six.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/DealersTell", fileName = "DealersTell")]
    public sealed class DealersTellTrinketSO : TrinketSO
    {
        /// <inheritdoc />
        public override void OnInitialDeal(BlackjackEngine engine)
        {
            CardInstance? up = engine.GetDealerUpCard();
            if (up != null && up.Card.BaseValue == 6)
            {
                engine.RequestPeek();
            }
        }
    }
}
