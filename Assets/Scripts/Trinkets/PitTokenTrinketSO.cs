using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Prevents every third bust by reducing the total instead.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/PitToken", fileName = "PitToken")]
    public sealed class PitTokenTrinketSO : TrinketSO
    {
        private int bustCounter;

        /// <inheritdoc />
        public override void OnTableStart(BlackjackEngine engine)
        {
            bustCounter = 0;
        }

        /// <summary>
        /// Attempts to prevent a bust.
        /// </summary>
        /// <param name="engine">Current engine.</param>
        /// <param name="hand">Hand about to bust.</param>
        /// <returns>True if the bust was prevented.</returns>
        public bool TryPreventBust(BlackjackEngine engine, Hand hand)
        {
            bustCounter++;
            if (bustCounter % 3 != 0)
            {
                return false;
            }

            engine.Resources.Sanity = Mathf.Max(0, engine.Resources.Sanity - 1);
            hand.HiddenModifier -= 2;
            return true;
        }
    }
}
