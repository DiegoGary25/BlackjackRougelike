using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Split right-hand gains a hidden bonus.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/GlassVein", fileName = "GlassVein")]
    public sealed class GlassVeinTrinketSO : TrinketSO
    {
        /// <inheritdoc />
        public override void OnSplit(BlackjackEngine engine)
        {
            if (engine.PlayerHands.Count > 1)
            {
                Hand right = engine.PlayerHands[1];
                if (right.IsRightSplit)
                {
                    right.HiddenModifier += 1;
                }
            }
        }
    }
}
