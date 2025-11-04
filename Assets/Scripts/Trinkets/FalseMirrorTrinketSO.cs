using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Refunds the first bust each table.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/FalseMirror", fileName = "FalseMirror")]
    public sealed class FalseMirrorTrinketSO : TrinketSO
    {
        private bool usedThisTable;

        /// <inheritdoc />
        public override void OnTableStart(BlackjackEngine engine)
        {
            usedThisTable = false;
        }

        /// <inheritdoc />
        public override void OnBust(BlackjackEngine engine)
        {
            if (!usedThisTable)
            {
                usedThisTable = true;
                engine.RefundBustHp = true;
            }
        }
    }
}
