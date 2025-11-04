using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Standing low grants sanity once per table.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/ColdPalm", fileName = "ColdPalm")]
    public sealed class ColdPalmTrinketSO : TrinketSO
    {
        private bool grantedThisTable;

        /// <inheritdoc />
        public override void OnTableStart(BlackjackEngine engine)
        {
            grantedThisTable = false;
        }

        /// <inheritdoc />
        public override void OnStand(BlackjackEngine engine)
        {
            if (grantedThisTable)
            {
                return;
            }

            if (engine.GetHandValue(engine.CurrentHand) <= 15)
            {
                engine.Resources.Sanity = Mathf.Min(10, engine.Resources.Sanity + 1);
                grantedThisTable = true;
            }
        }
    }
}
