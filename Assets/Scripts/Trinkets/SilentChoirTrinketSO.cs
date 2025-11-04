using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Heals at table end if sanity is low.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/SilentChoir", fileName = "SilentChoir")]
    public sealed class SilentChoirTrinketSO : TrinketSO
    {
        /// <inheritdoc />
        public override void OnTableEnd(BlackjackEngine engine)
        {
            if (engine.Resources.Sanity <= 3)
            {
                engine.Resources.HitPoints += 1;
            }
        }
    }
}
