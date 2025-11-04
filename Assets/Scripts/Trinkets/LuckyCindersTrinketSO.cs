using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Awards favor when drawing golden cards.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/LuckyCinders", fileName = "LuckyCinders")]
    public sealed class LuckyCindersTrinketSO : TrinketSO
    {
        /// <summary>
        /// Called when a golden card is drawn.
        /// </summary>
        /// <param name="engine">Current engine.</param>
        public void OnGoldenCard(BlackjackEngine engine)
        {
            engine.Resources.Favor += 1;
        }
    }
}
