using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Grants chips at the start of each table.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/GutterCoin", fileName = "GutterCoin")]
    public sealed class GutterCoinTrinketSO : TrinketSO
    {
        /// <inheritdoc />
        public override void OnTableStart(BlackjackEngine engine)
        {
            engine.Resources.Chips += 2;
        }
    }
}
