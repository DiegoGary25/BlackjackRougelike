using HouseTakes21.Blackjack;
using HouseTakes21.Core;
using HouseTakes21.Trinkets;
using UnityEngine;

namespace HouseTakes21.Items
{
    /// <summary>
    /// Reveals the next card in the shoe for the current hand.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Items/Peek", fileName = "PeekItem")]
    public sealed class PeekItemSO : ItemSO
    {
        /// <inheritdoc />
        public override bool Activate(BlackjackEngine engine, GameResources resources, TrinketBus bus)
        {
            engine.RequestPeek();
            return true;
        }
    }
}
