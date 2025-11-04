using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Exact 21 heals 1 HP.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/SaintsAsh", fileName = "SaintsAsh")]
    public sealed class SaintsAshTrinketSO : TrinketSO
    {
        /// <inheritdoc />
        public override void OnExact21(BlackjackEngine engine)
        {
            engine.Resources.HitPoints += 1;
        }
    }
}
