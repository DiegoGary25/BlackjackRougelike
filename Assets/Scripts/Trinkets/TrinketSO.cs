using UnityEngine;
using HouseTakes21.Blackjack;

namespace HouseTakes21.Trinkets
{
    /// <summary>
    /// Base class for trinkets containing metadata and hook implementations.
    /// </summary>
    public abstract class TrinketSO : ScriptableObject, ITrinket
    {
        [SerializeField]
        private string trinketId = "";

        [SerializeField]
        private string displayName = "";

        [SerializeField]
        private TrinketRarity rarity;

        [SerializeField]
        [TextArea]
        private string description = "";

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public string TrinketId => trinketId;

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName => displayName;

        /// <summary>
        /// Gets the rarity.
        /// </summary>
        public TrinketRarity Rarity => rarity;

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description => description;

        /// <inheritdoc />
        public TrinketSO Definition => this;

        /// <summary>
        /// Initializes runtime state for the trinket when granted.
        /// </summary>
        /// <param name="rng">Seeded random generator.</param>
        public virtual void Initialize(HouseTakes21.Core.RNGService rng)
        {
        }

        /// <inheritdoc />
        public virtual void OnTableStart(BlackjackEngine engine)
        {
        }

        /// <inheritdoc />
        public virtual void OnHandStart(BlackjackEngine engine)
        {
        }

        /// <inheritdoc />
        public virtual void OnInitialDeal(BlackjackEngine engine)
        {
        }

        /// <inheritdoc />
        public virtual void OnBeforeHit(BlackjackEngine engine)
        {
        }

        /// <inheritdoc />
        public virtual void OnAfterHit(BlackjackEngine engine)
        {
        }

        /// <inheritdoc />
        public virtual void OnStand(BlackjackEngine engine)
        {
        }

        /// <inheritdoc />
        public virtual void OnDouble(BlackjackEngine engine)
        {
        }

        /// <inheritdoc />
        public virtual void OnSplit(BlackjackEngine engine)
        {
        }

        /// <inheritdoc />
        public virtual void OnBust(BlackjackEngine engine)
        {
        }

        /// <inheritdoc />
        public virtual void OnWin(BlackjackEngine engine)
        {
        }

        /// <inheritdoc />
        public virtual void OnLoss(BlackjackEngine engine)
        {
        }

        /// <inheritdoc />
        public virtual void OnExact21(BlackjackEngine engine)
        {
        }

        /// <inheritdoc />
        public virtual void OnPayout(BlackjackEngine engine)
        {
        }

        /// <inheritdoc />
        public virtual void OnTableEnd(BlackjackEngine engine)
        {
        }
    }
}
