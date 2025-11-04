using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Provides an automatic peek at hand start.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/SnakesWhisper", fileName = "SnakesWhisper")]
    public sealed class SnakesWhisperTrinketSO : TrinketSO
    {
        /// <inheritdoc />
        public override void OnHandStart(BlackjackEngine engine)
        {
            engine.RequestPeek();
        }
    }
}
