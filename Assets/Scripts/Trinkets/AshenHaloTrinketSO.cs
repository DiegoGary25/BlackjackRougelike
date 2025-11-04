using UnityEngine;
using HouseTakes21.Blackjack;
using HouseTakes21.Core;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// May tip a total of twenty up to twenty-one.
    /// </summary>
    [CreateAssetMenu(menuName = "HouseTakes21/Trinkets/AshenHalo", fileName = "AshenHalo")]
    public sealed class AshenHaloTrinketSO : TrinketSO
    {
        private RNGService? rng;
        private bool triggeredThisHand;

        /// <inheritdoc />
        public override void Initialize(RNGService rng)
        {
            this.rng = rng;
        }

        /// <inheritdoc />
        public override void OnHandStart(BlackjackEngine engine)
        {
            triggeredThisHand = false;
        }

        /// <inheritdoc />
        public override void OnStand(BlackjackEngine engine)
        {
            if (triggeredThisHand || rng == null)
            {
                return;
            }

            if (engine.GetHandValue(engine.CurrentHand) == 20 && rng.RollChance(0.2f))
            {
                foreach (CardInstance card in engine.CurrentHand.Cards)
                {
                    if (card.ValueOverride == 10)
                    {
                        card.ValueOverride = 11;
                        triggeredThisHand = true;
                        break;
                    }
                }
            }
        }
    }
}
