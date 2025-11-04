using HouseTakes21.Blackjack;
using HouseTakes21.Core;
using HouseTakes21.Trinkets;
using UnityEngine;

namespace HouseTakes21.Items
{
    /// <summary>
    /// Removes a card from the player's hand and draws a replacement.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Items/Purge", fileName = "PurgeItem")]
    public sealed class PurgeItemSO : ItemSO
    {
        /// <inheritdoc />
        public override bool Activate(BlackjackEngine engine, GameResources resources, TrinketBus bus)
        {
            return engine.RequestPurge();
        }
    }
}
