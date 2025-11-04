using HouseTakes21.Blackjack;
using HouseTakes21.Core;
using HouseTakes21.Trinkets;
using UnityEngine;

namespace HouseTakes21.Items
{
    /// <summary>
    /// Heals HP at the cost of sanity.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Items/Cigarettes", fileName = "CigarettesItem")]
    public sealed class CigarettesItemSO : ItemSO
    {
        /// <inheritdoc />
        public override bool Activate(BlackjackEngine engine, GameResources resources, TrinketBus bus)
        {
            resources.HitPoints += 1;
            resources.Sanity = System.Math.Max(0, resources.Sanity - 1);
            return true;
        }
    }
}
