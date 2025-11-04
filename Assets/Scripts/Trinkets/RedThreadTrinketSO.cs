using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Guarantees odd/even pairing after a loss.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/RedThread", fileName = "RedThread")]
    public sealed class RedThreadTrinketSO : TrinketSO
    {
        /// <inheritdoc />
        public override void OnLoss(BlackjackEngine engine)
        {
            engine.RedThreadPending = true;
        }
    }
}
