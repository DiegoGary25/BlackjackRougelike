using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Grants extra chips when defeating a dealer with 17.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/QuietLedger", fileName = "QuietLedger")]
    public sealed class QuietLedgerTrinketSO : TrinketSO
    {
        /// <inheritdoc />
        public override void OnPayout(BlackjackEngine engine)
        {
            if (engine.LastDealerTotal == 17)
            {
                engine.Resources.Chips += 1;
            }
        }
    }
}
