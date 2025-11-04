using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Restores sanity every third win.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/TallyMark", fileName = "TallyMark")]
    public sealed class TallyMarkTrinketSO : TrinketSO
    {
        private int winCounter;

        /// <inheritdoc />
        public override void OnTableStart(BlackjackEngine engine)
        {
            winCounter = 0;
        }

        /// <inheritdoc />
        public override void OnWin(BlackjackEngine engine)
        {
            winCounter++;
            if (winCounter % 3 == 0)
            {
                engine.Resources.Sanity = Mathf.Min(10, engine.Resources.Sanity + 1);
            }
        }
    }
}
